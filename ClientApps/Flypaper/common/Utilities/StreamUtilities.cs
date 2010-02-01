using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.IO.Compression;

namespace Utilities
{
    public class StreamUtilities
    {
        #region Compression Utilities
        public static byte[] Compress( byte[] arrIn )
        {
            MemoryStream ms = new MemoryStream();
            GZipStream zip = new GZipStream( ms, CompressionMode.Compress );

            zip.Write( arrIn, 0, arrIn.Length );

            zip.Close();

            return ms.GetBuffer();
        }

        public static byte[] Decompress( byte[] arrIn )
        {
            MemoryStream ms = new MemoryStream( arrIn );
            GZipStream unzip = new GZipStream( ms, CompressionMode.Decompress );

            MemoryStream outStr = new MemoryStream();

            // need to read from stream - not sure of length
            byte[] bytes = new byte[4096];
            int n = 0;
            while( (n = unzip.Read( bytes, 0, bytes.Length )) != 0 )
            {
                outStr.Write( bytes, 0, n );
            }

            return outStr.GetBuffer();
        }
        #endregion
    }
}
