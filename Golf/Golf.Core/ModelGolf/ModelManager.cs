using Golf.Core.ModelGolf.Cam;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Golf.Core.ModelGolf
{
    /// <summary>
    /// The first implementation of the model manager
    /// </summary>
    public class ModelManager
    {
        public List<GameObject> Elements;
        public ModelManager()
        {
            Elements = new List<GameObject>();
        }

        public void HandleModelCollision(GameObject currentObject)
        {
            foreach (var element in Elements)
            {
                if(currentObject != element)
                {
                    //currentObject.HandleModelCollision(element);
                }
            }
        }

        public void AddModel(GameObject model)
        {
            Elements.Add(model);
        }


        /*public void Draw(GameTime gameTime, Camera camera)
        {
            foreach (GameObject element in elements)
            {
                element.Draw(gameTime, camera);
            }
                
        }*/

        public void Update(GameTime gameTime)
        {
            foreach (GameObject element in Elements)
            {
                element.Update(gameTime);         
            }
        }


    }
}
