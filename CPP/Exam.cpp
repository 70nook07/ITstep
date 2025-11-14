#include <iostream>
#include <cstdlib>
#include <ctime>
#include <cstring>

using namespace std;

// Game config
const int BOARD_SIZE = 10;
const int EMPTY = 0;
const int SHIP = 1;
const int HIT = 2;
const int MISS = 3;
const int SUNK = 4;

// Ship types and sizes
const int CARRIER = 5;
const int BATTLESHIP = 4;
const int CRUISER = 3;
const int SUBMARINE = 3;
const int DESTROYER = 2;

// Game data
struct Game {
    int pl_board[BOARD_SIZE][BOARD_SIZE];
    int pc_board[BOARD_SIZE][BOARD_SIZE];
    int pl_hits[BOARD_SIZE][BOARD_SIZE];
    int pc_hits[BOARD_SIZE][BOARD_SIZE];
    int pl_ships_sunk;
    int pc_ships_sunk;
    bool pl_turn;
    bool game_running;
};

// Funcs init
void init_game(Game* game);
void show_board(int board[BOARD_SIZE][BOARD_SIZE], bool show_ships);
void show_game_state(Game* game);
bool place_ship(int board[BOARD_SIZE][BOARD_SIZE], int size);
bool is_valid_place(int board[BOARD_SIZE][BOARD_SIZE], int row, int col, int size, bool is_horizontal);
void pc_deploy_ships(Game* game);
void pl_deploy_ships(Game* game);
bool pc_move(Game* game);
bool pl_move(Game* game, int row, int col);
bool check_n_mark(int board[BOARD_SIZE][BOARD_SIZE], int hit_board[BOARD_SIZE][BOARD_SIZE], int size);
void intel_move_search(Game* game, int& target_row, int& target_col);
bool is_game_over(Game* game);
void play(Game* game);	

// Funcs defined
void init_game(Game* game) {
	
	// Clear board
	for (int i = 0; i < BOARD_SIZE; i++) {
        for (int j = 0; j < BOARD_SIZE; j++) {
            game->pl_board[i][j] = EMPTY;
            game->pc_board[i][j] = EMPTY;
            game->pl_hits[i][j] = EMPTY;
            game->pc_hits[i][j] = EMPTY;
        }
    }
    
    game->game_running = true;
    game->pl_turn = true;
    game->pl_ships_sunk = 0;
    game->pc_ships_sunk = 0;
}

void show_board(int board[BOARD_SIZE][BOARD_SIZE], bool show_ships) {

    // Column headers
    cout << "  ";
    for (int j = 0; j < BOARD_SIZE; j++)
        cout << j << " ";
    cout << "\n";

    for (int i = 0; i < BOARD_SIZE; i++) {
        cout << i << " ";

        for (int j = 0; j < BOARD_SIZE; j++) {
            int cell = board[i][j];

            // If it's a ship (IDs 1–5)
            if (cell == SHIP) {
				cout << (show_ships ? "S " : "~ ");
				continue;
            }

            // For non-ship cells
            if (cell == EMPTY)      cout << "~ ";
            else if (cell == HIT)   cout << "X ";
            else if (cell == MISS)  cout << "w ";
            else if (cell == SUNK)  cout << "# ";
            else                    cout << "? "; // Should never happen
        }

        cout << "\n";
    }
}



void show_game_state(Game* game) {
	
	cout << "\n ~Your Board~" << endl;
	show_board(game->pl_board, true);
	
	//Counterintuitive namings. prob ought to change.
	cout << "\nPlayer ships sunk: " << game->pc_ships_sunk << endl; 
	
	cout << "\n ~Your Hits on the Enemy~" << endl;
	show_board(game->pc_hits, true);
	
	cout << "\nComputer ships sunk: " << game->pl_ships_sunk << endl;
}

