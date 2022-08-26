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

        (List<KeyValuePair<int, int>> operations, int numOfBox) = Initialize();
        //ボール Xj が入っている箱を探し，その箱からボール Xj を取り出す．その後，箱 Yj にボール Xj を入れる．
        var boxSituation = MoveBall(operations, numOfBox);
        //葵が M 回の操作をすべて終えた後，N 個のボールがそれぞれどの箱に入っているかを求めよ．
        ShowBoxSituation(boxSituation);

        Console.ReadKey();
    }

    static IEnumerable<int> MoveBall(List<KeyValuePair<int, int>> operations, int numOfBox)
    {
        var boxes = CreateBoxes(numOfBox);
        foreach(var operation in operations)
        {
            MoveBallFromSourceToTarget(operation, boxes);
        }

        return CreateBoxNumberList(boxes);
    }

    static IEnumerable<int> CreateBoxNumberList(IEnumerable<Box> boxes)
    {
        var boxNumberList = new List<KeyValuePair<int, int>>();
        foreach(var box in boxes)
        {
            var boxNumber = box.Number;
            foreach(var ball in box.Balls)
            {
                boxNumberList.Add(new KeyValuePair<int, int>(ball.Number, boxNumber));
            }
        }

        return boxNumberList.OrderBy(x=> x.Key).Select(y => y.Value);
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
        int foundIndex;

        foreach(var box in boxes)
        {
            foreach(var ball in box.Balls)
            {
                if (ball.Number == sourceBallIndex)
                {
                    foundIndex = box.Number;
                    box.Balls.Remove(ball);
                    return ball;
                }
            }
        }

        return null;
    }

    static void MoveBallToTarget(IEnumerable<Box> boxes, int targetBoxIndex,  Ball ball)
    {
        var box = boxes.First(x => x.Number == targetBoxIndex);
        box.Balls.Add(ball);
    }

    static List<Box> CreateBoxes(int count)
    {
        var boxes = new List<Box>();
        for(int index = 1; index <= count; index++)
        {
            var box = new Box(new Ball(index), index);
            boxes.Add(box);
        }

        return boxes;
    }

    static void ShowBoxSituation(IEnumerable<int> boxSituation)
    {
        foreach(var inBox in boxSituation)
        {
            Console.WriteLine(inBox);
        }
    }
    static (List<KeyValuePair<int, int>> operations, int numOfBox) Initialize()
    {
        var inputData = ReadInputData;
        (List<KeyValuePair<int, int>> operations, int numOfBox)  = CreateOperation(inputData);

        return (operations, numOfBox);
    }

    static (List<KeyValuePair<int, int>> operations, int numOfBox) CreateOperation(IEnumerable<string> inputData)
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

        return (operations, N);

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
        }

        internal int Number {get;}
    }

}
