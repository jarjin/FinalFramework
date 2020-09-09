using System;
using System.Collections.Generic;
using UnityEngine;

namespace FirClient.Manager
{
    public class FontManager : BaseManager
    {
        private Dictionary<string, Font> fonts = new Dictionary<string, Font>();
        
        public override void Initialize()
        {
        }

        public void LoadFonts(Action initOK = null)
        {
            resMgr.LoadAssetAsync<Font>("Fonts", null, delegate(UnityEngine.Object[] objs)
            {
                foreach (var item in objs)
                {
                    var font = item as Font;
                    if (font != null)
                    {
                        this.AddFont(font);
                    }
                }
                this.InitFont("FZZZHONGHJW", string.Empty);
                if (initOK != null) initOK();
            });
        }

        private void InitFont(string fontName, string content)
        {
            var font = GetFont(fontName);
            if (font != null && !string.IsNullOrEmpty(content))
            {
                font.RequestCharactersInTexture(content);
            }
        }

        private void AddFont(Font font)
        {
            if (!fonts.ContainsKey(font.name))
            {
                fonts.Add(font.name, font);
            }
        }

        public Font GetFont(string fontName)
        {
            Font font = null;
            fonts.TryGetValue(fontName, out font); 
            return font;
        }

        public override void OnUpdate(float deltaTime)
        {
        }

        public override void OnDispose()
        {
            throw new System.NotImplementedException();
        }
    }
}