bool place_ship(int board[BOARD_SIZE][BOARD_SIZE], int size) {
	
	// Random horizontal placement
	bool is_horizontal = rand() % 2 == 0;

	for (int attempt = 0; attempt < 2; attempt++) {
		
		int valid_pos_count = 0;
		// Valid coords
		int valid_rows[BOARD_SIZE * BOARD_SIZE];
		int valid_cols[BOARD_SIZE * BOARD_SIZE];
		
		// Look for valid pos
		for (int row = 0; row < BOARD_SIZE; row++) {
			for (int col = 0; col < BOARD_SIZE; col++) {
				if (is_valid_place(board, row, col, size, is_horizontal)) {
					
					valid_rows[valid_pos_count] = row;
					valid_cols[valid_pos_count] = col;
					valid_pos_count++;
				}
			}
		}
	
	if (valid_pos_count > 0) {
		// Pick random valid pos
		int random_pos = rand() % valid_pos_count;
		int row = valid_rows[random_pos];
		int col = valid_cols[random_pos];
		
		if (is_horizontal) {
			for (int i = 0; i < size; i++) {
				board[row][col + i] = SHIP;
			}
		} else {
				for (int i = 0; i < size; i++) {
				board[row + i][col] = SHIP;
			}
		}
		
		return true;
	}
	
	// Flip orientation for the next attempt
	is_horizontal = !is_horizontal;
	}

	return false;
}

bool is_valid_place(int board[BOARD_SIZE][BOARD_SIZE], int size, int row, int col, bool is_horizontal) {
	
	// Does ship fit in boundaries?
	if (is_horizontal) {
        if (col + size > BOARD_SIZE) return false;
    } else {
        if (row + size > BOARD_SIZE) return false;
    }
    
    for (int i = 0; i < size; i++) {
		
		int current_row = row + (is_horizontal ? 0 : i);
		int current_col = col + (is_horizontal ? i : 0);
		
		for (int sr = -1; sr <= 1; sr++) {
			for (int sc = -1; sc <= 1; sc++) {
				int check_row = current_row + sr;
				int check_col = current_col + sc;
				
				// Skip out-of-bounds cellls
				if (check_row < 0 || check_col < 0 || check_row >= BOARD_SIZE || check_col >= BOARD_SIZE)
					continue;
				
				// Check for every nearby cell
				if (board[check_row][check_col] != EMPTY)
					return false;
				
			}
		}
	}
	
	return true;
}

void pc_deploy_ships(Game* game) {
	
	int ships[5] = {CARRIER, BATTLESHIP, CRUISER, SUBMARINE, DESTROYER};
	
	for (int i = 0; i < 5; i++) {
		bool placed = false;
		while (!placed) {
			placed = place_ship(game->pc_board, ships[i]);
		}
	}
	cout << "Computer has placed its ships." << endl;
}

void pl_deploy_ships(Game* game) {
	
	cout << "\nPlace Ships!"
		 << "\nShips to place: Carrier(5), Battleship(4), Cruiser(3), Submarine(3), Destroyer(2)" << endl;
		 
	int ships[5] = {CARRIER, BATTLESHIP, CRUISER, SUBMARINE, DESTROYER};
	char ship_names[5][20] = {"Carrier", "Battleship", "Cruiser", "Submarine", "Destroyer"};

	for (int i = 0; i < 5; i++) {
		bool placed = false;
		while (!placed) {
			show_board(game->pl_board, true);
			cout << "\nPlacing" << ship_names[i] << " (size " << ships[i] << ")" << endl;
			
			int row, col;
			char direction;
			
			cout << "Enter row (0-" << BOARD_SIZE-1 << "): ";
            cin >> row;
            cout << "Enter column (0-" << BOARD_SIZE-1 << "): ";
            cin >> col;
        	cout << "Horizontal (h) or Vertical (v): ";
			cin >> direction;
            
			bool is_horizontal = (direction == 'h' || direction == 'H');
			
			if (is_valid_place(game->pl_board, ships[i], row, col, is_horizontal)) {
				if (is_horizontal) {
					for (int j = 0; j < ships[i]; j++) {
                        game->pl_board[row][col + j] = SHIP;
                    }
				} else {
					for (int j = 0; j < ships[i]; j++) {
                        game->pl_board[row + j][col] = SHIP;
                    }
				}
				
				placed = true;
			} else {
				
				cout << "\nInvalid Placement! Try different coordinates!" << endl;
			}
		}
	}
}

