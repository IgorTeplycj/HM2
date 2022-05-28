using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HM2.GameSolve.Structures
{
    public struct Coordinats
    {
        double _X;
        public double X
        {
            get
            {
                if (Double.IsNaN(_X))
                    throw new Exception("Невозможно прочитать значение координаты X");

                if (Double.IsInfinity(_X))
                    throw new Exception("Невозможно прочитать значение координаты X");

                return _X;
            }
            set
            {
                _X = value;
            }
        }

        double _Y;
        public double Y
        {
            get
            {
                if (Double.IsNaN(_Y))
                    throw new Exception("Невозможно прочитать значение координаты Y");

                if (Double.IsInfinity(_Y))
                    throw new Exception("Невозможно прочитать значение координаты Y");

                return _Y;
            }
            set
            {
                _Y = value;
            }
        }
    }
    public struct VelosityVect
    {
        double _Velosity;
        /// <summary>
        /// Модуль
        /// </summary>
        public double Velosity
        {
            get
            {
                if (Double.IsNaN(_Velosity))
                    throw new ArgumentException();

                if (Double.IsInfinity(_Velosity))
                    throw new ArgumentException();

                return _Velosity;
            }
            set
            {
                _Velosity = value;
            }
        }

        double _Angular;
        /// <summary>
        /// Угол Альфа
        /// </summary>
        public double Angular 
        { 
            get
            {
                if (Double.IsNaN(_Angular))
                    throw new ArgumentException();

                if (Double.IsInfinity(_Angular))
                    throw new ArgumentException();

                return _Angular;
            }
            set
            {
                _Angular = value;
            }
        }
        public Coordinats Position(VelosityVect vel)
        {
            Coordinats newPos = new Coordinats
            {
                X = vel.Velosity * Math.Cos((double)vel.Angular * (double)(Math.PI / 180.0)),
                Y = vel.Velosity * Math.Sin((double)vel.Angular * (double)(Math.PI / 180.0)),
            };
            return newPos;
        }

        public void Add(VelosityVect inptVect)
        {
            //результирующий угол между векторами
            double anglrResult = 180.0 - (double)Math.Abs((double)(this.Angular - inptVect.Angular));

            double _cos = Math.Cos(anglrResult * (double) (Math.PI/180.0));
            //Модуль результирующего вектора
            double C = Math.Pow((double)Math.Pow(this.Velosity, 2.0) + (double)Math.Pow(inptVect.Velosity, 2.0) +
                2.0 * this.Velosity * inptVect.Velosity * Math.Abs(_cos), 0.5);

            if (this.Velosity == 0)
                throw new DivideByZeroException();

            double arg = (double)(Math.Pow(C, 2.0) + Math.Pow(this.Velosity, 2.0) - Math.Pow(inptVect.Velosity, 2.0)) /
                (double)(2 * C * this.Velosity);
            double anglr = Math.Acos(arg) * (double)(180.0 / Math.PI);

            if(this.Angular > inptVect.Angular)
            {
                this.Angular -= anglr; 
            }
            else if(this.Angular < inptVect.Angular)
            {
                this.Angular += anglr;
            }

            this.Velosity = C;
        }

        public override string ToString()
        {
            return $"Velosity {this.Velosity}, Angular {this.Angular}";
        }
    }
    public struct Vector
    {
        /// <summary>
        /// Координаты объекта
        /// </summary>
        public Coordinats PositionNow { get; set; }
        /// <summary>
        /// Параметры перемещения объекта по полю
        /// </summary>
        public Coordinats Shift { get; set; }
        /// <summary>
        /// Текущий вектор скорости
        /// </summary>
        public VelosityVect VelosityVectNow { get; set; }
        /// <summary>
        /// Вектор модификатор текущей скорости
        /// </summary>
        public VelosityVect VelosityVectModifer { get; set; }
        /// <summary>
        /// Модифицирует вектор скорости согласно заданного модификатора
        /// </summary>
        public void ModifVelosityVect()
        {
            VelosityVect vectNow = this.VelosityVectNow;
            VelosityVect vectModif = this.VelosityVectModifer;

            vectNow.Add(vectModif);

            this.VelosityVectNow = vectNow;
        }
        /// <summary>
        /// Объем топлива объекта
        /// </summary>
        /// 
        public int VolumeFuel { get; set; }
        /// <summary>
        /// Скорость изменения топлива
        /// </summary>
        public int VelosityVolumeFuel { get; set; }
        /// <summary>
        /// Модифицирует объем топлива
        /// </summary>
        public void ModifVolumeFuel()
        {
            this.VolumeFuel += this.VelosityVolumeFuel;
        }


        public int Direction { get; set; }
        public int DirectionNumber { get; set; }
        /// <summary>
        /// Угловая скорость поворота
        /// </summary>
        public int AngularVelosity { get; set; }
        public int NextDirection
        {
            get
            {
                double X = this.PositionNow.X;
                double Y = this.PositionNow.Y;

                if (this.DirectionNumber == 0)
                    return 0;
                else
                   return (this.Direction + this.AngularVelosity) % this.DirectionNumber;
            }
        }

        public Vector Add(Vector newV)
        {
            Vector beforeV = this;
            Coordinats newPos = new Coordinats();

            Coordinats velVect = newV.VelosityVectNow.Position(newV.VelosityVectNow);
            newPos.X = this.PositionNow.X + velVect.X + newV.Shift.X;
            newPos.Y = this.PositionNow.Y + velVect.Y + newV.Shift.Y;

            beforeV.PositionNow = newPos;
            beforeV.VelosityVectNow = new VelosityVect { Angular = 0, Velosity = 0 };
            return beforeV;
        }
    }
}
