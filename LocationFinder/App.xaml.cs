using System;
using LocationFinder.Services;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Forms.Maps;

namespace LocationFinder
{
    public partial class App : Application
    {
        Label lblLat, lblLng;
        IMyLocation loc;

        Map map;

        public App()
        {
            lblLat = new Label
            {
                HorizontalTextAlignment = TextAlignment.Center,
                Text = "Lat",
            };

            lblLng = new Label
            {
                HorizontalTextAlignment = TextAlignment.Center,
                Text = "Lng",
            };

            map = new Map(MapSpan.FromCenterAndRadius(
                        new Position(37, -122),
                        Distance.FromMiles(10)))
            {
                VerticalOptions =
                LayoutOptions.FillAndExpand,
                BackgroundColor = Color.Blue,
                IsShowingUser = true,
            };

            // The root page of your application
            var content = new ContentPage
            {
                Title = "MainPage",
                Content = new StackLayout
                {
                    //VerticalOptions = LayoutOptions.Center,
                    Children = {
                    new Label {
                        HorizontalTextAlignment =
                                    TextAlignment.Center,
                        Text = "Current Location"
                    },
                    lblLat,
                    lblLng,
                    map
                }
                }
            };

            MainPage = new NavigationPage(content);
        }

        protected override void OnStart()
        {
            // Handle when your app starts
            loc = DependencyService.Get<IMyLocation>();
            loc.LocationObtained += (object sender,
                ILocationEventArgs e) =>
            {
                var lat = e.lat;
                var lng = e.lng;
                lblLat.Text = lat.ToString();
                lblLng.Text = lng.ToString();

                var position = new Position(lat, lng);
                map.MoveToRegion(
                    MapSpan.FromCenterAndRadius(
                    new Position(position.Latitude,
                                 position.Longitude),
                    Distance.FromMiles(1)));
            };
            loc.GetMyLocation();
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}