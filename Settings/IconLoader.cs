using System;
using System.Drawing;
using System.IO;

namespace SMS_Search.Settings
{
    /// <summary>
    /// Utility class that loads embedded Base64-encoded icons for the Settings TreeView.
    /// Used to avoid managing separate image resources for each node.
    /// </summary>
    public static class IconLoader
    {
        // Icons from FamFamFam Silk (Legacy Icons)

        // General: application_view_columns.png
        private const string ICON_GENERAL = "iVBORw0KGgoAAAANSUhEUgAAABAAAAAQCAYAAAAf8/9hAAAABGdBTUEAAK/INwWK6QAAABl0RVh0U29mdHdhcmUAQWRvYmUgSW1hZ2VSZWFkeXHJZTwAAAF/SURBVDjLpZN/S8JgEMd9i1JURPSDIoIkS1FQI4iQgihStExrhGmydGObyZyyYRaybBRFQb/8vxcgBIF92275ApoHx7jjns/37p49LgCuQdzlXmEXd8RON1L4QPjM9NwbQtkXBE+eEWCe4D9+hC99j+XDO3j3b+FJ3CCcvu5a5wgQLXV6ceUT/3Xv3mWPAJayE5/fboAA4dw7nNjspmoDQqevlDAMA+12G61WC1/fP1BVFfV6HbIsUyyKIgRBAMdxVD8drf0BzIU5scl12QZY27ZM13VSbzQapFir1VCtViFJEsUsy6JQKCCfz1P9xFrFBlhX5cTGVyUb4D96oESz2SR1RVFIsVKpoFwuo1gsUpzNZsEwDDKZDNWPhQUb4D0wHHUwHCjZgKVEmxKaptHc/ZmtL8/zNLMVp1IpJJNJxGIxqh/yn9sAT1x31IHbx6L/FtiF3Sv6s+a2NMxE65jaUMwtX9CixiIiRkM8RoKc2XbRVGZhnrGcJcDAr3FQwC803UMOARws7QAAAABJRU5ErkJggg==";

        // Database: database.png
        private const string ICON_DATABASE = "iVBORw0KGgoAAAANSUhEUgAAABAAAAAQCAQAAAC1+jfqAAAABGdBTUEAAK/INwWK6QAAABl0RVh0U29mdHdhcmUAQWRvYmUgSW1hZ2VSZWFkeXHJZTwAAAEYSURBVBgZBcHPio5hGAfg6/2+R980k6wmJgsJ5U/ZOAqbSc2GnXOwUg7BESgLUeIQ1GSjLFnMwsKGGg1qxJRmPM97/1zXFAAAAEADdlfZzr26miup2svnelq7d2aYgt3rebl585wN6+K3I1/9fJe7O/uIePP2SypJkiRJ0vMhr55FLCA3zgIAOK9uQ4MS361ZOSX+OrTvkgINSjS/HIvhjxNNFGgQsbSmabohKDNoUGLohsls6BaiQIMSs2FYmnXdUsygQYmumy3Nhi6igwalDEOJEjPKP7CA2aFNK8Bkyy3fdNCg7r9/fW3jgpVJbDmy5+PB2IYp4MXFelQ7izPrhkPHB+P5/PjhD5gCgCenx+VR/dODEwD+A3T7nqbxwf1HAAAAAElFTkSuQmCC";

        // Advanced: wrench.png
        private const string ICON_ADVANCED = "iVBORw0KGgoAAAANSUhEUgAAABAAAAAQCAYAAAAf8/9hAAAABGdBTUEAAK/INwWK6QAAABl0RVh0U29mdHdhcmUAQWRvYmUgSW1hZ2VSZWFkeXHJZTwAAAH0SURBVDjLlZPLbxJRGMX5X/xbjBpjjCtXLl2L0YWkaZrhNQwdIA4FZxygC22wltYYSltG1HGGl8nopCMPX9AUKQjacdW4GNPTOywak7ZAF/eRe/M73/nOzXUAcEwaqVTKmUgkGqIoWoIgWP/fTYSTyaSTgAfdbhemaSIej+NcAgRudDod9Pt95PN5RKPR8wnwPG/Z1XVdB8dxin0WDofBsiyCwaA1UYBY/tdqtVAqlRCJRN6FQiE1k8mg2WyCpunxArFY7DKxfFir1VCtVlEoFCBJEhRFQbFYhM/na5wKzq/+4ALprzqxbFUqFWiaBnstl8tQVRWyLMPr9R643W7nCZhZ3uUS+T74jR7Y5c8wDAO5XA4MwxzalklVy+PxNCiKcp4IkbbhzR4K+h9IH02wax3MiAYCgcBfv99/4TS3xxtfepcTCPyKgGl5gCevfyJb/Q3q6Q5uMcb7s3IaTZ6lHY5f70H6YGLp7QDx9T0kSRtr5V9wLbZxw1N/fqbAHIEXsj1saQR+M8BCdg8icbJaHOJBqo3r1KfMuJdyuBZb2NT2R5a5l108JuFl1CHuJ9q4NjceHgncefSN9LoPcYskT9pYIfA9Al+Z3X4xzUdz3H74RbODWlGGeCYPcVf4jksz08HHId6k63USFK7ObuOia3rYHkdyavlR+267GwAAAABJRU5ErkJggg==";