bool pc_move(Game* game) {
	
	int row, col;
	
	// Picking target
	intel_move_search(game, row, col);
	
	cout << "Computer shoots at " << row << ", " << col << endl;
	
	int cell = game->pl_board[row][col];
	
	
	if (cell == SHIP) {
        cout << "Computer HIT your ship!\n";

        game->pl_board[row][col] = HIT;
        game->pl_hits[row][col]  = HIT;

        // Check if ship is sunk
        for (int size = 2; size <= 5; size++) {
            if (check_n_mark(game->pl_board, game->pl_hits, size)) {
                game->pl_ships_sunk++;
                cout << "Computer sunk your ship of size " << size << "!\n";
            }
        }

        return true;
    }
	
	 cout << "Computer MISSED!\n";
    game->pl_hits[row][col] = MISS;

    if (cell == EMPTY) {
        game->pl_board[row][col] = MISS;
    }

    return true;
	
}

bool pl_move(Game* game, int row, int col) {
	
	if (row < 0 || row >= BOARD_SIZE || col < 0 || col >= BOARD_SIZE) {
        cout << "Invalid coordinates!\n";
        return false;
    }
    
    if (game->pc_hits[row][col] != EMPTY) {
        cout << "This place has already been shot!\n";
        return false;
    }
    
    if (game->pc_board[row][col] == SHIP) {
        cout << "HIT!\n";
        game->pc_hits[row][col] = HIT;
        game->pc_board[row][col] = HIT;
        
        // Check if ship is sunk
        for (int size = 2; size <= 5; size++) {
            if (check_n_mark(game->pc_board, game->pc_hits, size)) {
                game->pc_ships_sunk++;
                cout << "You sunk a ship of size " << size << "!\n";
            }
        }
    } else {
        cout << "MISS!\n";
        game->pc_hits[row][col] = MISS;
    }
    
    return true;
}


// Whacky and doesn't work as intended, kind of breaks intelligent targeting.
bool check_n_mark(int board[BOARD_SIZE][BOARD_SIZE], int hit_board[BOARD_SIZE][BOARD_SIZE], int size) {
	for (int orientation = 0; orientation < 2; orientation++) {
        
        // Increments for scanning
        int sr = orientation == 0 ? 0 : 1;
        int sc = orientation == 0 ? 1 : 0;
        
        // limit scan area to check if ship fits inside board
        int max_r = BOARD_SIZE - (sr ? size : 0);
        int max_c = BOARD_SIZE - (sc ? size : 0);
        
        for (int r = 0; r < max_r; r++) {
             for (int c = 0; c < max_c; c++) {

                bool all_hit = true;
				
				// Check all cells in possible ship seg
                for (int k = 0; k < size; k++) {
                    int cr = r + sr * k;
                    int cc = c + sc * k;

                    int cell = board[cr][cc];

                    if (cell != HIT && cell != SUNK) all_hit = false;
                }

				// Mark sunk if hits on ship
                if (all_hit) {
                    for (int k = 0; k < size; k++) {
                        int cr = r + sr * k;
                        int cc = c + sc * k;
                        board[cr][cc] = SUNK;
                        hit_board[cr][cc] = SUNK;
                    }
                    
                    return true;
                }
            }
        }
    }

    return false;
}

