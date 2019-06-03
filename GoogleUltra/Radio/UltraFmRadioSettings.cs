using System;

namespace GoogleUltra.Radio
{
    public class UltraFmRadioSettings : IRadioSettings
    {
        public Uri RadioInfoUri { get; } = new Uri("https://fmgid.com/stations/ultra/current.json");
        public Uri StreamAddress { get; } = new Uri("http://nashe2.hostingradio.ru/ultra-128.mp3");
    }
}