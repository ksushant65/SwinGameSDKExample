using System;
using SwinGameSDK;
using Core = SwinGameSDK.SwinGame;
using System.Windows.Forms;
using System.Collections.Generic;


namespace MyGame
{
	//Class Shape
	public abstract class  Shape{
		private Color _color;
		private float _x, _y;
		private bool _selected;

		//Constructor for class Shape
		public Shape(float x, float y){
			_color = Color.Green;
			_x = x;
			_y = y;
			_selected = false;
		}

		public Shape (Color clr) : this (0, 0){
			_color = clr;
		}

		public Shape () : this(0, 0){}

		//Get and Set Property for _color
		public Color color{
			get{
				return _color;
			}
			set{
				_color = value;
			}
		}

		//Get and Set Property for _x
		public float X{
			get{ 
				return _x;
			}
			set{ 
				_x = value;
			}
		}

		//Get and Set Property for _y
		public float Y{
			get{ 
				return _y;
			}
			set{ 
				_y = value;
			}
		}
			

		//Readonly Property for selected
		public bool Selected{
			get{ 
				return _selected;
			}
			set{ 
				_selected = value;
			}
		}

		//Function to Draw the Shape on Screen
		public abstract void Draw();

		//Function to check if the Mouse Pointer is inside the Drawn Shape.
		public abstract bool IsAt(Point2D cur_pos);

		//Function to Generate Random Color.
		public static Color getRandomColor(){
			Random randomGen = new Random();
			System.Drawing.KnownColor[] names = (System.Drawing.KnownColor[]) Enum.GetValues(typeof(System.Drawing.KnownColor));
			System.Drawing.KnownColor randomColorName = names[randomGen.Next(names.Length)];
			System.Drawing.Color randomColor = System.Drawing.Color.FromKnownColor(randomColorName);
			return Color.FromArgb(randomColor.ToArgb());
		}

		//Function to Draw Outline of the Shape
		public abstract void DrawOutline();
	}

	public class Rectangle : Shape{

		private int _width, _height;

		public Rectangle(Color clr, float x, float y, int width, int height) : base(clr){
			_width = width;
			_height = height;
			X = x;
			Y = y;
		}
		//Get and Set Property for _width
		public int width{
			get{ 
				return _width;
			}
			set{ 
				_width = value;
			}
		}

		//Get and Set Property for _height.
		public int height{
			get{ 
				return _height;
			}
			set{ 
				_height = value;
			}
		}

		public override void Draw(){
			SwinGame.FillRectangle (color, X, Y, width, height);
		}

		public override bool IsAt(Point2D cur_pos){
			if (Core.PointInRect (cur_pos, X, Y, width, height))
				return true;
			else
				return false;
		}

		public override void DrawOutline ()
		{
			Core.DrawThickLine (Color.Black, X-4, Y-4, X-4, Y+_height+4, 2);
			Core.DrawThickLine (Color.Black, X-4, Y-4, X+_width+4, Y-4, 2);
			Core.DrawThickLine (Color.Black, X-4, Y+_height+4, X+_width+4, Y+_height+4, 2);
			Core.DrawThickLine (Color.Black, X+_width+4, Y-4, X+_width+4, Y+_height+4, 2);
		}
	}

	public class Circle : Shape{
		
		public int _radius;
		public Circle(Color clr,float x,float y,int radius){
			color = clr;
			X = x;
			Y = y;
			_radius = radius;
		}
		public Circle() : base(Color.Blue)
		{
			_radius = 50;
		}
		public override void Draw (){
			SwinGame.FillCircle (color, X,Y,_radius);
		}

		//Function to create a Thick Border if the Circle is selected.
		public override void DrawOutline ()
		{
			SwinGame.DrawCircle (Color.Black,X,Y,_radius + 2);
			SwinGame.DrawCircle (Color.Black,X,Y,_radius + 3);
			SwinGame.DrawCircle (Color.Black, X, Y, _radius + 4);
		}

		public override bool IsAt(Point2D cur_pos){
			if (Core.PointInCircle (cur_pos, X, Y, _radius))
				return true;
			else
				return false;
		}
	}

	public class Line : Shape{

		private float x1, x2, y1, y2;

		public Line(Color clr, float x,float y) : base(){
			color = clr;
			x1 = x;
			y1 = y;
			x2 = x1 + 50;
			y2 = y1;
		}

		public override void Draw(){
			SwinGame.DrawThickLine (color, x1, y1, x2, y2, 3);
		}

