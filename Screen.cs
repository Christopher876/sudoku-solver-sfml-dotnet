using SFML;
using SFML.System;
using SFML.Graphics;
using SFML.Window;
using System;

namespace sudoku_solver
{
    public class Screen
    {
        private string title;
        private VideoMode videoMode;
        private RenderWindow window; 
        public Screen(uint width,uint height,string title){
            this.videoMode = new VideoMode();
            this.videoMode.Width = width;
            this.videoMode.Height = height;
            this.title = title;
            this.window = new RenderWindow(videoMode,this.title);

            //Setup key events
            this.window.KeyPressed += Window_KeyPressed;
            //Setup Window Events
            this.window.Closed += (sender,e) => {(this.window).Close();};
        }

        private void Window_KeyPressed(object sender, KeyEventArgs e)
        {
            if(e.Code == Keyboard.Key.Escape)
                this.window.Close();
        }
        public void Loop(){
            var circle = new SFML.Graphics.CircleShape(100f)
            {
                FillColor = SFML.Graphics.Color.Blue,
                Position = new Vector2f(200,200)
            };

            var rect = new RectangleShape(new Vector2f(100,100)){
                FillColor = Color.White,
                Position = new Vector2f(100,100)
            };

            while(this.window.IsOpen){                
                this.window.DispatchEvents();
                this.window.Draw(circle);
                this.window.Draw(rect);
                this.window.Display();
            }
        }   
    }
}