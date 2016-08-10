using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PokemonGo.RocketAPI;
using PokemonGo.RocketAPI.Enums;
using PokemonGo.RocketAPI.Extensions;
using POGOProtos.Enums;
using POGOProtos.Networking.Envelopes;

namespace Website.Service
{
    public static class PokemonGoApiHack
    {
        static PokemonGoApiHack()
        {
            Settings = SetDeviceInfo(
                new MySettings
                {
                    AuthType = AuthType.Google,
                    DefaultLatitude = -43.532116,
                    DefaultLongitude = 172.619092,
                    GoogleUsername = ConfigurationManager.AppSettings["GoogleUsername"],
                    GooglePassword = ConfigurationManager.AppSettings["GooglePassword"]
                });
        }

        public static ISettings Settings { get; }

        public static Client GetClient()
        {
            return new Client(Settings, new MyStrategy());
        }

        public static int GetPokemonId(string name)
        {
            PokemonId pkId;
            if (Enum.TryParse(name, true, out pkId) && pkId != PokemonId.Missingno)
            {
                return (int) pkId;
            }
            return -1;
        }

        private static MySettings SetDeviceInfo(MySettings mySettings)
        {
            mySettings.DeviceId = "123456789123";
            mySettings.AndroidBoardName = "Huawei";
            mySettings.AndroidBootloader = "Google";
            mySettings.DeviceBrand = "Huawei";
            mySettings.DeviceModel = "Y123";
            mySettings.DeviceModelIdentifier = "Y123";
            mySettings.DeviceModelBoot = "Google";
            mySettings.HardwareManufacturer = "Huawei";
            mySettings.HardwareModel = "Y123";
            mySettings.FirmwareBrand = "Huawei";
            mySettings.FirmwareTags = "";
            mySettings.FirmwareType = "";
            mySettings.FirmwareFingerprint = "";
            return mySettings;
        }

        private class MySettings : ISettings
        {
            public AuthType AuthType { get; set; }
            public double DefaultLatitude { get; set; }
            public double DefaultLongitude { get; set; }
            public double DefaultAltitude { get; set; }
            public string GoogleRefreshToken { get; set; }
            public string PtcPassword { get; set; }
            public string PtcUsername { get; set; }
            public string GoogleUsername { get; set; }
            public string GooglePassword { get; set; }
            public string DeviceId { get; set; }
            public string AndroidBoardName { get; set; }
            public string AndroidBootloader { get; set; }
            public string DeviceBrand { get; set; }
            public string DeviceModel { get; set; }
            public string DeviceModelIdentifier { get; set; }
            public string DeviceModelBoot { get; set; }
            public string HardwareManufacturer { get; set; }
            public string HardwareModel { get; set; }
            public string FirmwareBrand { get; set; }
            public string FirmwareTags { get; set; }
            public string FirmwareType { get; set; }
            public string FirmwareFingerprint { get; set; }
        }

        private class MyStrategy : IApiFailureStrategy
        {
            public Task<ApiOperation> HandleApiFailure(RequestEnvelope request, ResponseEnvelope response)
            {
                return Task.FromResult(ApiOperation.Retry);
            }

            public void HandleApiSuccess(RequestEnvelope request, ResponseEnvelope response)
            {

            }
        }
    }
}
