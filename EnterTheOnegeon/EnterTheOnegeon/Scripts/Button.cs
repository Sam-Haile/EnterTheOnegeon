using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace EnterTheOnegeon
{
    /// <summary>
    /// The class for a button with a paramaterized constructor to assign values to all of the fields
    /// an IsClicked method that returns true when the button is clicked, as well as an overridden Draw
    /// method that draws the button as well as its text in its center
    /// </summary>
    class Button
    {
        // Fields
        private SpriteFont font;
        public Texture2D buttText;
        private string text;
        public Vector2 textPos;
        private Rectangle buttRect;
        private Color textColor;

        // Properties
        public Rectangle ButtRect
        {
            get { return buttRect; }
        }

        public Texture2D Texture
        {
            get { return buttText; }
            set { value = buttText; }
        }


        // Constructor
        public Button(SpriteFont fnt, Texture2D buttonText, string name, Rectangle buttonRect, Color textColour)
        {
            font = fnt;
            buttText = buttonText;
            text = name;
            buttRect = buttonRect;
            Vector2 textSize = font.MeasureString(text);
            textPos = new Vector2(
                    (buttRect.X + buttRect.Width / 2) - textSize.X / 2,
                    (buttRect.Y + buttRect.Height / 2) - textSize.Y / 2);
            textColor = textColour;
        }

        public void Draw(SpriteBatch sb, Color color)
        {
            sb.Draw(
                buttText,
                buttRect,
                color);

            sb.DrawString(
                font,
                text,
                textPos,
                color);
        }
    }
}
