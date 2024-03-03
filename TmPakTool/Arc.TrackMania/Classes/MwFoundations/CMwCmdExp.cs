using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Arc.TrackMania.Classes.MwFoundations
{
    public class CMwCmdExp : CMwCmdScript
    {
        private static Dictionary<Type, int> _precedence = new Dictionary<Type, int>()
        {
            { typeof(CMwCmdExpNot), 1 },
            { typeof(CMwCmdExpNeg), 1 },
            { typeof(CMwCmdExpVec2Neg), 1 },
            { typeof(CMwCmdExpVec3Neg), 1 },
            { typeof(CMwCmdExpIso4Inverse), 1 },

            { typeof(CMwCmdExpMult), 2 },
            { typeof(CMwCmdExpIso4Mult), 2 },
            { typeof(CMwCmdExpDiv), 2 },
            { typeof(CMwCmdExpVec2Mult), 2 },
            { typeof(CMwCmdExpVec3Mult), 2 },
            { typeof(CMwCmdExpVec3MultIso), 2 },
            { typeof(CMwCmdExpVec3Product), 2 },

            { typeof(CMwCmdExpNumDotProduct2), 3 },
            { typeof(CMwCmdExpNumDotProduct3), 3 },

            { typeof(CMwCmdExpAdd), 4 },
            { typeof(CMwCmdExpVec2Add), 4 },
            { typeof(CMwCmdExpVec3Add), 4 },
            { typeof(CMwCmdExpSub), 4 },
            { typeof(CMwCmdExpVec2Sub), 4 },
            { typeof(CMwCmdExpVec3Sub), 4 },

            { typeof(CMwCmdExpInf), 6 },
            { typeof(CMwCmdExpInfEgal), 6 },
            { typeof(CMwCmdExpSup), 6 },
            { typeof(CMwCmdExpSupEgal), 6 },

            { typeof(CMwCmdExpEgal), 7 },
            { typeof(CMwCmdExpDiff), 7 },

            { typeof(CMwCmdExpAnd), 11 },

            { typeof(CMwCmdExpOr), 12 }
        };

        public override uint ID
        {
            get { return 0x01038000; }
        }

        protected string BinOpToString(string op, CMwCmdExp value1Exp, CMwCmdExp value2Exp)
        {
            int opPrecedence;
            int value1Precedence;
            int value2Precedence;
            _precedence.TryGetValue(GetType(), out opPrecedence);
            _precedence.TryGetValue(value1Exp.GetType(), out value1Precedence);
            _precedence.TryGetValue(value2Exp.GetType(), out value2Precedence);

            if (value1Precedence <= opPrecedence && value2Precedence <= opPrecedence)
            {
                return string.Format("{0} {1} {2}", value1Exp, op, value2Exp);
            }
            else if (value1Precedence > opPrecedence && value2Precedence <= opPrecedence)
            {
                return string.Format("({0}) {1} {2}", value1Exp, op, value2Exp);
            }
            else if (value1Precedence <= opPrecedence && value2Precedence > opPrecedence)
            {
                return string.Format("{0} {1} ({2})", value1Exp, op, value2Exp);
            }
            else
            {
                return string.Format("({0}) {1} ({2})", value1Exp, op, value2Exp);
            }
        }
    }
}