        // CleanSql: bin.png
        private const string ICON_CLEANSQL = "iVBORw0KGgoAAAANSUhEUgAAABAAAAAQCAQAAAC1+jfqAAAABGdBTUEAAK/INwWK6QAAABl0RVh0U29mdHdhcmUAQWRvYmUgSW1hZ2VSZWFkeXHJZTwAAAFuSURBVBgZBcG/S1RxAADwz3teyp3XFUUWNVSoRGQR3dLQIESBbUZt9gekm9XW2lRbNDv0gxbJWoJoCcT+ABskTgcDDwLpOD19d+/73rfPJ4kAANaejUx03t5eBZIIgKe34r3JB7OTVVvZuzf9lderiKIoip7MLba+xY24H4v4N36PC635uSgFIJ2/Pz7ppH19w66aHk/nqQCfk8LU1BWJAyMyo3Y1bV2nwpeh8nxxthg+Vm+ZUFVKHDjhK1UqlJeK52E61LOkasOhRDAic8EWKp/qxaupmdOO6Fi3bVyiEAQdA6Th7tjMGYcyDTcdtWlUoqYtypHmjy/atadrX6JpU5QaMhDlSPNTFX9kMj0H6rr+gYFCjnSw3XNZ2y9dPfT1lUq5UkA6+Phb3TU3NJArHFeKhtTkSBc+rC//0NBQVbNmwphzGu5oCztUGDz8udydbSrlVmI9eSkIirzYKZokESw+yl+EdtgL75eWAID/yIWfXhcZhKEAAAAASUVORK5CYII=";

        // Launcher: control_play_blue.png
        private const string ICON_LAUNCHER = "iVBORw0KGgoAAAANSUhEUgAAABAAAAAQCAYAAAAf8/9hAAAABGdBTUEAAK/INwWK6QAAABl0RVh0U29mdHdhcmUAQWRvYmUgSW1hZ2VSZWFkeXHJZTwAAAJfSURBVDjLpZNNbxJRFIb7A/wF/A5YunTDrpouujNxY8LGxEVTVyU11UVjCmEsUUyb1gYqEWuqtqmRWukUimksH6UMHwIW6FCYwWFgYBjKcc6FGam68ybvZuY87/m4544BwNiobiyCQZVJlVnV5FDm4TfDn/Gj4DVVxgdvBIvv4IwKHafp2MkpF40nuP2jJP1qL0dNeXkLxmDsFYMhfN0TKFujp1mGrQkgSl1QLvtEjZYMpQoPwaM4s7STtWKsZqIZGBGOJ7+L7Y4CeCS5B7zYBU5Vs9Mj30RJhv1wRHRtpdDESAywLywbM2twVZCh8lOGt+EKsHUZyvUOlPiObrKzG2TurbHYjgENTD76B4Vlj8II3noYgI3DCoHPam0iPMncOTi8IQpZNDAHv6Vo7BlLRVDLenN2j+h1iCVwodoGoaXARV2C5fV3NLJoMBmJnXA4rFqjS2DMWOTaKvyZaOJRCPwxDnIViRjJyiWsudc5ZInBcTRODLB8DcZAAs8dwPiMn/zLstKwii4sr7zUDcxfviboutiBhqTovWLgxBx9Bc6ct8jNpIt1cLjcegsmtz9DFUo16PeBgPkLiZQ7PvOJwAimyy1IlVrQ7fVh9zABVucHfYiG+56qxR8IM5wwmDJmQyGsgclSkyTIqNntz1aZO8704Bq1RXJsRK2bHwMiyw8C601FrwaXCTOnizzYXB5x2rH1e5FGV3neHbauejeZUCQDBVYgM8GeE3kOtgNRmHcsMVP293+v8uhjuvsib5l9vk09WVyhHU+d3IKd4h7bXPS0zUfdppL/fkz/85x/AR14FVfMwp4lAAAAAElFTkSuQmCC";