void intel_move_search(Game* game, int& target_row, int& target_col) {
	
	// Check adjacent cells
	for (int row = 0; row < BOARD_SIZE; row++) {
        for (int col = 0; col < BOARD_SIZE; col++) {
            if (game->pl_hits[row][col] != HIT) continue;

			// Adjacent cells relative coords
            int sr[4] = {-1, 1, 0, 0};
            int sc[4] = {0, 0, -1, 1};

            for (int s = 0; s < 4; s++) {
                int r = row + sr[s];
                int c = col + sc[s];
                if (r >= 0 && r < BOARD_SIZE && c >= 0 && c < BOARD_SIZE && game->pl_hits[r][c] == EMPTY) {
                    target_row = r;
                    target_col = c;
                    return;
                }
            }
        }
    }
    
    // Scoring system idea I found on stack exchange
    int best_score = -1;

    for (int row = 0; row < BOARD_SIZE; row++) {
        for (int col = 0; col < BOARD_SIZE; col++) {
            if (game->pl_hits[row][col] != EMPTY) continue;

            int score = 0;

            // Horizontal free cells
            if (col > 0 && game->pl_hits[row][col - 1] == EMPTY) score++;
            if (col < BOARD_SIZE - 1 && game->pl_hits[row][col + 1] == EMPTY) score++;

            // Vertical free cells
            if (row > 0 && game->pl_hits[row - 1][col] == EMPTY) score++;
            if (row < BOARD_SIZE - 1 && game->pl_hits[row + 1][col] == EMPTY) score++;

            if (score > best_score) {
                best_score = score;
                target_row = row;
                target_col = col;
            }
        }
    }
    
    
    // If didn't find optimal move use random
	if (game->pl_hits[target_row][target_col] != EMPTY) {
		do {
		    target_row = rand() % BOARD_SIZE;
		    target_col = rand() % BOARD_SIZE;
		} while (game->pl_hits[target_row][target_col] != EMPTY);
    }
}

bool is_game_over(Game* game) {
	
	 return game->pl_ships_sunk >= 5 || game->pc_ships_sunk >= 5;
}

