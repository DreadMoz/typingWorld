// using System.IO;
// using System.Net;
// using System.Text;
// using Google.Apis.Auth.OAuth2;
// using UnityEngine;

// public static class GoogleServiceAccount
// {
//     // GASから取得したSheetInfoを格納する静的プロパティ
//     public static string SheetInfo { get; set; }

//     private static ICredential _credential;

//     public static ICredential GetCredential(string[] scopes)
//     {
//         // GASアクセスなしのための直接代入
//         SheetInfo = "{\n  \"type\": \"service_account\",\n  \"project_id\": \"nekotapu\",\n  \"private_key_id\": \"284b2687a6520be459f37bc75785fc1cf2355385\",\n  \"private_key\": \"-----BEGIN PRIVATE KEY-----\\nMIIEvQIBADANBgkqhkiG9w0BAQEFAASCBKcwggSjAgEAAoIBAQCGbrCEtB/mzV/l\\nBEr/knQoPOUJWJh6MaM4MNdKHoh8KZP+xaX2Sw+etqz6ksUS45gdu4hLNCEzXz0z\\nn63jr4Cb4Wvffo3AbPQtAQ2hif49vYyHlcv7t7F8kl/QDCfp/uHhZT/0AlDie4aV\\n+nQLTEFQdZh4pxxqB686IYq7c+FnLvN31u8oGv/M67v72ysQIkJ8Vb7l64tx578F\\nIUfugDru8Jgn72qrtdCa3usKAmXPQGP0m4q0m54BDVisaXeLKR4dtUZWBv5zK+yG\\nxYVtPXALCVpIcTGuHH6O4bQIWXlLaSl6+8POhRP4EyXWNZJcpyNce8/StE75qp/M\\nmiXhMYINAgMBAAECggEADd3NB2MtBzmX8p+GvOX2ZVAir9wbnj8QfvNKwbJ0kZoY\\nUedBPy5u5gghv5b0DQa84hQ98sPlyM7CGVkXLq0jrvCJl/wN0xNp/FpndYouV1/9\\n5t5ktAo2nIrcpzEZzOElm4I+HoU+op3rO/0hiyjKc+otWASkwasZP/3FuaAyV9ox\\nq+gFTZy2t8nfwZ1amfYCN9CwjcevzPzCGbuz41u5cN3JoQWidXfZM3fipZ4ceDKV\\nT1i0eWwM9MHwqf5+NBaK3BPY39j38YTJ5YUleoiGnHct7fPlJ7ncjLCmz2we2+z9\\nkTSs/+xEPkyX/ObyLnD1qGnbAuJUbP2jn3ZySb9eAQKBgQC87QtumIezNT6KUq3L\\n87+8EDMgxF2+ddPZdacyAgxp/lx9FltK1e72dFZU8KLdGliH3he/KD7OqmxwC55O\\nU3pp6tad15xJIgKaJmTLoX4fAYN6BDbsFQaOF5P+vR2ekmdz3xHTeenRglcS8moe\\n5Ky5Ysg+Squcy//fCm3H0J7l/QKBgQC2KOAOvPFa9sh6PSFlf7tvyG1vOv5KO4bA\\nqBmILLG9rfT1PnHCGnYDOi9I8HHveUI8Ww8C6L66GcaSvejX7UchxT9q2nJKR0mb\\n6HFGYXwEMxyooXhCu/aSmo1/+GpFqlGiSbojqMfwkF0CeTGVMi3C/kOlpm42mNYN\\nhEqY1NPBUQKBgQCQbBkII3vg5/v2G7wWJDBXIH8lNld/SG15WDJGzUAWKscjLFr/\\ne0kgh9CTQB0QLpqsyn+WtrpEnA0nHgqXT8NNgqMrqG4ljeU1V9JHxB04sJyEQwKF\\nOJF5P9b3rjQdS0fgUQ88fX7blOrhZgTvttm/Ih93VveXdm8UXtGLJRTDNQKBgDWk\\nNc7BPwW9dG3iwInmImkZiXhe0/FCND1ZgyNBnhmwN1lcyR0Ss7vhj3kYLUUK3UFu\\nwy3lIf0lh/9AY2fqnK7KKhGqQEu1UGzT9z91h5KpBoB2BDcKqOKlFpQfsBPRwvZa\\nZbEIi9BAgtMuozY/L5CjVYJbT0tOmIIked8llTHxAoGAfNhlmjghSJ/BLoZ5Le6D\\nlQncUZHELojprOL7ulkSvp89yaJBWEtNniZPV3F5qg73tXBascR4j8l8TPbrMA+s\\nn7qj5KwQ6MKUMn/EAIcX02NceE1BjkSIyKtxs+uJ5TKUcyaST0ZSfkfO9A+XVVr9\\na06qNfRuTeZE2uMNU9cs/4M=\\n-----END PRIVATE KEY-----\\n\",\n  \"client_email\": \"id-348@nekotapu.iam.gserviceaccount.com\",\n  \"client_id\": \"104223563352387841429\",\n  \"auth_uri\": \"https://accounts.google.com/o/oauth2/auth\",\n  \"token_uri\": \"https://oauth2.googleapis.com/token\",\n  \"auth_provider_x509_cert_url\": \"https://www.googleapis.com/oauth2/v1/certs\",\n  \"client_x509_cert_url\": \"https://www.googleapis.com/robot/v1/metadata/x509/id-348%40nekotapu.iam.gserviceaccount.com\",\n  \"universe_domain\": \"googleapis.com\"\n}\n";

//         if (_credential != null)
//         {
//             return _credential;
//         }

//         // JSON 文字列から MemoryStream を生成
//         using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(SheetInfo)))
//         {
//             // ストリームから GoogleCredential を生成
//             _credential = GoogleCredential.FromStream(stream)
//                             .CreateScoped(scopes)
//                             .UnderlyingCredential;
//         }

//         return _credential;
//     }
// }