using Golf.Core.ModelGolf.Cam;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Golf.Core.ModelGolf
{
    
    public class ModelManager
    {
        public List<GameObject> elements;
        public ModelManager()
        {
            elements = new List<GameObject>();
        }

        public void HandleModelCollision(GameObject currentObject)
        {
            foreach (var element in elements)
            {
                if(currentObject != element)
                {
                    //currentObject.HandleModelCollision(element);
                }
            }
        }

        public void AddModel(GameObject model)
        {
            elements.Add(model);
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
            foreach (GameObject element in elements)
            {
                element.Update(gameTime);         
            }
        }


    }
}
