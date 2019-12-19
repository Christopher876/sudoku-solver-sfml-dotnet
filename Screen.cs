using SFML;
using SFML.System;
using SFML.Graphics;
using SFML.Window;
using System;
using System.Collections.Generic;

namespace sudoku_solver
{
    public class Screen
    {
        private string title;
        private VideoMode videoMode;
        private RenderWindow window;
        private Font sourceCodeFont;
        private readonly uint fontSize; 

        private Text text=new Text(){
            DisplayedString = "1",
            FillColor = Color.Red,
            Style = Text.Styles.Bold,
            CharacterSize = 45,
            Position = new Vector2f(30,0),
            Font = new Font("./SourceCodeVariable-Roman.ttf"),            
        };
        private Text text2=new Text(){
            DisplayedString = "2",
            FillColor = Color.Blue,
            Style = Text.Styles.Bold,
            CharacterSize = 45,
            Position = new Vector2f(120+90+90+90,67), //120
            Font = new Font("./SourceCodeVariable-Roman.ttf"),            
        };
        public Screen(uint width,uint height,string title,Styles styles){
            this.videoMode = new VideoMode();
            this.videoMode.Width = width;
            this.videoMode.Height = height;
            this.title = title;
            this.window = new RenderWindow(videoMode,this.title,styles);
            this.sourceCodeFont = new Font("./SourceCodeVariable-Roman.ttf");
            this.fontSize = (uint)(window.Size.X/9/2);

            //Setup key events
            this.window.KeyPressed += Window_KeyPressed;
            //Setup Window Events
            this.window.Closed += (sender,e) => {(this.window).Close();};
            text.CharacterSize = fontSize;
            text2.CharacterSize = fontSize;
            text2.Position = new Vector2f(30+(90*5),(window.Size.Y/9)*3);
        }

        private void Window_KeyPressed(object sender, KeyEventArgs e)
        {
            if(e.Code == Keyboard.Key.Escape)
                this.window.Close();
            if(e.Code == Keyboard.Key.Up){
                text2.Position = new Vector2f(text2.Position.X,text2.Position.Y+1);
                Console.WriteLine(text2.Position);
            }
            if(e.Code == Keyboard.Key.Down){
                text2.Position = new Vector2f(text2.Position.X,text2.Position.Y-1);
                Console.WriteLine(text2.Position);
            }
            if(e.Code == Keyboard.Key.Right){
                text2.Position = new Vector2f(text2.Position.X+1,text2.Position.Y);
                Console.WriteLine(text2.Position);
            }
            if(e.Code == Keyboard.Key.Down){
                text2.Position = new Vector2f(text2.Position.X-1,text2.Position.Y);
                Console.WriteLine(text2.Position);
            }
        }

        private List<RectangleShape> DrawGrid(int size = 50){
            List<RectangleShape> rectangles = new List<RectangleShape>();

            float rectSizeX = window.Size.X / 9;
            float rectSizeY = window.Size.Y / 9;

            for(int y = 0; y < window.Size.Y/50;y++){
                for(int x = 0; x < window.Size.X/50; x++){
                    float thickness = 2f;
                    if(x % 3 == 0)
                        thickness = 4f;
                    rectangles.Add(new RectangleShape(new Vector2f(rectSizeX,rectSizeY)){
                        Position = new Vector2f(rectSizeX*x,rectSizeY*y),
                        OutlineColor = Color.Black,
                        OutlineThickness = 1f,
                    });
                }
            }
            return rectangles;
        }

        private List<Text> DrawText(){
            Random random = new Random();
            List<Text> numbers = new List<Text>();
            for(int y = 0; y < 9;y++){
                for(int x = 0; x < 9; x++){
                    numbers.Add(new Text(){
                        DisplayedString = (random.Next(0,10)).ToString(),
                        FillColor = Color.Red,
                        Style = Text.Styles.Bold,
                        CharacterSize = 45,
                        Position = new Vector2f(30+(90*x),(window.Size.Y/9)*y+5),
                        Font = new Font("./SourceCodeVariable-Roman.ttf"),            
                });
                }
            }
            return numbers;
        }

        //Draw the lines to segment the 3x3 cells
        private List<RectangleShape> DrawSeparationLines(){
            float rectSizeX = window.Size.X / 9;
            float rectSizeY = window.Size.Y / 9;
            float rectCellX = rectSizeX * 3;
            float rectCellY = rectSizeY * 3;

            List<RectangleShape> lines = new List<RectangleShape>();
            for(int x = 1; x < window.Size.X/50/3-2; x++){
                lines.Add(new RectangleShape(new Vector2f(5,window.Size.Y)){
                        FillColor = Color.Black,
                        Position = new Vector2f(rectCellX*x,0),
                    });    
            }
            for(int y = 1; y < window.Size.Y/50/3-1;y++){
                lines.Add(new RectangleShape(new Vector2f(window.Size.X,5)){
                    FillColor = Color.Black,
                    Position = new Vector2f(0,y*rectCellY),
                });
            }
            return lines;
        }

        public void Loop(){
            List<RectangleShape> rectangles = DrawGrid();
            List<RectangleShape> lines = DrawSeparationLines();
            List<Text> numbers = DrawText();
            while(this.window.IsOpen){                
                this.window.DispatchEvents();
                for(int i = 0; i < rectangles.Count;i++){
                    window.Draw(rectangles[i]);
                }
                for(int i = 0; i < lines.Count;i++){
                    window.Draw(lines[i]);
                }
                for(int i = 0; i < numbers.Count;i++){
                    window.Draw(numbers[i]);
                }
                //window.Draw(text);
                //window.Draw(text2);
                this.window.Display();
            }
        }   
    }
}