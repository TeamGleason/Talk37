using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.Media;

/*
<Capabilities>
    <DeviceCapability Name="serialcommunication">
        <Device Id="any">
            <Function Type="name:serialPort" />
        </Device>
    </DeviceCapability>
</Capabilities>
*/

namespace TeamGleason.Talk37.ComSupport
{
    public class DeviceConnection
    {
        DeviceConnection(string comPort)
        {
            _arduinoConnectivity = new ArduinoConnectivityUWP();
            _comPort = comPort;
        }

        public async Task<DeviceConnection> CreateAsync(string comPort)
        {
            var connection = new DeviceConnection(comPort);
            await connection._arduinoConnectivity.Connect(_comPort);
            return connection;
        }

        public async void PlayAnimationSequence(List<IMediaMarker> markers)
        {
            var startTime = DateTime.UtcNow;

            foreach (var marker in markers)
            {
                var cueTime = startTime + marker.Time;
                var delay = cueTime - DateTime.UtcNow;
                if (TimeSpan.Zero < delay)
                {
                    await Task.Delay(delay);
                }

                await _arduinoConnectivity.UploadCompressedSequence(marker.Text);
            }
        }

        public async void ClearDisplay()
        {
            await _arduinoConnectivity.ClearSequence();
        }

        private ArduinoConnectivityUWP _arduinoConnectivity;
        private string _comPort;
    }
}