        // Update: drive_web.png
        private const string ICON_UPDATE = "iVBORw0KGgoAAAANSUhEUgAAABAAAAAQCAYAAAAf8/9hAAAABGdBTUEAAK/INwWK6QAAABl0RVh0U29mdHdhcmUAQWRvYmUgSW1hZ2VSZWFkeXHJZTwAAAJASURBVDjLhZLNa9NwGMe/WRKXpp19L6WrdVsZHgq9aBF2cFDRDi8WxJPssqMXDx4H+xuGFDz1YC+eBvUgnWDc0FqoL+CmHlylthurdWqG7bY0zUvtL5CSxYOBh+f5/fJ8P89LQg0GA9ifu68XMzOB8INJ/kL8WKGwf/y5WW817z/KrBXtuZQdMBRfuz5z+emcb4E97LvwtXsG3aMOfiiP1Y0Pwu3ineenIGN24nm//+GsN8U2dQ3bf4BnByJe0luIhsKM1+Fatecz9ovZs9NT7+QaPFoKG3sStOgOPrFPQP92YtoTif4XoOkyTmTgTUvHN5EBdxKFo7sEyr2Jnlr7Z1+jEarVqlCpVAa7P0U6pEg4kmqgxjgcfPdAP9xDnAPqu7/oQqEwyOfzwinAUDzvcDjSyWQSVzxZ7Oy/RSZE45JXw9w5BTeTW/jSfo+l1D1ks1kEAoF0LpdLj0ZQVXXF5/Oh3W4jPD6Ji+O3UNxeg6q9AsP28bHVwo2pRfBdHo1GA/F4HPV6fXkofUGVy+V5nuc3Y7EYOp0O+v3+yIZgkM9MURRomgbLspiYmIDT6YQgCAR2lVEUZSUYDGLojSSO4wwz/w/irbGu6+j1ekgkEqjVassMqSJJEkRRhCzLoyRN0wxvns07cmYYBm632+iQANKkMmnZTLAL7GfiXS4X6TpNRjBIxMyq1sp2iPnO1DGm0BTbIfZRzJ2Q2AAQkt/vH1WyJpjLI7F1ocQikcgIsF4qlRbMlqwjWWPrmJau1/8CtF7RM3ksOU0AAAAASUVORK5CYII=";

        // Logging: application_xp_terminal.png
        private const string ICON_LOGGING = "iVBORw0KGgoAAAANSUhEUgAAABAAAAAQCAYAAAAf8/9hAAAABGdBTUEAAK/INwWK6QAAABl0RVh0U29mdHdhcmUAQWRvYmUgSW1hZ2VSZWFkeXHJZTwAAAGNSURBVDjLpVM9SwNBEJ297J1FQBtzjQj2dgppYiP4A1KZRoiFrYWt9rHyH6QUPBDTCimtLNSAnSB26YKg4EdMdsd5611cjwsIWRhmZ3f2zZuPVcxMsyx9fPF0NRfS2vM7lx2WtcQiJHvDRvZMluXMGNHstJH7+Wj09jHkOy1+tc3VxeC+P6TXT1sYZX2hT7cvS6lepv3zHUp2T8vXNw81dXT2yGwEGeERSbSVCC5qysYa+3vm9sJGmLFojceXJ9uklCqUIAic5G3IytahAAhqqVSiwWDwx6nogW9XKhWphaGAvC50Oh1qtVr/7oAdCwBQwjB00mg0qFqtUr1ed3YURZM7X7TWTqM2Gm3CASRJEur1etTtdp1DnrafFtJGMbVNGSBas9l0DrAzR6x8DdwASUB0RqNNGS2/gH7EInvCwMhkZTnlnX0GsP09tJER0BgMoAEAa1rETDIQvBkjBZeHMIjjuNB5Ggg0/oZWPGrHGwd7Fp9F2CAlgHKqf0aYXb6Y2mzE8d/IfrXVrN/5G81p6oa2mIEUAAAAAElFTkSuQmCC";


        /// <summary>
        /// Retrieves an Image object for the specified setting category.
        /// </summary>
        public static Image GetIcon(string name)
        {
            switch (name)
            {
                case "General": return SystemIcons.Application.ToBitmap();
                case "Application": return SystemIcons.Application.ToBitmap();
                case "Display": return LoadImage(ICON_GENERAL);
                case "Database": return LoadImage(ICON_DATABASE);
                case "Advanced": return LoadImage(ICON_ADVANCED);
                case "Search": return LoadImage(ICON_ADVANCED);
                case "Behavior": return LoadImage(ICON_ADVANCED);
                case "CleanSql": return LoadImage(ICON_CLEANSQL);
                case "Launcher": return LoadImage(ICON_LAUNCHER);
                case "Update": return LoadImage(ICON_UPDATE);
                case "Logging": return LoadImage(ICON_LOGGING);
                default: return SystemIcons.Application.ToBitmap();
            }
        }

        private static Image LoadImage(string base64)
        {
            try
            {
                byte[] bytes = Convert.FromBase64String(base64);
                using (var ms = new MemoryStream(bytes))
                {
                    // Create a copy of the bitmap so we can close the stream
                    return new Bitmap(Image.FromStream(ms));
                }
            }
            catch
            {
                return SystemIcons.Error.ToBitmap();
            }
        }
    }
}
