using System.IO;
using System.Net;
using System.Text;
using Google.Apis.Auth.OAuth2;
using UnityEngine;

public static class GoogleServiceAccount
{
    // GASから取得したSheetInfoを格納する静的プロパティ
    public static string SheetInfo { get; set; }

    private static ICredential _credential;

    public static ICredential GetCredential(string[] scopes)
    {
        if (_credential != null)
        {
            return _credential;
        }

        // JSON 文字列から MemoryStream を生成
        using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(SheetInfo)))
        {
            // ストリームから GoogleCredential を生成
            _credential = GoogleCredential.FromStream(stream)
                            .CreateScoped(scopes)
                            .UnderlyingCredential;
        }

        return _credential;
    }
}