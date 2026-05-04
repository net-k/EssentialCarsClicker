using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public static class EnumerableUtilities
{
    public static IEnumerable<int> RangePython(int start, int stop, int step = 1)
    {
        if (step == 0)
            throw new ArgumentException("Parameter step cannot equal zero.");

        if (start < stop && step > 0)
        {
            for (var i = start; i < stop; i += step)
            {
                yield return i;
            }
        }
        else if (start > stop && step < 0)
        {
            for (var i = start; i > stop; i += step)
            {
                yield return i;
            }
        }
    }

    public static IEnumerable<int> RangePython(int stop)
    {
        return RangePython(0, stop);
    }
}

static class ListExtensions
{
// https://stackoverflow.com/questions/9325627/splice-on-collections
    public static List<T> Splice<T>(this List<T> Source, int Start, int Size)
    {
        List<T> retVal = Source.Skip(Start).Take(Size).ToList<T>();
        Source.RemoveRange(Start, Size);
        return retVal;
    }
}   

public class ShisenShoUseCase : MonoBehaviour
{
    public class ShisenShoState
    {
        public int[] board;
        public int target;
        public int rest;
        public bool solved = false;
        public bool tested = false;
    }

    private ShisenShoState _shisenShoState;

    public ShisenShoState State => _shisenShoState;
    
    int Pick(ref List<int> tiles)
    {
        return tiles.Splice((int) (tiles.Count * Random.Range(0.0f, 1.0f)), 1)[0];
    }

    int[] Range(int start, int stop, int step)
    {
        return EnumerableUtilities.RangePython(start, stop, step).ToArray();
    }

    int[] Range(int start)
    {
        return EnumerableUtilities.RangePython(start).ToArray();
    }

    const  int N = 17 * 8; // 牌の数   136 / 2 = 68
    public int W = 17 + 4; // 盤の横幅
    public int H =  8 + 4; // 盤の縦幅

    int X(int p)
    {
        return p % W;
    }

    int Y(int p)
    {
        return (int) Mathf.Floor((int) (p / W));
    }

    int FromXY(int x, int y)
    {
        return x + y * W;
    }

    int FromYX(int y, int x)
    {
        return FromXY(x, y);
    }

    int Move(int[] board, int p, int d)
    {
        return board[p + d] != 0 ? p : Move(board, p + d, d);
    }


    bool Pass(int[] board, int p, int q, Func<int, int> U, Func<int, int> V, Func<int, int, int> C)
    {
        int e = C(1, 0);
        int u0 = Mathf.Max(U(Move(board, p, -e)), U(Move(board, q, -e)));
        int u1 = Mathf.Min(U(Move(board, p, +e)), U(Move(board, q, +e)));
        int v0 = Mathf.Min(V(p), V(q)) + 1;
        int v1 = Mathf.Max(V(p), V(q)) - 1;
        int[] us = Range(u0, u1 + 1, 1);
        int[] vs = Range(v0, v1 + 1, 1);
        return us.Any(u => vs.All(v => board[C(u, v)] == 0));
    }

    bool Test(int[] board, int p, int q)
    {
        Debug.Log($"p={p.ToString()}, q={q.ToString()}");
        if (p != q && board[p] == board[q])
        {
            bool pass1 = Pass(board, p, q, X, Y, FromXY);
            bool pass2 = Pass(board, p, q, Y, X, FromYX);

            Debug.Log($"pass1={pass1.ToString()}, pass2={pass2.ToString()}, v={board[p].ToString()}, v={board[q].ToString()}");
            return pass1 || pass2;
        }

        return false;
    }

    public ShisenShoState Create()
    {
        List<int> tiles = Range(N).Select(i => (int)(1 + Mathf.Floor(i / 4) ) ).ToList();
        
        int[] board = Range(W * H).Select(p =>
        {
            int d = Mathf.Min(X(p), Y(p), W - 1 - X(p), H - 1 - Y(p));
            // return d == 0 ? -1 : d == 1 ? 0 : Pick(tiles);
            if (d == 0)
            {
                return -1;
            }
            else if (d == 1)
            {
                return 0;
            }
            else
            {
                return Pick(ref tiles);
            }
        }).ToArray();
        _shisenShoState = new ShisenShoState();
        _shisenShoState.board = board;
        _shisenShoState.target = -1;
        _shisenShoState.rest = N;
        _shisenShoState.solved = false;
        // return  new { board, target: -1, rest: N };
        return _shisenShoState;
    }

    public ShisenShoState UpdateState(int boardIndex)
    {
        _shisenShoState.solved = false;
        _shisenShoState.tested = false;

        if (_shisenShoState.board[boardIndex] <= 0)
        {
            Debug.Log($"update board[p] <=0 | p={boardIndex.ToString()},v={_shisenShoState.board[boardIndex].ToString()}");
            return _shisenShoState;
        }

        if (_shisenShoState.target < 0)
        {
            Debug.Log($"update target< 0 | p={boardIndex.ToString()},v={_shisenShoState.board[boardIndex].ToString()}");
            _shisenShoState.target = boardIndex;
            return _shisenShoState;
        }

        _shisenShoState.tested = true;
        if (!Test(_shisenShoState.board, _shisenShoState.target, boardIndex))
        {
            Debug.Log($"update test is false");
            _shisenShoState.target = -1;
            return _shisenShoState;
        }
        

        _shisenShoState.board = _shisenShoState.board.Select((v, i) => i == boardIndex || i == _shisenShoState.target ? 0 : v).ToArray();
        _shisenShoState.target = -1;
        _shisenShoState.rest = _shisenShoState.rest - 2;
        _shisenShoState.solved = true;
        return _shisenShoState;
    }

    public bool Solve(ShisenShoState shisenShoState)
    {
        while (shisenShoState.rest != 0)
        {
            int[] pair = findPair(shisenShoState.board);
            if (pair.Length == 0)
            {
                return false;
            }

            shisenShoState = UpdateState(pair[0]);
            shisenShoState = UpdateState(pair[1]);
        }

        return true;
    }

    int[] findPair(int[] board)
    {
        return new List<int>().ToArray();
    }
}