using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        public DeviceConnection(string comPort)
        {
            _arduinoConnectivity = new ArduinoConnectivityUWP();
            _comPort = comPort;
        }

        public async void PlayAnimationSequence(List<IMediaMarker> markers)
        {
            await _arduinoConnectivity.Connect(_comPort);

            foreach (var marker in markers)
            {
                await _arduinoConnectivity.UploadCompressedSequence(marker.Text);
                await Task.Delay(marker.Time);
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
