using System;
namespace LocationFinder.Services
{
    public interface IMyLocation
    {
        void GetMyLocation();
        event EventHandler<ILocationEventArgs>
            LocationObtained;
    }

    public interface ILocationEventArgs
    {
        double lat { get; set; }
        double lng { get; set; }
    }
}
