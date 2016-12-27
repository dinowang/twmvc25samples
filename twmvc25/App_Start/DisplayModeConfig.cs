using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Optimization;
using System.Web.WebPages;
using Antlr.Runtime.Misc;
using Hexdigits.DisplayModeMatrix;

namespace twmvc25
{
    public class DisplayModeConfig
    {
        public static void RegisterBasic(DisplayModeProvider instance)
        {
            instance.Modes.Clear();

            instance.Modes.Add(new DefaultDisplayMode("Preview")
            {
                ContextCondition = x => x.Request.Cookies.AllKeys.Contains("Preview")
            });

            instance.Modes.Add(new DefaultDisplayMode());
        }

        public static void RegisterHarder(DisplayModeProvider instance)
        {
            Func<HttpContextBase, bool> IsMobile = x => x.GetOverriddenBrowser().IsMobileDevice;
            Func<HttpContextBase, bool> IsTablet = x => Regex.IsMatch(x.GetOverriddenUserAgent(), "iPad|Tablet", RegexOptions.IgnoreCase);
            Func<HttpContextBase, string> CurrentTheme = x => x.Request.Cookies.AllKeys.Contains("Theme") ? x.Request.Cookies["Theme"].Value : string.Empty;
            Func<HttpContextBase, bool> IsPreview = x => x.Request.Cookies.AllKeys.Contains("Preview");

            instance.Modes.Clear();

            instance.Modes.Add(new DefaultDisplayMode("Tablet-Dark-Preview")
            {
                ContextCondition = x => IsTablet(x) && CurrentTheme(x) == "dark" && IsPreview(x)
            });

            instance.Modes.Add(new DefaultDisplayMode("Mobile-Dark-Preview")
            {
                ContextCondition = x => IsMobile(x) && CurrentTheme(x) == "dark" && IsPreview(x)
            });

            instance.Modes.Add(new DefaultDisplayMode("Tablet-Dark")
            {
                ContextCondition = x => IsTablet(x) && CurrentTheme(x) == "dark"
            });

            instance.Modes.Add(new DefaultDisplayMode("Mobile-Dark")
            {
                ContextCondition = x => IsMobile(x) && CurrentTheme(x) == "dark"
            });

            instance.Modes.Add(new DefaultDisplayMode("Dark-Preview")
            {
                ContextCondition = x => CurrentTheme(x) == "dark" && IsPreview(x)
            });

            instance.Modes.Add(new DefaultDisplayMode("Tablet-Preview")
            {
                ContextCondition = x => IsTablet(x) && IsPreview(x)
            });

            instance.Modes.Add(new DefaultDisplayMode("Mobile-Preview")
            {
                ContextCondition = x => IsMobile(x) && IsPreview(x)
            });

            instance.Modes.Add(new DefaultDisplayMode("Tablet")
            {
                ContextCondition = x => IsTablet(x)
            });

            instance.Modes.Add(new DefaultDisplayMode("Mobile")
            {
                ContextCondition = x => IsMobile(x)
            });

            instance.Modes.Add(new DefaultDisplayMode("Dark")
            {
                ContextCondition = x => CurrentTheme(x) == "dark"
            });

            instance.Modes.Add(new DefaultDisplayMode("Preview")
            {
                ContextCondition = x => IsPreview(x)
            });

            instance.Modes.Add(new DefaultDisplayMode());
        }

        public static void Register(DisplayModeProvider instance)
        {
            var builder = new DisplayModeMatrixBuilder();

            var profiles = builder
                            .SetEvaluateBehavior(EvaluateBehavior.Lazy)
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
