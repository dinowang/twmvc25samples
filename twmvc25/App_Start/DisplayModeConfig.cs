using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Optimization;
using System.Web.WebPages;
using Hexdigits.DisplayModeMatrix;

namespace twmvc25
{
    public class DisplayModeConfig
    {
        public static void Register(DisplayModeProvider instance)
        {
            var builder = new DisplayModeMatrixBuilder();

            var profiles = builder
                            .AddOptionalFactor("Device", o => o
                                     .Evidence("Tablet", x => Regex.IsMatch(x.GetOverriddenUserAgent(), @"\b(iPad|Tablet)\b"))
                                     .Evidence("Mobile", x => x.GetOverriddenBrowser().IsMobileDevice))
                            .AddOptionalFactor("Theme", o => o
                                     .Evidence("Dark", x => x.Request.Cookies.AllKeys.Contains("Theme") && x.Request.Cookies["Theme"].Value == "dark")
                                     .Evidence("Light", x => x.Request.Cookies.AllKeys.Contains("Theme") && x.Request.Cookies["Theme"].Value == "light"))
                            .AddOptionalFactor("Preview", o => o
                                     .Evidence("Preview", x => x.Request.Cookies.AllKeys.Contains("Preview")))
                            .Build();

            instance.Modes.Clear();

            foreach (var profile in profiles)
            {
                instance.Modes.Add(new DefaultDisplayMode(profile.Name)
                {
                    ContextCondition = profile.ContextCondition
                });
            }

            instance.Modes.Add(new DefaultDisplayMode());
        }
    }
}
