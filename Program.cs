// See https://aka.ms/new-console-template for more information
// https://onlinejudge.u-aizu.ac.jp/challenges/search/titles/0718

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

internal class Program{

    static void Main(string[] args)
    {
        //N 個のボールがあり，1 から N までの番号が付けられている．また，何個でもボールを入れることのできる N 個の箱があり，箱には 1 から N までの番号が付けられている．
        //箱 i (1≦i≦N) には最初，ボール i が入っていた．
        //JOI 高校の生徒である葵は，この状態から箱とボールに対して M 回の操作を行った．j 回目 (1≦j≦M) の操作は，次のように行われた．

        (IReadOnlyList<KeyValuePair<int, int>> operations, int numOfBox) = Initialize();
        //ボール Xj が入っている箱を探し，その箱からボール Xj を取り出す．その後，箱 Yj にボール Xj を入れる．
        var numberOfBoxesForEachBall = MoveBall(operations, numOfBox);
        //葵が M 回の操作をすべて終えた後，N 個のボールがそれぞれどの箱に入っているかを求めよ．
        ShowNumberOfBoxesForEachBall(numberOfBoxesForEachBall);

        //Console.ReadKey();
    }

    static IEnumerable<int> MoveBall(IReadOnlyList<KeyValuePair<int, int>> operations, int numOfBoxex)
    {
        (IEnumerable<Box> boxes, IEnumerable<Ball> balls) = CreateBoxesAndBalls(numOfBoxex);
        foreach(var operation in operations)
        {
            MoveBallFromSourceToTarget(operation, boxes);
        }

        return CreateBoxNumberList(balls);
    }

    static IEnumerable<int> CreateBoxNumberList(IEnumerable<Ball> balls)
    {
        var boxNumberList = new List<int>();
        foreach(var ball in balls) //ballsの順番は1から降順
        {
            boxNumberList.Add(ball.BoxNumber);
        }

        return boxNumberList;
    }
    static void MoveBallFromSourceToTarget(KeyValuePair<int, int> operation, IEnumerable<Box> boxes)
    {
        var sourceBallIndex = operation.Key;
        var targetBoxIndex = operation.Value;
        var ballToMove = MoveBallFromSource(boxes, sourceBallIndex);
        MoveBallToTarget(boxes, targetBoxIndex, ballToMove);
    }

    static Ball MoveBallFromSource(IEnumerable<Box> boxes, int sourceBallIndex)
    {
        foreach(var box in boxes)
        {
            foreach(var ball in box.Balls)
            {
                if (ball.Number == sourceBallIndex)
                {
                    box.Balls.Remove(ball);
                    ball.BoxNumber = 0;
                    return ball;
                }
            }
        }

        return null;
    }

    static void MoveBallToTarget(IEnumerable<Box> boxes, int targetBoxIndex, Ball ball)
    {
        var box = boxes.First(x => x.Number == targetBoxIndex);
        box.Balls.Add(ball);
        ball.BoxNumber = box.Number;
    }

    static (IEnumerable<Box> boxes, IEnumerable<Ball> balls) CreateBoxesAndBalls(int count)
    {
        var boxes = new List<Box>();
        var balls = new List<Ball>();
        for(int index = 1; index <= count; index++)
        {
            var ball = new Ball(index);
            var box = new Box(ball, index);
            boxes.Add(box);
            balls.Add(ball);
        }

        return (boxes, balls);
    }

    static void ShowNumberOfBoxesForEachBall(IEnumerable<int> boxSituation)
    {
        foreach(var inBox in boxSituation)
        {
            Console.WriteLine(inBox);
        }
    }
    static (IReadOnlyList<KeyValuePair<int, int>> operations, int numOfBox) Initialize()
    {
        var inputData = ReadInputData;
        (IReadOnlyList<KeyValuePair<int, int>> operations, int numOfBox)  = CreateOperation(inputData);

        return (operations, numOfBox);
    }

    static (IReadOnlyList<KeyValuePair<int, int>> operations, int numOfBox) CreateOperation(IEnumerable<string> inputData)
    {
        var numOfBallAndnumOfOperation = inputData.ElementAt(0).Split(" ");
        int N = int.Parse(numOfBallAndnumOfOperation[0]);
        int M = int.Parse(numOfBallAndnumOfOperation[1]);

        var sourceboxAndtargetboxData = inputData.ToList().GetRange(1, inputData.Count()-1);

        var operations = new List<KeyValuePair<int, int>>();
        for(int index=0; index < M; index++)
        {
            var sourceboxAndtargetbox = sourceboxAndtargetboxData[index].Split(" ");
            var source = int.Parse(sourceboxAndtargetbox[0]);
            var target = int.Parse(sourceboxAndtargetbox[1]);
            var pair = new KeyValuePair<int, int>(source, target);
            operations.Add(pair);
        }

        return (operations.AsReadOnly(), N);
    }

    static IEnumerable<string> ReadInputData
    {
        get
        {
            List<string> input = new List<string>();
            string line = null;
            do
            {
                line = Console.ReadLine();
                if (line == null)
                {
                    break;
                }
                else
                {
                    input.Add(line);
                }
            }while(string.IsNullOrEmpty(line) == false);

            return input;
        }
    }

    internal class Box
    {
        internal Box(Ball ball, int number)
        {
            Number = number;
            Balls.Add(ball);
        }

        internal int Number {get;}
        internal List<Ball> Balls {get;} = new List<Ball>();
    }

    internal class Ball
    {
        internal Ball(int number)
        {
            Number = number;
            BoxNumber = number;
        }

        internal int Number {get;}

        internal int BoxNumber {get; set;}
    }

}
