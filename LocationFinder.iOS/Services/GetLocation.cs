using System;
using CoreLocation;
using LocationFinder.iOS.Services;
using LocationFinder.Services;

[assembly: Xamarin.Forms.Dependency(typeof(GetLocation))]
namespace LocationFinder.iOS.Services
{
    public class LocationEventArgs : EventArgs,
            ILocationEventArgs
    {
        public double lat { get; set; }
        public double lng { get; set; }
    }

    public class GetLocation : IMyLocation
    {
        CLLocationManager lm;

        public event EventHandler<ILocationEventArgs> LocationObtained;

        event EventHandler<ILocationEventArgs>
            IMyLocation.LocationObtained
        {
            add
            {
                LocationObtained += value;
            }
            remove
            {
                LocationObtained -= value;
            }
        }

        public void GetMyLocation()
        {
            lm = new CLLocationManager();
            lm.DesiredAccuracy = CLLocation.AccuracyBest;
            lm.DistanceFilter = CLLocationDistance.FilterNone;

            //---fired whenever there is a change in location---
            lm.LocationsUpdated +=
                (object sender, CLLocationsUpdatedEventArgs e) =>
                {
                    var locations = e.Locations;
                    var strLocation =
                        locations[locations.Length - 1].
                            Coordinate.Latitude.ToString();
                    strLocation = strLocation + "," +
                        locations[locations.Length - 1].
                            Coordinate.Longitude.ToString();

                    LocationEventArgs args =
                        new LocationEventArgs();
                    args.lat = locations[locations.Length - 1].
                        Coordinate.Latitude;
                    args.lng = locations[locations.Length - 1].
                        Coordinate.Longitude;
                    LocationObtained(this, args);
                };

            lm.AuthorizationChanged += (object sender,
                CLAuthorizationChangedEventArgs e) =>
            {
                if (e.Status ==
                    CLAuthorizationStatus.AuthorizedWhenInUse)
                {
                    lm.StartUpdatingLocation();
                }
            };

            lm.RequestWhenInUseAuthorization();
        }

        //---stop the location update when the object is set to
        // null---
        ~GetLocation()
        {
            lm.StopUpdatingLocation();
        }
    }

}
