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
                try
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
                } catch (Exception e)
                {
                    return null;
                }
            } else
            {
                return null;
            }
        }

        public static void WriteStream(NetworkStream stream, Message message)
        {
            byte[] data = MessageEncoder.Encode(message);
            WriteData(stream, data);
        }

        private static void WriteData(NetworkStream stream, byte[] data)
        {
            if (stream.CanWrite)
            {
                try
                {

                    Int32 length = data.Length;
                    byte[] final = new byte[HeaderBytes + length];

                    byte[] lengthInBytes = BitConverter.GetBytes(length);
                    lengthInBytes.CopyTo(final, 0);
                    data.CopyTo(final, HeaderBytes);
                    stream.Write(final, 0, length + HeaderBytes);
                
                } catch (Exception e) { }
            }
            else
            {
                throw new Exception("NetworkStream not writable");
            }
        }
    }
}
