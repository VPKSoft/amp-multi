using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VPKSoft.Utils;

namespace amp.UtilityClasses.Settings
{
    /// <summary>
    /// A class to store user theme settings.
    /// </summary>
    public class ThemeSettings: XmlSettings
    {
        /// <summary>
        /// Gets or sets the grey background.
        /// </summary>
        /// <value>The grey background.</value>
        public Color GreyBackground { get; set; } = Color.FromArgb(220, 223, 225);

        /// <summary>
        /// Gets or sets the header background.
        /// </summary>
        /// <value>The header background.</value>
        public Color HeaderBackground { get; set; } = Color.FromArgb(210, 213, 215);

        /// <summary>
        /// Gets or sets the blue background.
        /// </summary>
        /// <value>The blue background.</value>
        public Color BlueBackground { get; set; } = Color.FromArgb(200, 203, 205);

        /// <summary>
        /// Gets or sets the dark blue background.
        /// </summary>
        /// <value>The dark blue background.</value>
        public Color DarkBlueBackground { get; set; } = Color.FromArgb(190, 193, 195);

        /// <summary>
        /// Gets or sets the dark background.
        /// </summary>
        /// <value>The dark background.</value>
        public Color DarkBackground { get; set; } = Color.FromArgb(225, 228, 231);

        /// <summary>
        /// Gets or sets the medium background.
        /// </summary>
        /// <value>The medium background.</value>
        public Color MediumBackground { get; set; } = Color.FromArgb(215, 218, 221);

        /// <summary>
        /// Gets or sets the light background.
        /// </summary>
        /// <value>The light background.</value>
        public Color LightBackground { get; set; } = Color.FromArgb(192, 193, 194);

        /// <summary>
        /// Gets or sets the lighter background.
        /// </summary>
        /// <value>The lighter background.</value>
        public Color LighterBackground { get; set; } = Color.FromArgb(182, 183, 184);

        /// <summary>
        /// Gets or sets the lightest background.
        /// </summary>
        /// <value>The lightest background.</value>
        public Color LightestBackground { get; set; } = Color.FromArgb(128, 128, 128);

        /// <summary>
        /// Gets or sets the light border.
        /// </summary>
        /// <value>The light border.</value>
        public Color LightBorder { get; set; } = Color.FromArgb(201, 201, 201);

        /// <summary>
        /// Gets or sets the dark border.
        /// </summary>
        /// <value>The dark border.</value>
        public Color DarkBorder { get; set; } = Color.FromArgb(171, 171, 171);

        /// <summary>
        /// Gets or sets the light text.
        /// </summary>
        /// <value>The light text.</value>
        public Color LightText { get; set; } = Color.FromArgb(50, 50, 50);

        /// <summary>
        /// Gets or sets the disabled text.
        /// </summary>
        /// <value>The disabled text.</value>
        public Color DisabledText { get; set; } = Color.FromArgb(113, 113, 113);

        /// <summary>
        /// Gets or sets the blue highlight.
        /// </summary>
        /// <value>The blue highlight.</value>
        public Color BlueHighlight { get; set; } = Color.DodgerBlue;

        /// <summary>
        /// Gets or sets the blue selection.
        /// </summary>
        /// <value>The blue selection.</value>
        public Color BlueSelection { get; set; } = Color.FromArgb(75, 110, 175);

        /// <summary>
        /// Gets or sets the grey highlight.
        /// </summary>
        /// <value>The grey highlight.</value>
        public Color GreyHighlight { get; set; } = Color.FromArgb(182, 188, 182);

        /// <summary>
        /// Gets or sets the grey selection.
        /// </summary>
        /// <value>The grey selection.</value>
        public Color GreySelection { get; set; } = Color.FromArgb(160, 160, 160);

        /// <summary>
        /// Gets or sets the dark grey selection.
        /// </summary>
        /// <value>The dark grey selection.</value>
        public Color DarkGreySelection { get; set; } = Color.FromArgb(202, 202, 202);

        /// <summary>
        /// Gets or sets the dark blue border.
        /// </summary>
        /// <value>The dark blue border.</value>
        public Color DarkBlueBorder { get; set; } = Color.FromArgb(171, 181, 198);

        /// <summary>
        /// Gets or sets the light blue border.
        /// </summary>
        /// <value>The light blue border.</value>
        public Color LightBlueBorder { get; set; } = Color.FromArgb(206, 217, 114);

        /// <summary>
        /// Gets or sets the active control.
        /// </summary>
        /// <value>The active control.</value>
        public Color ActiveControl { get; set; } = Color.FromArgb(159, 178, 196);
    }
}
