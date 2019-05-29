using System;

namespace GoogleUltra.Radio
{
    public class UltraFmRadioSettings : IRadioSettings
    {
        public Uri RadioInfoUri { get; } = new Uri("https://fmgid.com/stations/ultra/current.json");
    }
}