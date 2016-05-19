using UnityEngine;
using System;
using System.Collections;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography;
using System.Text;

namespace hg.ApiWebKit.apis.google
{
	public class GoogleJsonWebToken
	{
		private models.OAuthJwtAccessTokenClientInformation _clientInfo;
	
		private string createSerializedHeader()
		{
			return hg.LitJson.JsonMapper.ToJson(
				new
				{
					alg = "RS256",
					typ = "JWT"
				}
			);	
		}
		
		private string getSerializedPayload()
		{
			var issued = (int)(DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalSeconds;
			
			return hg.LitJson.JsonMapper.ToJson(
				new
				{
					iss = _clientInfo.ClientIdEmail,
					scope = _clientInfo.Scope,
					aud = _clientInfo.Audience,
					exp = issued + 3600,
					iat = issued
				}
			);
		}
	
		public string GetAssertion(models.OAuthJwtAccessTokenClientInformation clientInfo)
		{
			_clientInfo = clientInfo;
		
			string serializedHeader = createSerializedHeader();
			string serializedPayload = getSerializedPayload();
			
			StringBuilder assertion = new StringBuilder();
			assertion
				.Append(urlSafeBase64Encode(serializedHeader))
					.Append(".")
					.Append(urlSafeBase64Encode(serializedPayload));
			
			
			string signature = "";
			X509Certificate2 certificate = null;
			
			TextAsset bindata = Resources.Load(_clientInfo.CertificateResourceName) as TextAsset;
			/*System.Security.SecureString secure = new System.Security.SecureString();
			foreach (char c in "notasecret")
			{
			    secure.AppendChar(c);
			}*/
			certificate = new X509Certificate2(bindata.bytes); //, secure);  //TODO: broken as of 5.3
			RSACryptoServiceProvider rsa = (RSACryptoServiceProvider)certificate.PrivateKey;
			byte[] privateKeyBlob = rsa.ExportCspBlob(true);
			RSACryptoServiceProvider key = new RSACryptoServiceProvider();
			key.ImportCspBlob(privateKeyBlob);
			signature = urlSafeBase64Encode(key.SignData(Encoding.ASCII.GetBytes(assertion.ToString()), "SHA256"));
			
			/*	
			//TODO http://stackoverflow.com/questions/243646/how-to-read-a-pem-rsa-private-key-from-net
			string privateKey = "-----BEGIN PRIVATE KEY-----\nMIICdgIBADANBgkqhkiG9w0BAQEFAASCAmAwggJcAgEAAoGBANVWyJtw90cSp11Q\nefvnuQTyW2TiLG+IlQZMjgrA2nEIliYsUuJAxd2j7MnRRt0wdztf8qKxc+106nQV\ngqm03QAluQJX113e6qu7pWSOYMnWonk/cRPpEdmolG0zi+TMt0Wll1z3nFFJiS0f\nj7JN/5SCO0qSEdD5Qud90eXWOO59AgMBAAECgYBkysqefeGmJ48BDEuFkzYbuzEo\n2Z6q1zmpLzQQqorJyoe940UJdhbFn3P46bH0QLikSbGF4hbmQk2eqKcB7NxYH5bE\niml6h7Lt8426BESUHbTTAJJT3dbyPZ0uJ5wAw9CUIP69S0O//K8+t6QcG3U8Lo7e\nioKO1kee5b8wneOZlQJBAO9nMDJDhVGokJb2HjYe9iRm9fZwGTfN/kPEc2QlYr54\ny0L1Bq3ftnRJ3xEfRgd4s9UrabhQDaJyseqWXVpaFxsCQQDkIQXA9OU3f9qVUkW5\nhkHnyMw0fyLS17pzIQUfV32I2srFCCmbq+0i/R3ligOoGihFDyy7Bi+Cg4oNx3Tl\nf/JHAkEA61KfCYldbxsmpX1fzQs6ICYk+AzQfQ47NRnR40rseRX7luGLozYX/s7u\nOcJn78gx3QRDcy0deeUMBe+v67RKqQJAIjQZSr7tBw8yVKULMy+//eKLS2usavRR\nTiWrQPG7LqOvNy9sHZz/ZvmQW/P/bFrPotsNl9TrlqPmNP6stiQizQJAOvfmzFJF\n33wQfJNdTFNJHcrbZbAf5Kuh3V3uqhHGYi1ZC2PHhNANMW2X3214JpGNww2U1xlb\nLyzCa9sq2GwbYA\u003d\u003d\n-----END PRIVATE KEY-----\n";
			byte[] certBuffer = getBytesFromPEM(privateKey,"PRIVATE KEY");
			//certificate = new X509Certificate2(certBuffer);
			HMACSHA256 sha = new HMACSHA256(certBuffer);
			signature = urlSafeBase64Encode(sha.ComputeHash(Encoding.ASCII.GetBytes(assertion.ToString())));
			*/
			
			assertion
				.Append(".")
				.Append(signature);
			
			return assertion.ToString();
		}
	
		private string urlSafeBase64Encode(string value)
		{
			return urlSafeBase64Encode(Encoding.UTF8.GetBytes(value));
		}
		
		private string urlSafeBase64Encode(byte[] bytes)
		{
			return Convert.ToBase64String(bytes).Replace("=", String.Empty).Replace('+', '-').Replace('/', '_');
		}
	
		/*private byte[] getBytesFromPEM( string pemString, string section )
		{
			var header = String.Format("-----BEGIN {0}-----", section);
			var footer = String.Format("-----END {0}-----", section);
			
			var start = pemString.IndexOf(header, StringComparison.Ordinal) + header.Length;
			var end = pemString.IndexOf(footer, start, StringComparison.Ordinal) - start;
			
			if( start < 0 || end < 0 )
			{
				return null;
			}
			
			return Convert.FromBase64String( pemString.Substring( start, end ) );
		}*/
	}
}