		public override bool IsAt(Point2D cur_pos){
			if (SwinGame.PointOnLine (cur_pos, x1, y1, x2, y2))
				return true;
			else
				return false;
		}

		public override void DrawOutline(){
			
			SwinGame.DrawCircle (Color.Black, x1, y1, 3);
			SwinGame.DrawCircle (Color.Black, x1, y1, 4);

			SwinGame.DrawCircle (Color.Black, x2, y2, 3);
			SwinGame.DrawCircle (Color.Black, x2, y2, 4);
		}
	}

	public class Drawing{
		private readonly List<Shape> _shapes;
		private Color _background;

		public Drawing () : this(Color.White){}

		public Drawing(Color background){
			_background = background;
			_shapes = new List<Shape> ();
			ShapeCount = 0;
		}

		public Color backgroundColor{
			get{ return _background;}
			set{ _background = value;}
		}

		public List<Shape> SelectedShape{
			get{
				List<Shape> result = new List<Shape>();
				foreach (Shape s in _shapes){
					if (s.Selected)
						result.Add (s);
				}
				return result;
			}
		}

		public int ShapeCount;

		public int shapeCount{
			get{ 
				return ShapeCount;
			}
			set{ 
				ShapeCount = value;
			}
		}

		//Function Draw to display the added shapes on Screen.
		public void Draw(){
			Graphics.ClearScreen (_background);
			foreach (Shape s in _shapes){
				s.Draw ();
			}
			foreach (Shape s in SelectedShape){
				s.DrawOutline ();
			}
		}

		//Function to select a shape if the Mouse Pointer is inside it.
		public void SelectedShapesAt(Point2D pt){
			foreach (Shape s in _shapes){
				if (s.IsAt (Core.MousePosition())){
					s.Selected = true;
				}
			}
		}

		//Function to add the shape in the List _shapes.
		public void AddShape(Shape s){
			_shapes.Add (s);
		}

		//Function to remove a shape from the List _shapes.
		public void RemoveShape(Shape s){
			_shapes.Remove (s);
		}
	}

	public class GameMain  
	{
		private enum ShapeKind{
			Rectangle,
			Circle,
			Line
		}

		public static void Main()
		{

			ShapeKind kindToAdd = ShapeKind.Circle;
			Drawing drawing = new Drawing();
			//Open the game window
			Core.OpenGraphicsWindow ("GameMain", 768, 600);
			do
			{
				if(Core.MouseClicked(MouseButton.RightButton)){
					if(Core.MouseClicked(MouseButton.RightButton)){
						drawing.SelectedShapesAt(Core.MousePosition());
						drawing.Draw();
					}

				}
				if(Core.MouseClicked(MouseButton.LeftButton)){
					Point2D cur_pos = Core.MousePosition();
					Shape newShape;
					if(kindToAdd == ShapeKind.Rectangle){
						newShape = new Rectangle(Color.Green,cur_pos.X,cur_pos.Y,100,100);
						drawing.AddShape(newShape);
					}
					if(kindToAdd == ShapeKind.Circle){
						newShape = new Circle(Color.Blue,cur_pos.X,cur_pos.Y,50);
						drawing.AddShape(newShape);
					}
					if(kindToAdd == ShapeKind.Line){
						newShape = new Line(Color.Red,cur_pos.X,cur_pos.Y);
						drawing.AddShape(newShape);
					}
					drawing.Draw();
				}

				if(Core.KeyTyped(KeyCode.vk_SPACE)){
					drawing.backgroundColor = Shape.getRandomColor();
					Core.ClearScreen(drawing.backgroundColor);
					drawing.Draw();
				}

				if(Core.KeyTyped(KeyCode.vk_DELETE) || Core.KeyTyped(KeyCode.vk_BACKSPACE)){
					foreach(Shape s in drawing.SelectedShape){
						drawing.RemoveShape(s);
					}
					drawing.Draw();
				}

				if(Core.KeyTyped(KeyCode.vk_r)){
					kindToAdd = ShapeKind.Rectangle;
				}
				
				if(Core.KeyTyped(KeyCode.vk_c)){
					kindToAdd = ShapeKind.Circle;		
				}

				if(Core.KeyTyped(KeyCode.vk_l)){
					kindToAdd = ShapeKind.Line;
				}

				Core.ProcessEvents();
				Core.RefreshScreen();
				Core.ProcessEvents();		
			} while(!Core.WindowCloseRequested());
		}
	}
}