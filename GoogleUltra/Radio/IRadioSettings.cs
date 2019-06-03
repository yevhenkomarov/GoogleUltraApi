using System;

namespace GoogleUltra.Radio
{
    public interface IRadioSettings
    {
        Uri RadioInfoUri { get; }
        Uri StreamAddress { get; }
    }
}