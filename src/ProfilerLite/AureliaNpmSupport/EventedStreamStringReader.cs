using System;
using System.Text;

namespace ProfilerLite.AureliaNpmSupport
{
    internal class EventedStreamStringReader : IDisposable
    {
        private StringBuilder _stringBuilder = new StringBuilder();
        private EventedStreamReader _eventedStreamReader;
        private bool _isDisposed;

        public EventedStreamStringReader(EventedStreamReader eventedStreamReader)
        {
            EventedStreamReader eventedStreamReader1 = eventedStreamReader;
            if (eventedStreamReader1 == null)
                throw new ArgumentNullException(nameof(eventedStreamReader));
            this._eventedStreamReader = eventedStreamReader1;
            this._eventedStreamReader.OnReceivedLine += new EventedStreamReader.OnReceivedLineHandler(this.OnReceivedLine);
        }

        public string ReadAsString()
        {
            return this._stringBuilder.ToString();
        }

        private void OnReceivedLine(string line)
        {
            this._stringBuilder.AppendLine(line);
        }

        public void Dispose()
        {
            if (this._isDisposed)
                return;
            this._eventedStreamReader.OnReceivedLine -= new EventedStreamReader.OnReceivedLineHandler(this.OnReceivedLine);
            this._isDisposed = true;
        }
    }
}