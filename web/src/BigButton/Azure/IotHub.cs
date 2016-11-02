using M2Mqtt;
using M2Mqtt.Messages;
using System;
using System.Text;

namespace BigButton.Azure
{
    public class IotHub
    {
        private readonly IotHubConfiguration _config;
        private static MqttClient _client;

        public IotHub(IotHubConfiguration config)
        {
            _config = config;
        }

        void Connect()
        {
            if (_client != null && _client.IsConnected) return;
            _client = new MqttClient(_config.Uri, 8883, true, null, null, MqttSslProtocols.TLSv1_2);
            _client.Connect(_config.DeviceId, $"{_config.Uri}/{_config.DeviceId}", _config.SasKey, true, 3600);
            if(!_client.IsConnected) throw new Exception("Failed to connect to the cloud");
        }

        public void SendMessage(string color)
        {
            if (string.IsNullOrWhiteSpace(color)) return;
            Connect();
            var message = $"{{'color':'{color}', 'TimeStamp': '{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}' }}";
            var bytes = Encoding.UTF8.GetBytes(message);
            _client.Publish(_config.Hub, bytes, MqttMsgBase.QOS_LEVEL_AT_LEAST_ONCE, false);
        }

        public void Disconnect()
        {
            _client.Disconnect();
        }
    }
}
