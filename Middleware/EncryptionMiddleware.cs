using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using System.IO;

namespace APIAuthentication.Middleware
{
    public class EncryptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly bool _isEncrypted;

        // Demo key/IV (32 bytes for AES-256, 16 bytes for IV)
        private static readonly byte[] AesKey = Encoding.UTF8.GetBytes("0123456789ABCDEF0123456789ABCDEF");
        private static readonly byte[] AesIV = Encoding.UTF8.GetBytes("ABCDEF0123456789");

        public EncryptionMiddleware(RequestDelegate next, bool isEncrypted)
        {
            _next = next;
            _isEncrypted = isEncrypted;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (_isEncrypted)
            {
                context.Request.EnableBuffering();
                context.Request.Body = await DecryptRequestAsync(context.Request.Body);
            }

            // Capture the response
            var originalBodyStream = context.Response.Body;
            using var memStream = new MemoryStream();
            context.Response.Body = memStream;

            await _next(context);

            if (_isEncrypted)
            {
                memStream.Seek(0, SeekOrigin.Begin);
                var encrypted = await EncryptResponseAsync(memStream);
                context.Response.ContentLength = encrypted.Length;
                context.Response.Body = originalBodyStream;
                await context.Response.Body.WriteAsync(encrypted, 0, encrypted.Length);
            }
            else
            {
                memStream.Seek(0, SeekOrigin.Begin);
                await memStream.CopyToAsync(originalBodyStream);
            }
        }

        private async Task<Stream> DecryptRequestAsync(Stream body)
        {
            using var ms = new MemoryStream();
            await body.CopyToAsync(ms);
            var encryptedBytes = ms.ToArray();

            if (encryptedBytes.Length == 0)
                return new MemoryStream();

            using var aes = Aes.Create();
            aes.Key = AesKey;
            aes.IV = AesIV;
            using var decryptor = aes.CreateDecryptor();
            var decryptedBytes = decryptor.TransformFinalBlock(encryptedBytes, 0, encryptedBytes.Length);

            var decryptedStream = new MemoryStream(decryptedBytes);
            decryptedStream.Seek(0, SeekOrigin.Begin);
            return decryptedStream;
        }

        private async Task<byte[]> EncryptResponseAsync(Stream body)
        {
            using var ms = new MemoryStream();
            await body.CopyToAsync(ms);
            var plainBytes = ms.ToArray();

            if (plainBytes.Length == 0)
                return Array.Empty<byte>();

            using var aes = Aes.Create();
            aes.Key = AesKey;
            aes.IV = AesIV;
            using var encryptor = aes.CreateEncryptor();
            var encryptedBytes = encryptor.TransformFinalBlock(plainBytes, 0, plainBytes.Length);

            return encryptedBytes;
        }
    }
}
