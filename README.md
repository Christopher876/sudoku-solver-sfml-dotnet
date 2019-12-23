# Sudoku Solver using the Backtracking Algorithm

This is a project that utilizes the backtracking algorithm to solve sudoku puzzles. The project is written using C#.Net and SFML.NET.

Generating new sudoku boards with varying difficulty uses the API, [sugoku](https://github.com/berto/sugoku). 

The goal of this project was to get me to be more comfortable in a few different areas of programming:

- Implementing an algorithm to solve a real world puzzle without the use of initial tutorials
- Using an OpenGL GUI interface without the use of software like Unity
- Using threads and keeping things thread safe
- Creating software that can work cross-platform and actually testing them to ensure that they work with the same code base.
- Properly including the licenses to different third party libraries that I utilize.

**THE THIRD PARTY LICENSES ARE LISTED IN THE LICENSE FILE IN THE ROOT DIRECTORY**

## Using the Software

### Command Line Arguments
- \-n, --new {easy, medium, hard OR random} Select one choice to fetch a new sudoku puzzle from sugoku's public sudoku generation API. If you want to use a local board supplied by "board.json" in the root directory, then do not use this parameter.  
- \-s,--speed {integer} controls the speed of the algorithm solving the puzzle. This can be omitted or set to 0 if you want to run the solver at the max speed that the code will execute.

### In-Game Controls
- **Space** will start the solving after the sudoku board has been loaded.
- **Esc** will close the game. 