void play(Game* game) {
	cout << "================================================================================\n";
	cout << "|                                                                              |\n";
	cout << "|                  ╔══════════════════════════════════════╗                    |\n";
	cout << "|                  ║          NAVAL COMBAT SIMULATOR      ║                    |\n";
	cout << "|                  ║           BATTLESHIP COMMAND         ║                    |\n";
	cout << "|                  ╚══════════════════════════════════════╝                    |\n";
	cout << "|                                                                              |\n";
	cout << "|  COMMANDER! The fate of our naval supremacy rests in your capable hands.     |\n";
	cout << "|  You stand at the precipice of maritime glory, facing an adversary of        |\n";
	cout << "|  unparalleled tactical acumen - the Cybernetic Naval Tactical Unit.          |\n";
	cout << "|                                                                              |\n";
	cout << "|  Your mission, should you choose to accept it:                               |\n";
	cout << "|  • Deploy your fleet with strategic precision across the treacherous waters  |\n";
	cout << "|  • Engage in a battle of wits against our most advanced AI opponent          |\n";
	cout << "|  • Demonstrate tactical superiority through calculated strikes               |\n";
	cout << "|  • Emerge victorious where lesser commanders have faltered                   |\n";
	cout << "|                                                                              |\n";
	cout << "|  The computer shall deploy its armada with cunning artifice, employing       |\n";
	cout << "|  advanced probabilistic targeting algorithms and pattern recognition.        |\n";
	cout << "|  This is no mere random number generator - you face a true digital admiral!  |\n";
	cout << "|                                                                              |\n";
	cout << "|  Prepare to chart your course, Commander. The high seas await your command!  |\n";
	cout << "|                                                                              |\n";
	cout << "================================================================================\n";
	cout << "\n";
	cout << "ADVANCED TACTICAL BRIEFING:\n";
	cout << "Your arsenal consists of five formidable vessels:\n";
	cout << "  • AIRCRAFT CARRIER (5 units)   - The pride of your fleet\n";
	cout << "  • BATTLESHIP (4 units)         - Heavy firepower and resilience\n";
	cout << "  • CRUISER (3 units)            - Versatile and deadly\n";
	cout << "  • SUBMARINE (3 units)          - Silent but lethal\n";
	cout << "  • DESTROYER (2 units)          - Swift and agile\n";
	cout << "\n";
	cout << "INITIATE FLEET DEPLOYMENT SEQUENCE...\n";
	cout << "Commander, position your vessels with strategic foresight!\n";
	cout << "\n";
	
	pc_deploy_ships(game);
    pl_deploy_ships(game);
    
    while (game->game_running) {
        show_game_state(game);
        
        if (game->pl_turn) {
			// Player move
            cout << "\n~~~~~ YOUR TURN ~~~~~\n";
            
            int row, col;
            cout << "Enter coordinates to shoot (row, column): ";
            cin >> row >> col;
            
            if (pl_move(game, row, col)) {
                game->pl_turn = false;
            }
        } else {
			// PC move
            cout << "\n~~~~~ COMPUTER'S TURN ~~~~~\n";
            pc_move(game);
            game->pl_turn = true;
        }
		if (is_game_over(game)) {
			game->game_running = false;
			show_game_state(game);
			
			if (game->pc_ships_sunk >= 5) {
			    cout << "\n";
			    cout << "╔══════════════════════════════════════════════════════════════════════════════╗\n";
			    cout << "║                                                                              ║\n";
			    cout << "║                        ██████  VICTORIA MAXIMA!  ██████                      ║\n";
			    cout << "║                                                                              ║\n";
			    cout << "║    Your strategic brilliance has vanquished the cybernetic adversary!        ║\n";
			    cout << "║    The digital admiral lies defeated beneath the waves, its algorithms       ║\n";
			    cout << "║    silenced by your superior tactical acumen. The naval theater bears        ║\n";
			    cout << "║    witness to your unparalleled command presence and decisive judgment.      ║\n";
			    cout << "║                                                                              ║\n";
			    cout << "║    History shall record this day as a testament to human intuition           ║\n";
			    cout << "║    triumphing over cold machine logic. Your name shall be inscribed          ║\n";
			    cout << "║    in the annals of naval warfare as a master tactician!                     ║\n";
			    cout << "║                                                                              ║\n";
			    cout << "║    The seas are secure, Commander. Your victory resonates across             ║\n";
			    cout << "║    the maritime domain like thunder across calm waters!                      ║\n";
			    cout << "║                                                                              ║\n";
			    cout << "╚══════════════════════════════════════════════════════════════════════════════╝\n";
			    cout << "\n";
			} else {
			    cout << "\n";
				cout << "╔══════════════════════════════════════════════════════════════════════════════╗\n";
				cout << "║                                                                              ║\n";
				cout << "║                     ██████  NAVAL CATASTROPHE!  ██████                       ║\n";
				cout << "║                                                                              ║\n";
				cout << "║    The cold, unfeeling machinery has prevailed. Your fleet lies              ║\n";
				cout << "║    scattered across the ocean floor, a solemn testament to the               ║\n";
				cout << "║    relentless precision of artificial intelligence.                          ║\n";
				cout << "║                                                                              ║\n";
				cout << "║    Though defeat stings like salt in a fresh wound, take solace              ║\n";
				cout << "║    in knowing you faced our most advanced tactical processor.                ║\n";
				cout << "║    The machine calculates probabilities with inhuman efficiency,             ║\n";
				cout << "║    but remember: it lacks your courage, intuition, and the                   ║\n";
				cout << "║    indomitable human spirit that learns from adversity.                      ║\n";
				cout << "║                                                                              ║\n";
				cout << "║    Regroup, reassess, and return to command! The naval theater               ║\n";
				cout << "║    awaits your redemption and future triumphs!                               ║\n";
				cout << "║                                                                              ║\n";
				cout << "╚══════════════════════════════════════════════════════════════════════════════╝\n";
				cout << "\n";
			}
			cout << "Play again? (y/n): ";
			char answ;
			cin >> answ;
			
			if (answ == 'y' || answ == 'Y') {
			    init_game(game);
			    continue;   // restart loop with new game
			}
			
			break;          // exit main loop
        }
    }
}


int main() {
	srand(time(0)); // Seed random number generator
    
    Game game;
    init_game(&game);
    play(&game);
    
	return 0;
}







