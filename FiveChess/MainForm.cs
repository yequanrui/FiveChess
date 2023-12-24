using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Collections;

namespace FiveChess
{
    public partial class MainForm : Form
    {
        // 动画窗体调用，关闭时将向上移除屏幕
        [System.Runtime.InteropServices.DllImport("user32")]
        private static extern bool AnimateWindow(IntPtr hwnd, int dwTime, int dwFlags);
        const int AW_HOR_POSITIVE = 0x0001;
        const int AW_HOR_NEGATIVE = 0x0002;
        const int AW_VER_POSITIVE = 0X0004;
        const int AW_VER_NEGATIVE = 0x0008;
        const int AW_CENTER = 0x0010;
        const int AW_HIDE = 0x10000;
        const int AW_ACTIVATE = 0x20000;
        const int AW_SLIDE = 0x40000;
        const int AW_BLEND = 0x80000;

        private int[,] virtualGobangBoard=new int[15,15];//虚拟棋盘
        private PictureBox[,] gobangPictureBox=new PictureBox[15,15];//棋子
        private Point lastMovePoint = new Point(-1, -1);
        private bool blnBegin;
        private const int black = -1, white = 1, background = 0;
        private int personGobangColor, computerGobangColor;
        private int totalGobangCount = 0;
        private Stack backStack = new Stack();//用于悔棋的栈
        private Stack backTrackStack = new Stack();//用于回溯的栈 
        const int M = 1;//预测的步数

        public MainForm()
        {
            InitializeComponent();
            gobangBoardGroupBox.Paint+=new PaintEventHandler(gobangBoardGroupBox_Paint);
            InitializeGobangBoard();
            gobangBoardGroupBox.MouseMove+=new MouseEventHandler(gobangBoardGroupBox_MouseMove);
            this.MouseMove+=new MouseEventHandler(MainForm_MouseMove);
            gobangBoardGroupBox.MouseClick+=new MouseEventHandler(gobangBoardGroupBox_MouseClick);
        }
        
        private void InitializeGobangBoard()//初始化棋盘
        {
            gobangBoardGroupBox.Paint += new PaintEventHandler(gobangBoardGroupBox_Paint);
            int x, y;
            for (x = 0; x < 15; x++)
                for (y = 0; y < 15; y++)
                {
                    gobangPictureBox[x, y] = new PictureBox();
                    gobangPictureBox[x, y].Location = new Point(10 + x * 40, 10 + y * 40);
                    gobangPictureBox[x, y].Size = new Size(40,40);
                    gobangPictureBox[x, y].BackColor = Color.Transparent;
                    gobangPictureBox[x, y].SizeMode = PictureBoxSizeMode.CenterImage;
                    gobangPictureBox[x, y].Visible = false;
                    gobangBoardGroupBox.Controls.Add(gobangPictureBox[x, y]);
                }
        }

        private void gobangBoardGroupBox_Paint(object sender, PaintEventArgs e)//画棋盘格
        {
            int i;
            Graphics gr = e.Graphics;
            Pen myPen = new Pen(Color.Black, 2);
            SolidBrush brush = new SolidBrush(Color.Red);
            for (i = 0; i < 15; i++)
            {
                gr.DrawLine(myPen, 30 + i * 40, 30, 30 + i * 40, 590);
                gr.DrawLine(myPen, 30, 30 + i * 40, 590, 30 + i * 40);
            }
            gr.FillEllipse(brush,306,306,8,8);
            gr.FillEllipse(brush, 147, 147, 6, 6);
            gr.FillEllipse(brush, 467, 147, 6, 6);
            gr.FillEllipse(brush, 147, 467, 6, 6);
            gr.FillEllipse(brush, 467, 467, 6, 6);

        }

