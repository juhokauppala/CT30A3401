using System;
using System.Net.Sockets;

namespace Shared
{
    public static class TcpIO
    {
        public static int HeaderBytes = 4;
        public static Message ReadStream(NetworkStream stream)
        {
            if (stream.CanRead)
            {
                byte[] headerBytes = new byte[HeaderBytes];
                byte[] bytes;
                Int32 length;

                if (!stream.DataAvailable)
                    return null;

                // 4-byte integer telling how long the message is
                stream.Read(headerBytes, 0, HeaderBytes);
                length = BitConverter.ToInt32(headerBytes, 0);

                // The actual message
                bytes = new byte[length];
                stream.Read(bytes, 0, length);

                return MessageEncoder.Decode(bytes);
            } else
            {
                throw new Exception("NetworkStream not readable");
            }
        }

        public static void WriteStream(NetworkStream stream, Message message)
        {
            if (stream.CanWrite)
            {
                byte[] data = MessageEncoder.Encode(message);
                Int32 length = data.Length;
                byte[] final = new byte[HeaderBytes + length];

                byte[] lengthInBytes = BitConverter.GetBytes(length);
                for (int i = 0; i < HeaderBytes; i++)
                {
                    final[i] = lengthInBytes[i];
                }
                for (int i = 0; i < length; i++)
                {
                    final[HeaderBytes + i] = data[i];
                }
                stream.Write(final, 0, length + HeaderBytes);
            }
            else
            {
                throw new Exception("NetworkStream not writable");
            }
        }
    }
}
