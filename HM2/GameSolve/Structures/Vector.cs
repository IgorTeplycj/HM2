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
        public double Velosity
        {
            get
            {
                if (Double.IsNaN(_Velosity))
                    throw new Exception("Невозможно прочитать значение мгновенной скорости");

                if (Double.IsInfinity(_Velosity))
                    throw new Exception("Невозможно прочитать значение мгновенной скорости");

                return _Velosity;
            }
            set
            {
                _Velosity = value;
            }
        }
        public double Angular { get; set; }
        public Coordinats Position(VelosityVect vel)
        {
            Coordinats newPos = new Coordinats
            {
                X = vel.Velosity * Math.Cos((double)vel.Angular * (double)(180.0 / Math.PI)),
                Y = vel.Velosity * Math.Sin((double)vel.Angular * (double)(180.0 / Math.PI)),
            };
            return newPos;
        }
    }
    public struct Vector
    {
        public Coordinats PositionNow { get; set; }
        public Coordinats Shift { get; set; }
        public VelosityVect @VelosityVect { get; set; }
        public int VolumeFuel { get; set; }

        public int Direction { get; set; }
        public int DirectionNumber { get; set; }
        public int AngularVelosity { get; set; }
        public int NextDirection
        {
            get
            {
                double X = this.PositionNow.X;
                double Y = this.PositionNow.Y;

                return (this.Direction + this.AngularVelosity) % this.DirectionNumber;
            }
        }

        public Vector Add(Vector newV)
        {
            Vector beforeV = new Vector();
            Coordinats newPos = new Coordinats();

            Coordinats velVect = newV.VelosityVect.Position(newV.VelosityVect);
            newPos.X = this.PositionNow.X + velVect.X + newV.Shift.X;
            newPos.Y = this.PositionNow.Y + velVect.Y + newV.Shift.Y;

            beforeV.PositionNow = newPos;
            beforeV.VelosityVect = new VelosityVect { Angular = 0, Velosity = 0 };
            return beforeV;
        }
    }
}
