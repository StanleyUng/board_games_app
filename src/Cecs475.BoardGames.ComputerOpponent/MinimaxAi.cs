using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cecs475.BoardGames.ComputerOpponent {

	/// <summary>
	/// A pair of an IGameMove that was the best move to apply for a given board state,
	/// and the Weight of the board that resulted.
	/// </summary>
	internal struct MinimaxBestMove {
		public int Weight { get; set; }
		public IGameMove Move { get; set; }
	}

	/// <summary>
	/// A minimax with alpha-beta pruning implementation of IGameAi.
	/// </summary>
	public class MinimaxAi : IGameAi {
		private int mMaxDepth;
		public MinimaxAi(int maxDepth) {
			mMaxDepth = maxDepth;
		}

		// The public calls this function, which kicks off the minimax search.
		public IGameMove FindBestMove(IGameBoard b) {
			// TODO: call the private FindBestMove with appropriate values for the parameters.
			// mMaxDepth is what the depthLeft should start at.
			// You are maximizing if the board's current player is 1.

			//return FindBestMove(b, mMaxDepth, b.CurrentPlayer == 1).Move;
         return FindBestMove(b, mMaxDepth, b.CurrentPlayer == 1, Int32.MinValue, Int32.MaxValue).Move;
		}

		//private static MinimaxBestMove FindBestMove(IGameBoard b, int depthLeft, bool maximize) {
      private static MinimaxBestMove FindBestMove(IGameBoard b, int depthLeft, bool maximize, int alpha, int beta) {
			// Implement the minimax algorithm. 
			// Your first attempt will not use alpha-beta pruning. Once that works, 
			// implement the pruning as discussed in the project notes.

         // maximize = player 1
         // !maximize = player 2

         if (depthLeft == 0 || b.IsFinished) {
            return new MinimaxBestMove() {
               Weight = b.Weight,
               Move = null
            };
         }

         //int bestWeight = maximize ? int.MinValue : int.MaxValue;
         IGameMove bestMove = null;

         foreach (var m in b.GetPossibleMoves()) {
            b.ApplyMove(m);
            MinimaxBestMove w = FindBestMove(b, depthLeft - 1, !maximize, alpha, beta);
            b.UndoLastMove();

            if (maximize && w.Weight > alpha) {
               alpha = w.Weight;
               bestMove = m;
            } else if (!maximize && w.Weight < beta) {
               beta = w.Weight;
               bestMove = m;
            } 
            
            if (alpha >= beta) {
               break;
            }
         }

         return new MinimaxBestMove() {
            Weight = maximize ? alpha : beta,
            Move = bestMove
         };
		}
	}
}