        private void gobangBoardGroupBox_MouseClick(object sender, MouseEventArgs e)
        {
            int x, y;
            if (blnBegin)
            {
                x = (e.X - 10) / 40;
                y = (e.Y - 10) / 40;
                PutAGobang(personGobangColor, x, y);
                if (Forbiden(personGobangColor, x, y))
                {
                    MessageBox.Show("你输了！本点为禁手点！", "本局结果", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    blnBegin = false;
                    personRadioButton.Enabled = true;
                    computerRadioButton.Enabled = true;
                    startButton.Enabled = true;
                }
                else if (Win(personGobangColor, x, y))
                {
                    MessageBox.Show("恭喜你，你赢了！", "本局结果", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    blnBegin = false;
                    personRadioButton.Enabled = true;
                    computerRadioButton.Enabled = true;
                    startButton.Enabled = true;
                }
                else if (totalGobangCount == 225)
                {
                    MessageBox.Show("棋盘已满，本局平棋！", "本局结果", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    blnBegin = false;
                    personRadioButton.Enabled = true;
                    computerRadioButton.Enabled = true;
                    startButton.Enabled = true;
                }
                else
                {
                    Point bestPoint = new Point();
                    if (FindBestPoint(ref bestPoint))
                    {
                        PutAGobang(computerGobangColor, bestPoint);
                        if (Win(computerGobangColor, bestPoint))
                        {
                            MessageBox.Show("你输了！加油哦！", "本局结果", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            blnBegin = false;
                            personRadioButton.Enabled = true;
                            computerRadioButton.Enabled = true;
                            startButton.Enabled = true;
                        }
                        else if(totalGobangCount==225)
                        {
                            MessageBox.Show("棋盘已满，本局平棋！", "本局结果", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            blnBegin = false;
                            personRadioButton.Enabled = true;
                            computerRadioButton.Enabled = true;
                            startButton.Enabled = true;
                        }

                    }
                    else
                    {
                        MessageBox.Show("恭喜你，你赢了！棋盘上所有点均为电脑的禁手点！", "本局结果", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        blnBegin = false;
                        personRadioButton.Enabled = true;
                        computerRadioButton.Enabled = true;
                        startButton.Enabled = true;
                    }
                }
            }
        }

        private bool Forbiden(int gobangColor, int x, int y)
        {
            if(gobangColor==white)
                return false;
            else 
            {
                int temp = virtualGobangBoard[x, y];
                bool blntemp;
                virtualGobangBoard[x,y]=background;
                blntemp = (GetGobangPower(black, x, y) == -1);
                virtualGobangBoard[x, y] = temp;
                return blntemp;
            }
        }

        private void PutAGobang(int gobangColor, Point point)//放一个gobangColor色的棋在bestPoint上
        {
            PutAGobang(gobangColor, point.X, point.Y);
        }

        private void PutAGobang(int gobangColor, int x, int y)//放一个gobangColor色的棋在(x,y)上
        {
            Point tempPoint=new Point();
            if (gobangColor == black)
            {
                gobangPictureBox[x, y].BackgroundImage = global::FiveChess.Properties.Resources.blackstone;
                virtualGobangBoard[x, y] = black;
                gobangPictureBox[x, y].Image = global::FiveChess.Properties.Resources.lastblackstone;
                if (backStack.Count > 0)
                {
                    tempPoint = (Point)backStack.Pop();
                    gobangPictureBox[tempPoint.X, tempPoint.Y].Image = global::FiveChess.Properties.Resources._null;
                    backStack.Push(tempPoint);
                }
            }
            else
            {
                gobangPictureBox[x, y].BackgroundImage = global::FiveChess.Properties.Resources.whitestone;
                virtualGobangBoard[x, y] = white;
                gobangPictureBox[x, y].Image = global::FiveChess.Properties.Resources.lastwhitestone;
                if (backStack.Count > 0)
                {
                    tempPoint = (Point)backStack.Pop();
                    gobangPictureBox[tempPoint.X, tempPoint.Y].Image = global::FiveChess.Properties.Resources._null;
                    backStack.Push(tempPoint);
                }
            }
            tempPoint = new Point(x, y);
            backStack.Push(tempPoint);
            gobangPictureBox[x, y].Visible = true;
        }

        private int ConnectGobangsCount(int gobangColor, Point point1, Point point2)//求point1与point2之间可能形成五连子的gobangColor色棋的连子数（包括活棋）
        {
            int x, y,i,j,length,xPlus=0,yPlus=0,sum,maxSum=0;
            length=Math.Max(Math.Abs(point1.X-point2.X),Math.Abs(point1.Y-point2.Y))+1;
            if(point1.X!=point2.X) xPlus=1;
            if(point1.Y!=point2.Y) yPlus=(point2.Y-point1.Y)/Math.Abs(point2.Y-point1.Y);
            for (i = 0; i < length - 4; i++)
            {
                x = point1.X + i * xPlus;
                y = point1.Y + i * yPlus;
                sum = 0;
                for (j = 0; j < 5; j++)
                {
                    if (virtualGobangBoard[x + j * xPlus, y + j * yPlus] == gobangColor)
                        sum++;
                    else if (virtualGobangBoard[x + j * xPlus, y + j * yPlus] == -gobangColor)
                    {
                        sum = 0;
                        break;
                    }
                }
                if (maxSum < sum)
                    maxSum = sum;
            }
            return maxSum;
        }

        private bool lenthConnect(int x, int y)//判断在(x,y)点放一黑棋后是否有长连禁手
        {
            Point left, right,top,down,leftTop, rightTop, leftDown, rightDown;
            int temp;
            int tempppppp=virtualGobangBoard[x,y];
            virtualGobangBoard[x, y] = black;
            bool blntemp;
            left = new Point(Math.Max(0, x - 5), y);
            right = new Point(Math.Min(14, x + 5), y);
            top = new Point(x, Math.Max(0, y - 5));
            down = new Point(x, Math.Min(14, y + 5));
            temp = Math.Min(x - left.X, y - top.Y);
            leftTop = new Point(x - temp, y - temp);
            temp = Math.Min(x - left.X, down.Y - y);
            leftDown = new Point(x - temp, y + temp);
            temp = Math.Min(right.X - x, y - top.Y);
            rightTop = new Point(x + temp, y - temp);
            temp = Math.Min(right.X - x, down.Y - y);
            rightDown = new Point(x + temp, y + temp);
            blntemp = (lenthConnectTowPoint(left, right) || lenthConnectTowPoint(top, down) || lenthConnectTowPoint(leftTop, rightDown) || lenthConnectTowPoint(leftDown, rightTop));
            virtualGobangBoard[x, y] = tempppppp;
            return blntemp;

        }

        private bool lenthConnectTowPoint(Point point1, Point point2)//求point1与point2之间是否能形成长连禁手
        {
            int x, y, i, j, length, xPlus = 0, yPlus = 0, sum;
            length = Math.Max(Math.Abs(point1.X - point2.X), Math.Abs(point1.Y - point2.Y)) + 1;
            if (point1.X != point2.X) xPlus = 1;
            if (point1.Y != point2.Y) yPlus = (point2.Y - point1.Y) / Math.Abs(point2.Y - point1.Y);
            for (i = 0; i < length - 5; i++)
            {
                x = point1.X + i * xPlus;
                y = point1.Y + i * yPlus;
                sum = 0;
                for (j = 0; j < 6; j++)
                {
                    if (virtualGobangBoard[x + j * xPlus, y + j * yPlus] == black)
                        sum++;
                    else
                    {
                        sum = 0;
                        break;
                    }
                }
                if (sum == 6) return true;
            }
            return false;
        }

        private bool ActiveConnectGobangs(int gobangColor, int count, Point point1, Point point2)//判断point1与point2之间是否有gobangColor色的活count
        {
            int x, y, i, j, length, xPlus = 0, yPlus = 0, sum;
            int temp1, temp2;
            temp1=Math.Min(Math.Min(Math.Min(5-count,point1.X),point1.Y),14-point1.Y);
            temp2=Math.Min(Math.Min(Math.Min(5-count,14-point2.X),14-point2.Y),point2.Y);
            length = Math.Max(Math.Abs(point1.X - point2.X), Math.Abs(point1.Y - point2.Y)) + 1+temp1+temp2;
            if (point1.X != point2.X) xPlus = 1;
            if (point1.Y != point2.Y) yPlus = (point2.Y - point1.Y) / Math.Abs(point2.Y - point1.Y);
            for (i = 0; i < length - 4; i++)
            {
                x = point1.X -temp1*xPlus+ i * xPlus;
                y = point1.Y-temp1*yPlus + i * yPlus;
                if (x + 4 * xPlus > 14 || y + 4 * yPlus > 14)
                    break;
                sum = 0;
                for (j = 0; j < 4; j++)
                {
                    if (virtualGobangBoard[x + j * xPlus, y + j * yPlus] == gobangColor)
                        sum++;
                    else if (virtualGobangBoard[x + j * xPlus, y + j * yPlus] == -gobangColor)
                    {
                        sum = 0;
                        break;
                    }
                }
                if (0<x&&0<=y-yPlus&&y-yPlus<=14)
                {
                    if(sum==count&&virtualGobangBoard[x-xPlus,y-yPlus]==background&&virtualGobangBoard[x+4*xPlus,y+4*yPlus]==background)
                      return true;
                }
            }
            return false;
        }

        private bool BreakActiveConnectGobangs(int gobangColor, int count, int x, int y, Point point1, Point point2)//(x,y)处放gobangColor色棋后形成活count,且放一反色棋后破坏棋形成活count的能，注意返回时不能破坏棋盘
        {
            if (!ActiveConnectGobangs(gobangColor, count, point1, point2)) return false;
            if (count == 5) return false;
            else if (count == 4) return true;
            else
            {
                bool blnFlag;
                virtualGobangBoard[x, y] = -gobangColor;
                blnFlag = !ActiveConnectGobangs(gobangColor, count - 1, point1, point2);
                virtualGobangBoard[x, y] = gobangColor;
                return blnFlag;
            }
        }

        private bool FindBestPoint(ref Point bestPoint)
        {
            Conclution totalConclution=Conclution.lose;
            int i, bestStepNumber=0;
            StackElement tempStackElement= new StackElement();
            if (!FindBestFivePointsAndFormAStackElement(computerGobangColor,ref tempStackElement))
                return false;
            backTrackStack.Push(tempStackElement);
            while (backTrackStack.Count > 0)//栈非空
            {
                tempStackElement = (StackElement)backTrackStack.Pop();
                if (tempStackElement.pointNumber < tempStackElement.pointsCount)
                {
                    //在虚拟棋盘上下一棋
                    virtualGobangBoard[tempStackElement.bestFivePoints[tempStackElement.pointNumber].X, tempStackElement.bestFivePoints[tempStackElement.pointNumber].Y] = tempStackElement.gobangColor;
                    if (Win(tempStackElement.gobangColor, tempStackElement.bestFivePoints[tempStackElement.pointNumber]))
                    {//赢棋，不在继续探测
                        tempStackElement.theConclution[tempStackElement.pointNumber] = Conclution.win;
                        tempStackElement.stepNumber[tempStackElement.pointNumber] = backTrackStack.Count + 1;
                        //在虚拟棋盘上退一棋
                        virtualGobangBoard[tempStackElement.bestFivePoints[tempStackElement.pointNumber].X, tempStackElement.bestFivePoints[tempStackElement.pointNumber].Y] = background;
                        tempStackElement.pointNumber++;
                        backTrackStack.Push(tempStackElement);
                    }
                    else if (backTrackStack.Count == M - 1)
                    {//将此元素压入栈后栈满，不在继续探测
                        tempStackElement.theConclution[tempStackElement.pointNumber] = Conclution.equal;
                        tempStackElement.stepNumber[tempStackElement.pointNumber] = M;
                        //在虚拟棋盘上退一棋
                        virtualGobangBoard[tempStackElement.bestFivePoints[tempStackElement.pointNumber].X, tempStackElement.bestFivePoints[tempStackElement.pointNumber].Y] = background;
                        tempStackElement.pointNumber++;
                        backTrackStack.Push(tempStackElement);
                    }
                    else
                    {//另一方继续下棋向下探测
                        tempStackElement.pointNumber++;
                        backTrackStack.Push(tempStackElement);
                        FindBestFivePointsAndFormAStackElement(-tempStackElement.gobangColor,ref tempStackElement);
                        backTrackStack.Push(tempStackElement);
                    }
                }//end if
                else//栈顶元素无点或点均已试过
                {
                    if (tempStackElement.pointsCount == 0)//栈顶元素无点，且弹出后栈必非空
                    {
                        tempStackElement = (StackElement)backTrackStack.Pop();
                        tempStackElement.theConclution[tempStackElement.pointNumber-1] = Conclution.win;
                        tempStackElement.stepNumber[tempStackElement.pointNumber-1] = backTrackStack.Count + 1;
                        //在虚拟棋盘上退一棋
                        virtualGobangBoard[tempStackElement.bestFivePoints[tempStackElement.pointNumber-1].X, tempStackElement.bestFivePoints[tempStackElement.pointNumber-1].Y] = background;
                        backTrackStack.Push(tempStackElement);
                    }
                    else//栈顶元素中点均已试过
                    {
                        //寻找栈顶元素中点的最好结局
                        totalConclution = tempStackElement.theConclution[0];
                        for (i = 0; i < tempStackElement.pointsCount; i++)
                            if (totalConclution < tempStackElement.theConclution[i])
                                totalConclution = tempStackElement.theConclution[i];
                        //寻找最佳步数
                        if (totalConclution == Conclution.win)
                        {
                            bestStepNumber = M + 2;
                            for (i = 0; i < tempStackElement.pointsCount; i++)
                                if (totalConclution == tempStackElement.theConclution[i] && bestStepNumber > tempStackElement.stepNumber[i])
                                    bestStepNumber = tempStackElement.stepNumber[i];
                        }
                        else//totalConclution==Conclution.equal或lose
                        {
                            bestStepNumber = 0;
                            for (i = 0; i < tempStackElement.pointsCount; i++)
                                if (totalConclution == tempStackElement.theConclution[i] && bestStepNumber < tempStackElement.stepNumber[i])
                                    bestStepNumber = tempStackElement.stepNumber[i];
                        }
                        if (backTrackStack.Count > 0)//栈非空
                        {
                            tempStackElement = (StackElement)backTrackStack.Pop();
                            tempStackElement.theConclution[tempStackElement.pointNumber-1] = (Conclution)(0-totalConclution);
                            tempStackElement.stepNumber[tempStackElement.pointNumber-1] = bestStepNumber;
                            //在虚拟棋盘上退一棋
                            virtualGobangBoard[tempStackElement.bestFivePoints[tempStackElement.pointNumber-1].X, tempStackElement.bestFivePoints[tempStackElement.pointNumber-1].Y] = background;
                            backTrackStack.Push(tempStackElement);
                        }
                    }//end else
                }//end else
            }//end while
            //栈已空
            for (i = 0; i < tempStackElement.pointsCount; i++)
                if (totalConclution == tempStackElement.theConclution[i] && bestStepNumber == tempStackElement.stepNumber[i])
                    break;
            bestPoint = tempStackElement.bestFivePoints[i];
            return true;
        }

        private bool FindBestFivePointsAndFormAStackElement(int gobangColor, ref StackElement tempStackElement)//寻找最佳的五个点，并形成栈元素，若一个也找不到返回false
        {
            int[,] gobangPower = new int[15, 15];
            bool blnHaveFound;
            int x, y,i,max;
            tempStackElement.pointsCount = 0;
            for (x = 0; x < 15; x++)
                for (y = 0; y < 15; y++)
                    gobangPower[x, y] = GetGobangPower(gobangColor, x, y);
            for (i = 0; i < 5; i++)
            {//求第i个最佳点
                max = 0;
                for (x = 0; x < 15; x++)
                    for (y = 0; y < 15; y++)
                        if (max < gobangPower[x, y])
                            max = gobangPower[x, y];
                for(x=0;x<15;x++)
                {
                    blnHaveFound = false;
                    for (y = 0; y < 15; y++)
                        if (max == gobangPower[x, y])
                        {
                            tempStackElement.bestFivePoints[i] = new Point(x, y);
                            tempStackElement.pointsCount++;
                            gobangPower[x, y] = -1;
                            blnHaveFound = true;
                            break;
                        }
                    if (blnHaveFound) break;
                }

            }
            if (tempStackElement.pointsCount == 0)
                return false;
            else
            {
                tempStackElement.gobangColor = gobangColor;
                tempStackElement.pointNumber = 0;
                return true;
            }
        }//寻找最佳的五个点，并形成栈元素，若一个也找不到返回false

        private int GetGobangPower(int gobangColor, int x, int y)//求(x,y)点对于gobangColor色棋的权值
        {
            int totalPower;
            GobangPointAttributinton gobPtAttri=new GobangPointAttributinton();
            Point left, right, top, down, leftTop, rightTop, leftDown, rightDown;
            int temp,connectCount;
            left = new Point(Math.Max(0, x - 4), y);
            right = new Point(Math.Min(14, x + 4), y);
            top = new Point(x, Math.Max(0, y - 4));
            down = new Point(x, Math.Min(14, y + 4));
            temp = Math.Min(x - left.X, y - top.Y);
            leftTop = new Point(x - temp, y - temp);
            temp = Math.Min(x - left.X, down.Y - y);
            leftDown = new Point(x - temp, y + temp);
            temp = Math.Min(right.X - x, y - top.Y);
            rightTop = new Point(x + temp, y - temp);
            temp = Math.Min(right.X - x, down.Y - y);
            rightDown = new Point(x + temp, y + temp);
            if (gobangColor == black)
            {
                if (virtualGobangBoard[x, y] != background)
                    return -2;
                else
                {
                    ///
                    ///处理黑棋连子情况
                    ///
                    virtualGobangBoard[x, y] = black;
                    //左右方向
                    connectCount = ConnectGobangsCount(black, left, right);
                    gobPtAttri.blackConnect[connectCount]++;
                    if (ActiveConnectGobangs(black, connectCount, left, right))
                    {
                        gobPtAttri.blackConnect[connectCount]--;
                        gobPtAttri.blackActive[connectCount]++;
                    }
                    //上下方向
                    connectCount = ConnectGobangsCount(black, top, down);
                    gobPtAttri.blackConnect[connectCount]++;
                    if (ActiveConnectGobangs(black, connectCount, top, down))
                    {
                        gobPtAttri.blackConnect[connectCount]--;
                        gobPtAttri.blackActive[connectCount]++;
                    }
                    //左上_右下方向
                    connectCount = ConnectGobangsCount(black, leftTop, rightDown);
                    gobPtAttri.blackConnect[connectCount]++;
                    if (ActiveConnectGobangs(black, connectCount, leftTop, rightDown))
                    {
                        gobPtAttri.blackConnect[connectCount]--;
                        gobPtAttri.blackActive[connectCount]++;
                    }
                    //左下_右上方向
                    connectCount = ConnectGobangsCount(black, leftDown, rightTop);
                    gobPtAttri.blackConnect[connectCount]++;
                    if (ActiveConnectGobangs(black, connectCount, leftDown, rightTop))
                    {
                        gobPtAttri.blackConnect[connectCount]--;
                        gobPtAttri.blackActive[connectCount]++;
                    }
                    virtualGobangBoard[x, y] = background;

                    ///
                    ///处理白棋连子情况
                    ///

                    virtualGobangBoard[x, y] = white;
                    //左右方向
                    connectCount = ConnectGobangsCount(white, left, right);
                    gobPtAttri.whiteConnect[connectCount]++;
                    if (BreakActiveConnectGobangs(white, connectCount, x, y, left, right))
                    {
                        gobPtAttri.whiteConnect[connectCount]--;
                        gobPtAttri.whiteActive[connectCount]++;
                    }
                    //上下方向
                    connectCount = ConnectGobangsCount(white, top, down);
                    gobPtAttri.whiteConnect[connectCount]++;
                    if (BreakActiveConnectGobangs(white, connectCount, x, y, top, down))
                    {
                        gobPtAttri.whiteConnect[connectCount]--;
                        gobPtAttri.whiteActive[connectCount]++;
                    }
                    //左上_右下方向
                    connectCount = ConnectGobangsCount(white, leftTop, rightDown);
                    gobPtAttri.whiteConnect[connectCount]++;
                    if (BreakActiveConnectGobangs(white, connectCount, x, y, leftTop, rightDown))
                    {
                        gobPtAttri.whiteConnect[connectCount]--;
                        gobPtAttri.whiteActive[connectCount]++;
                    }
                    //左下_右上方向
                    connectCount = ConnectGobangsCount(white, leftDown, rightTop);
                    gobPtAttri.whiteConnect[connectCount]++;
                    if (BreakActiveConnectGobangs(white, connectCount, x, y, leftDown, rightTop))
                    {
                        gobPtAttri.whiteConnect[connectCount]--;
                        gobPtAttri.whiteActive[connectCount]++;
                    }
                    if (ActiveConnectGobangs(white, 3, left, right) && ConnectGobangsCount(white, left, right)<=3) gobPtAttri.tempActive3++;
                    if (ActiveConnectGobangs(white, 3, top, down) && ConnectGobangsCount(white, top, down)<=3) gobPtAttri.tempActive3++;
                    if (ActiveConnectGobangs(white, 3, leftTop, rightDown) && ConnectGobangsCount(white, leftTop, rightDown)<=3) gobPtAttri.tempActive3++;
                    if (ActiveConnectGobangs(white, 3, leftDown, rightTop) && ConnectGobangsCount(white, leftDown, rightTop)<=3) gobPtAttri.tempActive3++;
                    virtualGobangBoard[x, y] = background;
                    //
                    //开始求权值
                    //
                    if (gobPtAttri.blackActive[3] > 1 || gobPtAttri.blackActive[4] > 1 || lenthConnect(x, y))
                        return -1;//禁手
                    else if (gobPtAttri.blackConnect[5] > 0)
                        return 150000;
                    else if (gobPtAttri.whiteConnect[5] > 0)
                        return 140000;
                    else if (gobPtAttri.blackActive[4] > 0 || gobPtAttri.blackConnect[4] > 1)
                        return 130000;
                    else if (gobPtAttri.blackConnect[4] == 1 && gobPtAttri.blackActive[3] == 1)
                        return 120000;
                    else if (gobPtAttri.blackConnect[4] == 1 && gobPtAttri.blackConnect[3] > 0)
                        return 110000;
                    else if (gobPtAttri.whiteActive[4] > 0 || gobPtAttri.whiteConnect[4] > 1)
                        return 100000;
                    else if (gobPtAttri.whiteConnect[4] == 1 && gobPtAttri.tempActive3 == 1)
                        return 90000;
                    else if (gobPtAttri.whiteActive[3] > 1)
                        return 80000;
                    else if (gobPtAttri.whiteConnect[4] == 1 && gobPtAttri.whiteConnect[3] > 0)
                        return 70000;
                    else
                    {
                        totalPower = (gobPtAttri.blackConnect[4] + gobPtAttri.blackActive[3]) * 6250 + (gobPtAttri.blackConnect[3] + gobPtAttri.blackActive[2] + gobPtAttri.whiteConnect[4] + gobPtAttri.whiteActive[3]) * 1250
                        + (gobPtAttri.blackConnect[2] + gobPtAttri.whiteConnect[3] + gobPtAttri.whiteActive[2]) * 250 + gobPtAttri.blackActive[1]*50 + (gobPtAttri.blackConnect[1] + gobPtAttri.whiteConnect[2] + gobPtAttri.whiteActive[1]) * 10 + gobPtAttri.whiteConnect[1] * 2;
                        return totalPower;
                    }

                }
            }
            else //gobangColor==white
            {
                if (virtualGobangBoard[x, y] != background)
                    return -2;
                else
                {
                    ///
                    ///处理黑棋连子情况
                    ///
                    virtualGobangBoard[x, y] = black;



                  

                    //左右方向
                  connectCount = ConnectGobangsCount(black, left, right); 
                    gobPtAttri.blackConnect[connectCount]++;
                    if (BreakActiveConnectGobangs(black, connectCount, x, y, left, right))
                    {
                        gobPtAttri.blackConnect[connectCount]--;
                        gobPtAttri.blackActive[connectCount]++;
                    }
                    //上下方向
                    connectCount = ConnectGobangsCount(black, top, down);
                    gobPtAttri.blackConnect[connectCount]++;
                    if (BreakActiveConnectGobangs(black, connectCount, x, y, top, down))
                    {
                        gobPtAttri.blackConnect[connectCount]--;
                        gobPtAttri.blackActive[connectCount]++;
                    }
                    //左上_右下方向
                    connectCount = ConnectGobangsCount(black, leftTop, rightDown);
                    gobPtAttri.blackConnect[connectCount]++;
                    if (BreakActiveConnectGobangs(black, connectCount, x, y, leftTop, rightDown))
                    {
                        gobPtAttri.blackConnect[connectCount]--;
                        gobPtAttri.blackActive[connectCount]++;
                    }
                    //左下_右上方向
                    connectCount = ConnectGobangsCount(black, leftDown, rightTop);
                    gobPtAttri.blackConnect[connectCount]++;
                    if (BreakActiveConnectGobangs(black, connectCount, x, y, leftDown, rightTop))
                    {
                        gobPtAttri.blackConnect[connectCount]--;
                        gobPtAttri.blackActive[connectCount]++;
                    }
                    if (ActiveConnectGobangs(black, 3, left, right)&&ConnectGobangsCount(black, left, right)<=3) gobPtAttri.tempActive3++;
                    if (ActiveConnectGobangs(black, 3, top, down) && ConnectGobangsCount(black, top, down)<=3) gobPtAttri.tempActive3++;
                    if (ActiveConnectGobangs(black, 3, leftTop, rightDown) && ConnectGobangsCount(black, leftTop, rightDown)<=3) gobPtAttri.tempActive3++;
                    if (ActiveConnectGobangs(black, 3, leftDown, rightTop) && ConnectGobangsCount(black, leftDown, rightTop)<=3) gobPtAttri.tempActive3++;
                    virtualGobangBoard[x, y] = background;

                    ///
                    ///处理白棋连子情况
                    ///
                    if (x == 6 && y == 9)
                        x = 6;


                    virtualGobangBoard[x, y] = white;
                    //左右方向
                    connectCount = ConnectGobangsCount(white, left, right);
                    gobPtAttri.whiteConnect[connectCount]++;
                    if (ActiveConnectGobangs(white, connectCount, left, right))
                    {
                        gobPtAttri.whiteConnect[connectCount]--;
                        gobPtAttri.whiteActive[connectCount]++;
                    }
                    //上下方向
                    connectCount = ConnectGobangsCount(white, top, down);
                    gobPtAttri.whiteConnect[connectCount]++;
                    if (ActiveConnectGobangs(white, connectCount, top, down))
                    {
                        gobPtAttri.whiteConnect[connectCount]--;
                        gobPtAttri.whiteActive[connectCount]++;
                    }
                    //左上_右下方向
                    connectCount = ConnectGobangsCount(white, leftTop, rightDown);
                    gobPtAttri.whiteConnect[connectCount]++;
                    if (ActiveConnectGobangs(white, connectCount, leftTop, rightDown))
                    {
                        gobPtAttri.whiteConnect[connectCount]--;
                        gobPtAttri.whiteActive[connectCount]++;
                    }
                    //左下_右上方向
                    connectCount = ConnectGobangsCount(white, leftDown, rightTop);
                    gobPtAttri.whiteConnect[connectCount]++;
                    if (ActiveConnectGobangs(white, connectCount, leftDown, rightTop))
                    {
                        gobPtAttri.whiteConnect[connectCount]--;
                        gobPtAttri.whiteActive[connectCount]++;
                    }
                    virtualGobangBoard[x, y] = background;
                    //
                    //开始求权值
                    //
                    bool blnBlackForbiden=(gobPtAttri.tempActive3>1||gobPtAttri.blackActive[4]>1||lenthConnect(x,y));
                    if (gobPtAttri.whiteConnect[5] > 0)
                        return 150000;
                    else if (gobPtAttri.blackConnect[5]>0 && !blnBlackForbiden)
                        return 140000;
                    else if (gobPtAttri.whiteActive[4] > 0 || gobPtAttri.whiteConnect[4] > 1)
                        return 130000;
                    else if (gobPtAttri.whiteConnect[4] == 1 && gobPtAttri.whiteActive[3] > 0)
                        return 120000;
                    else if (gobPtAttri.blackActive[4] == 1 && !blnBlackForbiden || gobPtAttri.blackConnect[4] > 1 && !blnBlackForbiden)
                        return 110000;
                    else if (gobPtAttri.whiteConnect[4] == 1 && gobPtAttri.whiteConnect[3] > 0)
                        return 100000;
                    else if (gobPtAttri.blackConnect[4] > 0 && gobPtAttri.tempActive3 == 1 && !blnBlackForbiden)
                        return 90000;
                    else if (gobPtAttri.whiteActive[3] > 1)
                        return 80000;
                    else if (gobPtAttri.blackConnect[4] > 0 && gobPtAttri.blackConnect[3] > 0 && !blnBlackForbiden)
                        return 70000;
                    else
                    {
                        totalPower=(gobPtAttri.whiteConnect[4]+gobPtAttri.whiteActive[3])*6250+(gobPtAttri.whiteConnect[3]+gobPtAttri.whiteActive[2]+gobPtAttri.blackConnect[4]+gobPtAttri.blackActive[3])*1250
                        + (gobPtAttri.whiteConnect[2] + gobPtAttri.blackConnect[3] + gobPtAttri.blackActive[2]) * 250 + gobPtAttri.whiteActive[1]*50+ (gobPtAttri.whiteConnect[1] + gobPtAttri.blackConnect[2] + gobPtAttri.blackActive[1]) * 10 + gobPtAttri.blackConnect[1] * 2;
                        return totalPower;
                    }
                }
            }
        }

        private bool Win(int gobangColor, int x, int y)//在(x,y)上放一gobangColor色的棋后，判断gobangColor色棋是否赢
        {
            bool blnWin;
            virtualGobangBoard[x, y] = background;
            if (GetGobangPower(gobangColor, x, y) >= 150000)
                blnWin = true;
            else
                blnWin = false;
            virtualGobangBoard[x, y] = gobangColor;
            return blnWin;

        }

        private bool Win(int gobangColor, Point point)
        {
            return Win(gobangColor, point.X, point.Y);
        }

        private void gobangBoardGroupBox_MouseMove(object sender, MouseEventArgs e)//鼠标在棋盘上移动式画红方框
        {
            int x, y;
            Graphics gr = gobangBoardGroupBox.CreateGraphics();
            Pen lastPen = new Pen(Color.BurlyWood, 1);
            Pen nowPen = new Pen(Color.Red, 1);
            if (lastMovePoint.X != -1)
            {
                gr.DrawLine(lastPen, lastMovePoint.X - 15, lastMovePoint.Y - 15, lastMovePoint.X - 15, lastMovePoint.Y - 5);
                gr.DrawLine(lastPen, lastMovePoint.X - 15, lastMovePoint.Y - 15, lastMovePoint.X - 5, lastMovePoint.Y - 15);
                gr.DrawLine(lastPen, lastMovePoint.X + 15, lastMovePoint.Y - 15, lastMovePoint.X + 5, lastMovePoint.Y - 15);
                gr.DrawLine(lastPen, lastMovePoint.X + 15, lastMovePoint.Y - 15, lastMovePoint.X + 15, lastMovePoint.Y - 5);
                gr.DrawLine(lastPen, lastMovePoint.X - 15, lastMovePoint.Y + 15, lastMovePoint.X - 15, lastMovePoint.Y + 5);
                gr.DrawLine(lastPen, lastMovePoint.X - 15, lastMovePoint.Y + 15, lastMovePoint.X - 5, lastMovePoint.Y + 15);
                gr.DrawLine(lastPen, lastMovePoint.X + 15, lastMovePoint.Y + 15, lastMovePoint.X + 15, lastMovePoint.Y + 5);
                gr.DrawLine(lastPen, lastMovePoint.X + 15, lastMovePoint.Y + 15, lastMovePoint.X + 5, lastMovePoint.Y + 15);
                
            }
            if (10 < e.X && 10 < e.Y && e.X < 600 && e.Y < 600)
            {
                x = ((e.X - 10) / 40)*40+30;
                y = ((e.Y - 10) / 40)*40+30;
                gr.DrawLine(nowPen, x - 15, y - 15, x - 15, y - 7);
                gr.DrawLine(nowPen, x - 15, y - 15, x - 7, y - 15);
                gr.DrawLine(nowPen, x + 15, y - 15, x + 5, y - 15);
                gr.DrawLine(nowPen, x + 15, y - 15, x + 15, y - 7);
                gr.DrawLine(nowPen, x - 15, y + 15, x - 15, y + 7);
                gr.DrawLine(nowPen, x - 15, y + 15, x - 7, y + 15);
                gr.DrawLine(nowPen, x + 15, y + 15, x + 15, y + 7);
                gr.DrawLine(nowPen, x + 15, y + 15, x + 7, y + 15);
                lastMovePoint.X = x;
                lastMovePoint.Y = y;
            }
        }

        private void MainForm_MouseMove(object sender, MouseEventArgs e)//鼠标窗口上移动时将棋盘上红方框去掉
        {
            Graphics gr = gobangBoardGroupBox.CreateGraphics();
            Pen lastPen = new Pen(Color.BurlyWood, 1);
            if (lastMovePoint.X != -1)
            {
                gr.DrawLine(lastPen, lastMovePoint.X - 15, lastMovePoint.Y - 15, lastMovePoint.X - 15, lastMovePoint.Y - 5);
                gr.DrawLine(lastPen, lastMovePoint.X - 15, lastMovePoint.Y - 15, lastMovePoint.X - 5, lastMovePoint.Y - 15);
                gr.DrawLine(lastPen, lastMovePoint.X + 15, lastMovePoint.Y - 15, lastMovePoint.X + 5, lastMovePoint.Y - 15);
                gr.DrawLine(lastPen, lastMovePoint.X + 15, lastMovePoint.Y - 15, lastMovePoint.X + 15, lastMovePoint.Y - 5);
                gr.DrawLine(lastPen, lastMovePoint.X - 15, lastMovePoint.Y + 15, lastMovePoint.X - 15, lastMovePoint.Y + 5);
                gr.DrawLine(lastPen, lastMovePoint.X - 15, lastMovePoint.Y + 15, lastMovePoint.X - 5, lastMovePoint.Y + 15);
                gr.DrawLine(lastPen, lastMovePoint.X + 15, lastMovePoint.Y + 15, lastMovePoint.X + 15, lastMovePoint.Y + 5);
                gr.DrawLine(lastPen, lastMovePoint.X + 15, lastMovePoint.Y + 15, lastMovePoint.X + 5, lastMovePoint.Y + 15);
                lastMovePoint.X = -1;
                lastMovePoint.Y = -1;

            }
        }

        private void personRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            if (personRadioButton.Checked)
            {
                blackLabel.Text = "玩家";
                whiteLabel.Text = "电脑";
            }
            else
            {
                blackLabel.Text = "电脑";
                whiteLabel.Text = "玩家";
            }
        }

        private void startButton_Click(object sender, EventArgs e)//开始游戏
        {
            int x, y;
            if (!blnBegin)
            {
                blnBegin = true;
                startButton.Enabled = false;
                personRadioButton.Enabled = false;
                computerRadioButton.Enabled = false;
                for (x = 0; x < 15; x++)
                    for (y = 0; y < 15; y++)
                    {
                        gobangPictureBox[x, y].Visible = false;
                        virtualGobangBoard[x, y] = background;
                    }
                while (backStack.Count > 0)
                    backStack.Pop();
                if (personRadioButton.Checked)
                {
                    personGobangColor = black;
                    computerGobangColor = white;
                }
                else
                {
                    computerGobangColor = black;
                    personGobangColor = white;
                    PutAGobang(computerGobangColor, 7, 7);
                }


            }
        }

        private void escButton_Click(object sender, EventArgs e)//退出游戏
        {
            if (MessageBox.Show("确定退出游戏？", "操作提示", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
                this.Close();
        }

        private void backButton_Click(object sender, EventArgs e)//悔棋
        {
            if (blnBegin)
            {
                int i;
                Point tempPoint = new Point();
                if (backStack.Count > 1)
                {
                    for (i = 0; i < 2; i++)
                    {
                        tempPoint = (Point)backStack.Pop();
                        gobangPictureBox[tempPoint.X, tempPoint.Y].Visible = false;
                        virtualGobangBoard[tempPoint.X, tempPoint.Y] = background;
                    }
                    if (backStack.Count > 0)
                    {
                        tempPoint = (Point)backStack.Pop();
                        if (virtualGobangBoard[tempPoint.X, tempPoint.Y] == black)
                            gobangPictureBox[tempPoint.X, tempPoint.Y].Image = global::FiveChess.Properties.Resources.lastblackstone;
                        else
                            gobangPictureBox[tempPoint.X, tempPoint.Y].Image = global::FiveChess.Properties.Resources.lastwhitestone;
                        backStack.Push(tempPoint);
                    }
                }
            }
        }

        private void aboutButton_Click(object sender, EventArgs e)//关于游戏
        {
            AboutForm ab = new AboutForm();
            ab.Show();
        }

        private void MainForm_HelpButtonClicked(object sender, CancelEventArgs e)//游戏帮助
        {
            HelpForm he = new HelpForm();
            he.Show();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            //动画由小渐大
            AnimateWindow(this.Handle, 1000, AW_CENTER | AW_ACTIVATE);
            //主界面渐变设置
            this.timer.Enabled = true;
            this.Opacity = 0;
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            AnimateWindow(this.Handle, 1000, AW_SLIDE | AW_HIDE | AW_VER_NEGATIVE);
            Application.Exit();
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            //让背景由0变到1
            if (this.Opacity < 1)
            {
                this.Opacity = this.Opacity + 0.05;
            }
            else
            {
                this.timer.Enabled = false;
            }
        }

        private void timerT_Tick(object sender, EventArgs e)
        {
            this.lbl.Text = System.DateTime.Now.ToString();
        }

    }

    public enum Conclution : int//结局
    {
        lose = -1,
        equal,
        win
    }

    public class GobangPointAttributinton//棋子点属性，包括连子数及权值
    {
        public int[] blackConnect=new int[6];
        public int[] blackActive = new int[6];
        public int[] whiteConnect = new int[6];
        public int[] whiteActive = new int[6];
        public int tempActive3;
        //public int totalPower;
    }

    public class StackElement//回溯栈元素
    {
        public int gobangColor;
        public Point[] bestFivePoints = new Point[5];
        public int pointsCount;
        public int pointNumber;
        public Conclution[] theConclution = new Conclution[5];
        public int[] stepNumber = new int[5];
    }
}
