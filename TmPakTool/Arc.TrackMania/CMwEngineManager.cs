using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Arc.TrackMania.Classes.MwFoundations;

namespace Arc.TrackMania
{
    public class CMwEngineManager
    {
        private static Dictionary<int, CMwEngineInfo> _engines;
        private static Dictionary<uint, uint> _classIDMapping;
        private static Dictionary<uint, uint> _reverseClassIDMapping;
        private static Dictionary<Type, CMwClassInfo> _classInfoByType;

        public static IEnumerable<CMwEngineInfo> Engines
        {
            get { return _engines.Values; }
        }

        public static uint MapClassID(uint classID)
        {
            uint result;
            uint chunkIndex = classID & 0xFFF;
            if (_classIDMapping.TryGetValue(classID & 0xFFFFF000, out result))
                return result | chunkIndex;

            return classID;
        }

        public static uint ReverseMapClassID(uint classID)
        {
            uint result;
            uint chunkIndex = classID & 0xFFF;
            if (_reverseClassIDMapping.TryGetValue(classID & 0xFFFFF000, out result))
                return result | chunkIndex;

            return classID;
        }

        public static CMwEngineInfo GetEngineInfo(uint classID)
        {
            int engineNum = (int)(MapClassID(classID) >> 24);
            CMwEngineInfo engineInfo;
            _engines.TryGetValue(engineNum, out engineInfo);
            return engineInfo;
        }

        public static CMwClassInfo GetClassInfo(uint classID)
        {
            classID = MapClassID(classID);
            CMwEngineInfo engineInfo = GetEngineInfo(classID);
            if (engineInfo == null)
                return null;

            return engineInfo.GetClassInfo(classID);
        }

        public static CMwClassInfo GetClassInfo(Type classType)
        {
            CMwClassInfo classInfo;
            _classInfoByType.TryGetValue(classType, out classInfo);
            return classInfo;
        }

        public static CMwMemberInfo GetMemberInfo(uint memberID)
        {
            CMwClassInfo classInfo = GetClassInfo(memberID & 0xFFFFF000u);
            if (classInfo == null)
                return null;

            return classInfo.GetMemberInfo(memberID);
        }

        public static string GetClassName(uint classID)
        {
            CMwEngineInfo engineInfo = GetEngineInfo(classID);
            if (engineInfo == null)
                return "?::?";

            CMwClassInfo classInfo = GetClassInfo(classID);
            if (classInfo == null)
                return string.Format("{0}::?", engineInfo.Name);

            return string.Format("{0}::{1}", engineInfo.Name, classInfo.Name);
        }

        public static string GetChunkName(uint classChunkID)
        {
            return string.Format("{0}.{1:X03}", GetClassName(classChunkID), classChunkID & 0xFFF);
        }

        public static CMwNod CreateClassInstance(uint classID)
        {
            CMwClassInfo classInfo = GetClassInfo(classID);
            if (classInfo == null)
                return null;

            return classInfo.Instantiate();
        }

        static CMwEngineManager()
        {
            #region Fill _engines

            _engines = new Dictionary<int, CMwEngineInfo>()
            {
                { 0x00, new CMwEngineInfo("_00", new Dictionary<int, CMwClassInfo>()
                  {
	                { 0x105, new CMwClassInfo("CMwCmdExpStringConcat", new Dictionary<int, CMwMemberInfo>()
	                  {
	                  })
	                }
                  })
                },
                { 0x01, new CMwEngineInfo("MwFoundations", new Dictionary<int, CMwClassInfo>()
                  {
	                { 0x001, new CMwClassInfo("CMwNod", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("IdName", 0x00000029) }
	                  })
	                },
	                { 0x003, new CMwClassInfo("CMwEngine", new Dictionary<int, CMwMemberInfo>()
	                  {
	                  })
	                },
	                { 0x005, new CMwClassInfo("CMwCmd", new Dictionary<int, CMwMemberInfo>()
	                  {
	                  })
	                },
	                { 0x006, new CMwClassInfo("CMwParam", new Dictionary<int, CMwMemberInfo>()
	                  {
	                  })
	                },
	                { 0x007, new CMwClassInfo("CMwParamClass", new Dictionary<int, CMwMemberInfo>()
	                  {
	                  })
	                },
	                { 0x008, new CMwClassInfo("CMwParamStruct", new Dictionary<int, CMwMemberInfo>()
	                  {
	                  })
	                },
	                { 0x009, new CMwClassInfo("CMwParamAction", new Dictionary<int, CMwMemberInfo>()
	                  {
	                  })
	                },
	                { 0x00C, new CMwClassInfo("CMwParamBool", new Dictionary<int, CMwMemberInfo>()
	                  {
	                  })
	                },
	                { 0x00D, new CMwClassInfo("CMwParamEnum", new Dictionary<int, CMwMemberInfo>()
	                  {
	                  })
	                },
	                { 0x00E, new CMwClassInfo("CMwParamInteger", new Dictionary<int, CMwMemberInfo>()
	                  {
	                  })
	                },
	                { 0x00F, new CMwClassInfo("CMwParamIntegerRange", new Dictionary<int, CMwMemberInfo>()
	                  {
	                  })
	                },
	                { 0x010, new CMwClassInfo("CMwParamNatural", new Dictionary<int, CMwMemberInfo>()
	                  {
	                  })
	                },
	                { 0x011, new CMwClassInfo("CMwParamNaturalRange", new Dictionary<int, CMwMemberInfo>()
	                  {
	                  })
	                },
	                { 0x012, new CMwClassInfo("CMwCmdFastCall", new Dictionary<int, CMwMemberInfo>()
	                  {
	                  })
	                },
	                { 0x013, new CMwClassInfo("CMwParamReal", new Dictionary<int, CMwMemberInfo>()
	                  {
	                  })
	                },
	                { 0x014, new CMwClassInfo("CMwParamRealRange", new Dictionary<int, CMwMemberInfo>()
	                  {
	                  })
	                },
	                { 0x015, new CMwClassInfo("CMwParamString", new Dictionary<int, CMwMemberInfo>()
	                  {
	                  })
	                },
	                { 0x016, new CMwClassInfo("CMwParamVec3", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("x", 0x00000024) },
		                { 0x001, new CMwFieldInfo("y", 0x00000024) },
		                { 0x002, new CMwFieldInfo("z", 0x00000024) }
	                  })
	                },
	                { 0x017, new CMwClassInfo("CMwParamIso3", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("AxeXx", 0x00000024) },
		                { 0x001, new CMwFieldInfo("AxeXy", 0x00000024) },
		                { 0x002, new CMwFieldInfo("AxeYx", 0x00000024) },
		                { 0x003, new CMwFieldInfo("AxeYy", 0x00000024) },
		                { 0x004, new CMwFieldInfo("tx", 0x00000024) },
		                { 0x005, new CMwFieldInfo("ty", 0x00000024) }
	                  })
	                },
	                { 0x018, new CMwClassInfo("CMwParamColor", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("r", 0x00000024) },
		                { 0x001, new CMwFieldInfo("g", 0x00000024) },
		                { 0x002, new CMwFieldInfo("b", 0x00000024) }
	                  })
	                },
	                { 0x019, new CMwClassInfo("CMwParamVec2", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("x", 0x00000024) },
		                { 0x001, new CMwFieldInfo("y", 0x00000024) }
	                  })
	                },
	                { 0x01A, new CMwClassInfo("CMwParamIso4", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("AxeXx", 0x00000024) },
		                { 0x001, new CMwFieldInfo("AxeXy", 0x00000024) },
		                { 0x002, new CMwFieldInfo("AxeXz", 0x00000024) },
		                { 0x003, new CMwFieldInfo("AxeYx", 0x00000024) },
		                { 0x004, new CMwFieldInfo("AxeYy", 0x00000024) },
		                { 0x005, new CMwFieldInfo("AxeYz", 0x00000024) },
		                { 0x006, new CMwFieldInfo("AxeZx", 0x00000024) },
		                { 0x007, new CMwFieldInfo("AxeZy", 0x00000024) },
		                { 0x008, new CMwFieldInfo("AxeZz", 0x00000024) },
		                { 0x009, new CMwFieldInfo("tx", 0x00000024) },
		                { 0x00A, new CMwFieldInfo("ty", 0x00000024) },
		                { 0x00B, new CMwFieldInfo("tz", 0x00000024) }
	                  })
	                },
	                { 0x01C, new CMwClassInfo("CMwCmdBuffer", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("Cmds", 0x00000008) },
		                { 0x001, new CMwFieldInfo("CmdCount", 0x0000001F) }
	                  })
	                },
	                { 0x01E, new CMwClassInfo("CMwCmdFiber", new Dictionary<int, CMwMemberInfo>()
	                  {
	                  })
	                },
	                { 0x01F, new CMwClassInfo("CMwParamVec4", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("x", 0x00000024) },
		                { 0x001, new CMwFieldInfo("y", 0x00000024) },
		                { 0x002, new CMwFieldInfo("z", 0x00000024) },
		                { 0x003, new CMwFieldInfo("w", 0x00000024) }
	                  })
	                },
	                { 0x020, new CMwClassInfo("CMwCmdBufferCore", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("IsEnabled", 0x00000001) },
		                { 0x001, new CMwFieldInfo("ComputerTime", 0x0000001F) },
		                { 0x002, new CMwFieldInfo("HumanTime", 0x0000001F) },
		                { 0x003, new CMwFieldInfo("GameTime", 0x0000001F) },
		                { 0x004, new CMwFieldInfo("SimulationTime", 0x0000001F) },
		                { 0x005, new CMwFieldInfo("PeriodEstimated", 0x00000024) },
		                { 0x006, new CMwFieldInfo("DeltaSmoothed", 0x00000024) },
		                { 0x007, new CMwFieldInfo("PeriodSmoothing", 0x00000028) },
		                { 0x008, new CMwFieldInfo("DeltaSmoothing", 0x00000028) }
	                  })
	                },
	                { 0x022, new CMwClassInfo("CMwClassInfoViewer", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("ClassId", 0x0000001F) }
	                  })
	                },
	                { 0x024, new CMwClassInfo("CMwParamProc", new Dictionary<int, CMwMemberInfo>()
	                  {
	                  })
	                },
	                { 0x026, new CMwClassInfo("CMwRefBuffer", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("Count", 0x0000001F) },
		                { 0x001, new CMwFieldInfo("Nods", 0x00000007) },
		                { 0x002, new CMwFieldInfo("UseAddRefRelease", 0x00000001) },
		                { 0x003, new CMwFieldInfo("NodClassId", 0x0000001F) }
	                  })
	                },
	                { 0x027, new CMwClassInfo("CMwParamRefBuffer", new Dictionary<int, CMwMemberInfo>()
	                  {
	                  })
	                },
	                { 0x028, new CMwClassInfo("CMwParamStringInt", new Dictionary<int, CMwMemberInfo>()
	                  {
	                  })
	                },
	                { 0x029, new CMwClassInfo("CMwStatsValue", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("NbMaxSamples", 0x0000001F) },
		                { 0x001, new CMwFieldInfo("ComputeDeviatedMean", 0x00000001) },
		                { 0x002, new CMwFieldInfo("StdDevRatio", 0x00000024) },
		                { 0x003, new CMwFieldInfo("ComputeMedian", 0x00000001) },
		                { 0x004, new CMwFieldInfo("ReductionRatio", 0x0000001F) },
		                { 0x005, new CMwFieldInfo("ComputeBuckets", 0x00000001) },
		                { 0x006, new CMwFieldInfo("AutoBuckets", 0x00000001) },
		                { 0x007, new CMwFieldInfo("BucketsRanges", 0x00000025) },
		                { 0x008, new CMwMethodInfo("Log", 0x00000000, null, null) },
		                { 0x009, new CMwFieldInfo("NbSamples", 0x0000001F) },
		                { 0x00A, new CMwFieldInfo("Summary", 0x00000029) },
		                { 0x00B, new CMwFieldInfo("MeanInv", 0x00000024) },
		                { 0x00C, new CMwFieldInfo("Mean", 0x00000024) },
		                { 0x00D, new CMwFieldInfo("StdDev", 0x00000024) },
		                { 0x00E, new CMwFieldInfo("Min", 0x00000024) },
		                { 0x00F, new CMwFieldInfo("Max", 0x00000024) },
		                { 0x010, new CMwFieldInfo("Latest", 0x00000024) },
		                { 0x011, new CMwFieldInfo("Median", 0x00000024) },
		                { 0x012, new CMwFieldInfo("MedianStdDev", 0x00000024) },
		                { 0x013, new CMwFieldInfo("DeviatedMean", 0x00000024) },
		                { 0x014, new CMwFieldInfo("BucketsRatio", 0x00000025) }
	                  })
	                },
	                { 0x030, new CMwClassInfo("CMwCmdBlock", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("Cmds", 0x00000006) },
		                { 0x001, new CMwFieldInfo("IsSleeping", 0x00000001) }
	                  })
	                },
	                { 0x031, new CMwClassInfo("CMwCmdInst", new Dictionary<int, CMwMemberInfo>()
	                  {
	                  })
	                },
	                { 0x032, new CMwClassInfo("CMwCmdAffectIdent", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("Value", 0x00000005) }
	                  })
	                },
	                { 0x033, new CMwClassInfo("CMwCmdAffectParam", new Dictionary<int, CMwMemberInfo>()
	                  {
	                  })
	                },
	                { 0x034, new CMwClassInfo("CMwCmdCall", new Dictionary<int, CMwMemberInfo>()
	                  {
	                  })
	                },
	                { 0x035, new CMwClassInfo("CMwCmdFor", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("VariableName", 0x00000029) },
		                { 0x001, new CMwFieldInfo("Min", 0x00000005) },
		                { 0x002, new CMwFieldInfo("Max", 0x00000005) },
		                { 0x003, new CMwFieldInfo("Block", 0x00000005) }
	                  })
	                },
	                { 0x036, new CMwClassInfo("CMwCmdIf", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("Condition", 0x00000005) },
		                { 0x001, new CMwFieldInfo("IfBlock", 0x00000005) },
		                { 0x002, new CMwFieldInfo("ElseBlock", 0x00000005) }
	                  })
	                },
	                { 0x037, new CMwClassInfo("CMwCmdWhile", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("Condition", 0x00000005) },
		                { 0x001, new CMwFieldInfo("Block", 0x00000005) }
	                  })
	                },
	                { 0x038, new CMwClassInfo("CMwCmdExp", new Dictionary<int, CMwMemberInfo>()
	                  {
	                  })
	                },
	                { 0x039, new CMwClassInfo("CMwCmdExpAdd", new Dictionary<int, CMwMemberInfo>()
	                  {
	                  })
	                },
	                { 0x03A, new CMwClassInfo("CMwCmdExpAnd", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("Value1", 0x00000005) },
		                { 0x001, new CMwFieldInfo("Value2", 0x00000005) }
	                  })
	                },
	                { 0x03B, new CMwClassInfo("CMwCmdExpBool", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("Bool", 0x00000001) }
	                  })
	                },
	                { 0x03C, new CMwClassInfo("CMwCmdExpBoolIdent", new Dictionary<int, CMwMemberInfo>()
	                  {
	                  })
	                },
	                { 0x03D, new CMwClassInfo("CMwCmdExpBoolParam", new Dictionary<int, CMwMemberInfo>()
	                  {
	                  })
	                },
	                { 0x03E, new CMwClassInfo("CMwCmdExpDiff", new Dictionary<int, CMwMemberInfo>()
	                  {
	                  })
	                },
	                { 0x03F, new CMwClassInfo("CMwCmdExpDiv", new Dictionary<int, CMwMemberInfo>()
	                  {
	                  })
	                },
	                { 0x040, new CMwClassInfo("CMwCmdExpEgal", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("Value1", 0x00000005) },
		                { 0x001, new CMwFieldInfo("Value2", 0x00000005) }
	                  })
	                },
	                { 0x041, new CMwClassInfo("CMwCmdExpInf", new Dictionary<int, CMwMemberInfo>()
	                  {
	                  })
	                },
	                { 0x042, new CMwClassInfo("CMwCmdExpInfEgal", new Dictionary<int, CMwMemberInfo>()
	                  {
	                  })
	                },
	                { 0x043, new CMwClassInfo("CMwCmdExpMult", new Dictionary<int, CMwMemberInfo>()
	                  {
	                  })
	                },
	                { 0x044, new CMwClassInfo("CMwCmdExpNeg", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("Value", 0x00000005) }
	                  })
	                },
	                { 0x045, new CMwClassInfo("CMwCmdExpNot", new Dictionary<int, CMwMemberInfo>()
	                  {
	                  })
	                },
	                { 0x046, new CMwClassInfo("CMwCmdExpNum", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("Type", 0x0000001F) }
	                  })
	                },
	                { 0x047, new CMwClassInfo("CMwCmdExpNumIdent", new Dictionary<int, CMwMemberInfo>()
	                  {
	                  })
	                },
	                { 0x048, new CMwClassInfo("CMwCmdExpNumParam", new Dictionary<int, CMwMemberInfo>()
	                  {
	                  })
	                },
	                { 0x049, new CMwClassInfo("CMwCmdExpOr", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("Value1", 0x00000005) },
		                { 0x001, new CMwFieldInfo("Value2", 0x00000005) }
	                  })
	                },
	                { 0x04A, new CMwClassInfo("CMwCmdExpPower", new Dictionary<int, CMwMemberInfo>()
	                  {
	                  })
	                },
	                { 0x04B, new CMwClassInfo("CMwCmdExpSub", new Dictionary<int, CMwMemberInfo>()
	                  {
	                  })
	                },
	                { 0x04C, new CMwClassInfo("CMwCmdExpSup", new Dictionary<int, CMwMemberInfo>()
	                  {
	                  })
	                },
	                { 0x04D, new CMwClassInfo("CMwCmdExpSupEgal", new Dictionary<int, CMwMemberInfo>()
	                  {
	                  })
	                },
	                { 0x04E, new CMwClassInfo("CMwCmdExpString", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("String", 0x00000029) }
	                  })
	                },
	                { 0x04F, new CMwClassInfo("CMwCmdExpStringIdent", new Dictionary<int, CMwMemberInfo>()
	                  {
	                  })
	                },
	                { 0x050, new CMwClassInfo("CMwCmdExpStringParam", new Dictionary<int, CMwMemberInfo>()
	                  {
	                  })
	                },
	                { 0x052, new CMwClassInfo("CMwCmdScript", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwMethodInfo("Run", 0x00000000, null, null) }
	                  })
	                },
	                { 0x053, new CMwClassInfo("CMwCmdExpNumBin", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("Value1", 0x00000005) },
		                { 0x001, new CMwFieldInfo("Value2", 0x00000005) }
	                  })
	                },
	                { 0x054, new CMwClassInfo("CMwCmdExpBoolBin", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("Value1", 0x00000005) },
		                { 0x001, new CMwFieldInfo("Value2", 0x00000005) }
	                  })
	                },
	                { 0x055, new CMwClassInfo("CMwCmdExpNumFunction", new Dictionary<int, CMwMemberInfo>()
	                  {
	                  })
	                },
	                { 0x056, new CMwClassInfo("CMwCmdExpClass", new Dictionary<int, CMwMemberInfo>()
	                  {
	                  })
	                },
	                { 0x057, new CMwClassInfo("CMwCmdExpClassIdent", new Dictionary<int, CMwMemberInfo>()
	                  {
	                  })
	                },
	                { 0x058, new CMwClassInfo("CMwCmdExpClassParam", new Dictionary<int, CMwMemberInfo>()
	                  {
	                  })
	                },
	                { 0x059, new CMwClassInfo("CMwCmdExpEnum", new Dictionary<int, CMwMemberInfo>()
	                  {
	                  })
	                },
	                { 0x05A, new CMwClassInfo("CMwCmdExpEnumParam", new Dictionary<int, CMwMemberInfo>()
	                  {
	                  })
	                },
	                { 0x05B, new CMwClassInfo("CMwCmdSwitch", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("EnumParam", 0x00000005) },
		                { 0x001, new CMwFieldInfo("CaseArray", 0x00000006) },
		                { 0x002, new CMwFieldInfo("DefaultBlock", 0x00000005) }
	                  })
	                },
	                { 0x05C, new CMwClassInfo("CMwCmdExpVec2", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("Exp1", 0x00000005) },
		                { 0x001, new CMwFieldInfo("Exp2", 0x00000005) }
	                  })
	                },
	                { 0x05D, new CMwClassInfo("CMwCmdExpVec2Ident", new Dictionary<int, CMwMemberInfo>()
	                  {
	                  })
	                },
	                { 0x05E, new CMwClassInfo("CMwCmdExpVec2Param", new Dictionary<int, CMwMemberInfo>()
	                  {
	                  })
	                },
	                { 0x05F, new CMwClassInfo("CMwCmdExpVec3", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("Exp1", 0x00000005) },
		                { 0x001, new CMwFieldInfo("Exp2", 0x00000005) },
		                { 0x002, new CMwFieldInfo("Exp3", 0x00000005) }
	                  })
	                },
	                { 0x060, new CMwClassInfo("CMwCmdExpVec3Ident", new Dictionary<int, CMwMemberInfo>()
	                  {
	                  })
	                },
	                { 0x061, new CMwClassInfo("CMwCmdExpVec3Param", new Dictionary<int, CMwMemberInfo>()
	                  {
	                  })
	                },
	                { 0x062, new CMwClassInfo("CMwCmdExpIso4", new Dictionary<int, CMwMemberInfo>()
	                  {
	                  })
	                },
	                { 0x063, new CMwClassInfo("CMwCmdExpIso4Ident", new Dictionary<int, CMwMemberInfo>()
	                  {
	                  })
	                },
	                { 0x064, new CMwClassInfo("CMwCmdExpIso4Param", new Dictionary<int, CMwMemberInfo>()
	                  {
	                  })
	                },
	                { 0x065, new CMwClassInfo("CMwCmdBlockCast", new Dictionary<int, CMwMemberInfo>()
	                  {
	                  })
	                },
	                { 0x066, new CMwClassInfo("CMwCmdSwitchType", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("CaseArray", 0x00000006) },
		                { 0x001, new CMwFieldInfo("DefaultBlock", 0x00000005) }
	                  })
	                },
	                { 0x067, new CMwClassInfo("CMwCmdBlockMain", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("Variables", 0x00000006) },
		                { 0x001, new CMwMethodInfo("Execute", 0x00000041,
			                new uint[] { 0x01001000 },
			                new string[] { "This" })
		                },
		                { 0x002, new CMwFieldInfo("Methods", 0x00000006) }
	                  })
	                },
	                { 0x068, new CMwClassInfo("CMwCmdScriptVar", new Dictionary<int, CMwMemberInfo>()
	                  {
	                  })
	                },
	                { 0x069, new CMwClassInfo("CMwCmdScriptVarBool", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("Bool", 0x00000001) }
	                  })
	                },
	                { 0x06A, new CMwClassInfo("CMwCmdScriptVarClass", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("Class", 0x00000005) }
	                  })
	                },
	                { 0x06B, new CMwClassInfo("CMwCmdScriptVarFloat", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("Float", 0x00000024) }
	                  })
	                },
	                { 0x06C, new CMwClassInfo("CMwCmdScriptVarInt", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("Int", 0x0000000E) }
	                  })
	                },
	                { 0x06D, new CMwClassInfo("CMwCmdScriptVarIso4", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("Iso4", 0x00000013) }
	                  })
	                },
	                { 0x06E, new CMwClassInfo("CMwCmdScriptVarString", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("String", 0x00000029) }
	                  })
	                },
	                { 0x06F, new CMwClassInfo("CMwCmdScriptVarVec2", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("Vec2", 0x00000031) }
	                  })
	                },
	                { 0x070, new CMwClassInfo("CMwCmdScriptVarVec3", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("Vec3", 0x00000035) }
	                  })
	                },
	                { 0x071, new CMwClassInfo("CMwCmdAffectIdentBool", new Dictionary<int, CMwMemberInfo>()
	                  {
	                  })
	                },
	                { 0x072, new CMwClassInfo("CMwCmdAffectIdentClass", new Dictionary<int, CMwMemberInfo>()
	                  {
	                  })
	                },
	                { 0x073, new CMwClassInfo("CMwCmdAffectIdentIso4", new Dictionary<int, CMwMemberInfo>()
	                  {
	                  })
	                },
	                { 0x074, new CMwClassInfo("CMwCmdAffectIdentNum", new Dictionary<int, CMwMemberInfo>()
	                  {
	                  })
	                },
	                { 0x075, new CMwClassInfo("CMwCmdAffectIdentString", new Dictionary<int, CMwMemberInfo>()
	                  {
	                  })
	                },
	                { 0x076, new CMwClassInfo("CMwCmdAffectIdentVec2", new Dictionary<int, CMwMemberInfo>()
	                  {
	                  })
	                },
	                { 0x077, new CMwClassInfo("CMwCmdAffectIdentVec3", new Dictionary<int, CMwMemberInfo>()
	                  {
	                  })
	                },
	                { 0x078, new CMwClassInfo("CMwCmdAffectParamBool", new Dictionary<int, CMwMemberInfo>()
	                  {
	                  })
	                },
	                { 0x079, new CMwClassInfo("CMwCmdAffectParamClass", new Dictionary<int, CMwMemberInfo>()
	                  {
	                  })
	                },
	                { 0x07A, new CMwClassInfo("CMwCmdAffectParamEnum", new Dictionary<int, CMwMemberInfo>()
	                  {
	                  })
	                },
	                { 0x07B, new CMwClassInfo("CMwCmdAffectParamIso4", new Dictionary<int, CMwMemberInfo>()
	                  {
	                  })
	                },
	                { 0x07C, new CMwClassInfo("CMwCmdAffectParamNum", new Dictionary<int, CMwMemberInfo>()
	                  {
	                  })
	                },
	                { 0x07D, new CMwClassInfo("CMwCmdAffectParamString", new Dictionary<int, CMwMemberInfo>()
	                  {
	                  })
	                },
	                { 0x07E, new CMwClassInfo("CMwCmdAffectParamVec2", new Dictionary<int, CMwMemberInfo>()
	                  {
	                  })
	                },
	                { 0x07F, new CMwClassInfo("CMwCmdAffectParamVec3", new Dictionary<int, CMwMemberInfo>()
	                  {
	                  })
	                },
	                { 0x080, new CMwClassInfo("CBlockVariable", new Dictionary<int, CMwMemberInfo>()
	                  {
	                  })
	                },
	                { 0x082, new CMwClassInfo("CMwCmdSleep", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("Time", 0x00000005) }
	                  })
	                },
	                { 0x083, new CMwClassInfo("CMwCmdWait", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("Condition", 0x00000005) }
	                  })
	                },
	                { 0x084, new CMwClassInfo("CMwCmdLog", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("Text", 0x00000005) }
	                  })
	                },
	                { 0x085, new CMwClassInfo("CMwCmdExpVec2Add", new Dictionary<int, CMwMemberInfo>()
	                  {
	                  })
	                },
	                { 0x086, new CMwClassInfo("CMwCmdExpVec2Sub", new Dictionary<int, CMwMemberInfo>()
	                  {
	                  })
	                },
	                { 0x087, new CMwClassInfo("CMwCmdExpVec2Neg", new Dictionary<int, CMwMemberInfo>()
	                  {
	                  })
	                },
	                { 0x088, new CMwClassInfo("CMwCmdExpVec2Mult", new Dictionary<int, CMwMemberInfo>()
	                  {
	                  })
	                },
	                { 0x089, new CMwClassInfo("CMwCmdExpVec3Add", new Dictionary<int, CMwMemberInfo>()
	                  {
	                  })
	                },
	                { 0x08A, new CMwClassInfo("CMwCmdExpVec3Sub", new Dictionary<int, CMwMemberInfo>()
	                  {
	                  })
	                },
	                { 0x08B, new CMwClassInfo("CMwCmdExpVec3Neg", new Dictionary<int, CMwMemberInfo>()
	                  {
	                  })
	                },
	                { 0x08C, new CMwClassInfo("CMwCmdExpVec3Mult", new Dictionary<int, CMwMemberInfo>()
	                  {
	                  })
	                },
	                { 0x08D, new CMwClassInfo("CMwCmdExpVec3MultIso", new Dictionary<int, CMwMemberInfo>()
	                  {
	                  })
	                },
	                { 0x08E, new CMwClassInfo("CMwCmdExpVec3Product", new Dictionary<int, CMwMemberInfo>()
	                  {
	                  })
	                },
	                { 0x08F, new CMwClassInfo("CMwCmdExpIso4Mult", new Dictionary<int, CMwMemberInfo>()
	                  {
	                  })
	                },
	                { 0x090, new CMwClassInfo("CMwCmdExpIso4Inverse", new Dictionary<int, CMwMemberInfo>()
	                  {
	                  })
	                },
	                { 0x091, new CMwClassInfo("CMwCmdExpNumDotProduct2", new Dictionary<int, CMwMemberInfo>()
	                  {
	                  })
	                },
	                { 0x092, new CMwClassInfo("CMwCmdExpNumDotProduct3", new Dictionary<int, CMwMemberInfo>()
	                  {
	                  })
	                },
	                { 0x093, new CMwClassInfo("CMwCmdContainer", new Dictionary<int, CMwMemberInfo>()
	                  {
	                  })
	                },
	                { 0x094, new CMwClassInfo("CMwCmdReturn", new Dictionary<int, CMwMemberInfo>()
	                  {
	                  })
	                },
	                { 0x095, new CMwClassInfo("CMwCmdBreak", new Dictionary<int, CMwMemberInfo>()
	                  {
	                  })
	                },
	                { 0x096, new CMwClassInfo("CMwCmdContinue", new Dictionary<int, CMwMemberInfo>()
	                  {
	                  })
	                },
	                { 0x097, new CMwClassInfo("CMwCmdProc", new Dictionary<int, CMwMemberInfo>()
	                  {
	                  })
	                },
	                { 0x098, new CMwClassInfo("CMwCmdBlockProcedure", new Dictionary<int, CMwMemberInfo>()
	                  {
	                  })
	                },
	                { 0x099, new CMwClassInfo("CMwCmdBlockFunction", new Dictionary<int, CMwMemberInfo>()
	                  {
	                  })
	                },
	                { 0x09A, new CMwClassInfo("CMwCmdExpBoolFunction", new Dictionary<int, CMwMemberInfo>()
	                  {
	                  })
	                },
	                { 0x09B, new CMwClassInfo("CMwCmdExpClassFunction", new Dictionary<int, CMwMemberInfo>()
	                  {
	                  })
	                },
	                { 0x09C, new CMwClassInfo("CMwCmdExpIso4Function", new Dictionary<int, CMwMemberInfo>()
	                  {
	                  })
	                },
	                { 0x09D, new CMwClassInfo("CMwCmdExpStringFunction", new Dictionary<int, CMwMemberInfo>()
	                  {
	                  })
	                },
	                { 0x09E, new CMwClassInfo("CMwCmdExpVec2Function", new Dictionary<int, CMwMemberInfo>()
	                  {
	                  })
	                },
	                { 0x09F, new CMwClassInfo("CMwCmdExpVec3Function", new Dictionary<int, CMwMemberInfo>()
	                  {
	                  })
	                },
	                { 0x0A0, new CMwClassInfo("CMwCmdArrayAdd", new Dictionary<int, CMwMemberInfo>()
	                  {
	                  })
	                },
	                { 0x0A1, new CMwClassInfo("CMwCmdArrayRemove", new Dictionary<int, CMwMemberInfo>()
	                  {
	                  })
	                },
	                { 0x0A2, new CMwClassInfo("CMwCmdExpClassThis", new Dictionary<int, CMwMemberInfo>()
	                  {
	                  })
	                },
	                { 0x0A3, new CMwClassInfo("CMwCmdExpStringTrunc", new Dictionary<int, CMwMemberInfo>()
	                  {
	                  })
	                },
	                { 0x0A4, new CMwClassInfo("CMwCmdExpEnumIdent", new Dictionary<int, CMwMemberInfo>()
	                  {
	                  })
	                },
	                { 0x0A5, new CMwClassInfo("CMwCmdAffectIdentEnum", new Dictionary<int, CMwMemberInfo>()
	                  {
	                  })
	                },
	                { 0x0A6, new CMwClassInfo("CMwCmdScriptVarEnum", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwEnumInfo("Enum", new string[] {  }) }
	                  })
	                },
	                { 0x0A7, new CMwClassInfo("CMwCmdExpStringUpDownCase", new Dictionary<int, CMwMemberInfo>()
	                  {
	                  })
	                },
	                { 0x0A8, new CMwClassInfo("CMwCmdExpNumCastedEnum", new Dictionary<int, CMwMemberInfo>()
	                  {
	                  })
	                },
	                { 0x0A9, new CMwClassInfo("CMwCmdExpEnumCastedNum", new Dictionary<int, CMwMemberInfo>()
	                  {
	                  })
	                },
	                { 0x0B0, new CMwClassInfo("CMwParamQuat", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("w", 0x00000024) },
		                { 0x001, new CMwFieldInfo("x", 0x00000024) },
		                { 0x002, new CMwFieldInfo("y", 0x00000024) },
		                { 0x003, new CMwFieldInfo("z", 0x00000024) }
	                  })
	                },
	                { 0x0C0, new CMwClassInfo("CMwCmdFastCallUser", new Dictionary<int, CMwMemberInfo>()
	                  {
	                  })
	                },
	                { 0x0D0, new CMwClassInfo("CMwParamMwId", new Dictionary<int, CMwMemberInfo>()
	                  {
	                  })
	                }
                  })
                },
                { 0x03, new CMwEngineInfo("Game", new Dictionary<int, CMwClassInfo>()
                  {
	                { 0x000, new CMwClassInfo("CGameEngine", new Dictionary<int, CMwMemberInfo>()
	                  {
	                  })
	                },
	                { 0x001, new CMwClassInfo("CGameRule", new Dictionary<int, CMwMemberInfo>()
	                  {
	                  })
	                },
	                { 0x002, new CMwClassInfo("CGamePlayer", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("Control", 0x00000005) },
		                { 0x001, new CMwFieldInfo("Mobil", 0x00000005) },
		                { 0x002, new CMwFieldInfo("Name", 0x0000002D) },
		                { 0x003, new CMwFieldInfo("PlayerInfo", 0x00000005) },
		                { 0x004, new CMwFieldInfo("CameraSet", 0x00000005) }
	                  })
	                },
	                { 0x003, new CMwClassInfo("CGameControlPlayer", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("Player", 0x00000005) }
	                  })
	                },
	                { 0x004, new CMwClassInfo("CGameProcess", new Dictionary<int, CMwMemberInfo>()
	                  {
	                  })
	                },
	                { 0x005, new CMwClassInfo("CGameApp", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwMethodInfo("Start", 0x00000000, null, null) },
		                { 0x001, new CMwMethodInfo("Exit", 0x00000000, null, null) },
		                { 0x002, new CMwFieldInfo("Viewport", 0x00000005) },
		                { 0x003, new CMwFieldInfo("AudioPort", 0x00000005) },
		                { 0x004, new CMwFieldInfo("InputPort", 0x00000005) },
		                { 0x005, new CMwMethodInfo("ShowMainMenu", 0x00000000, null, null) },
		                { 0x006, new CMwMethodInfo("HideMainMenu", 0x00000000, null, null) },
		                { 0x007, new CMwFieldInfo("MainMenu", 0x00000005) },
		                { 0x008, new CMwFieldInfo("BasicDialogs", 0x00000005) },
		                { 0x009, new CMwFieldInfo("SystemOverlay", 0x00000005) },
		                { 0x00A, new CMwFieldInfo("ManialinkBrowser", 0x00000005) },
		                { 0x00B, new CMwFieldInfo("ActiveMenus", 0x00000007) },
		                { 0x00C, new CMwMethodInfo("ShowMenu", 0x00000041,
			                new uint[] { 0x03009000 },
			                new string[] { "Menu" })
		                },
		                { 0x00D, new CMwMethodInfo("HideMenu", 0x00000041,
			                new uint[] { 0x03009000 },
			                new string[] { "Menu" })
		                },
		                { 0x00E, new CMwMethodInfo("OpenMessenger", 0x00000000, null, null) },
		                { 0x00F, new CMwMethodInfo("OnGraphicSettings", 0x00000000, null, null) },
		                { 0x010, new CMwMethodInfo("UpdatePacksAvailabilityAndUse", 0x00000000, null, null) },
		                { 0x011, new CMwFieldInfo("NbPlayers", 0x0000001F) },
		                { 0x012, new CMwFieldInfo("Players", 0x00000007) },
		                { 0x013, new CMwFieldInfo("OSUTCDate", 0x00000029) },
		                { 0x014, new CMwFieldInfo("OSLocalDate", 0x00000029) },
		                { 0x015, new CMwFieldInfo("OSUTCTime", 0x00000029) },
		                { 0x016, new CMwFieldInfo("OSLocalTime", 0x00000029) },
		                { 0x017, new CMwFieldInfo("LoadProgress", 0x00000005) },
		                { 0x018, new CMwFieldInfo("EnvironmentManager", 0x00000005) },
		                { 0x019, new CMwFieldInfo("CmdLineUrlTmtp", 0x00000029) }
	                  })
	                },
	                { 0x006, new CMwClassInfo("CGameMasterServer", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("IsConnected", 0x00000001) },
		                { 0x001, new CMwFieldInfo("SubscribePath", 0x0000002D) },
		                { 0x002, new CMwFieldInfo("SubscribeWishedPath", 0x0000002D) },
		                { 0x003, new CMwFieldInfo("SubscribeNickName", 0x0000002D) },
		                { 0x004, new CMwFieldInfo("SubscribeNickNameNew", 0x0000002D) },
		                { 0x005, new CMwFieldInfo("SubscribeEMail", 0x00000029) },
		                { 0x006, new CMwFieldInfo("AliveUpdate_Needed", 0x00000001) },
		                { 0x007, new CMwFieldInfo("AliveUpdate_Count", 0x0000001F) },
		                { 0x008, new CMwFieldInfo("AliveUpdate_Last", 0x0000001F) },
		                { 0x009, new CMwFieldInfo("AliveUpdate_RefreshPeriodAsPlayer", 0x0000001F) },
		                { 0x00A, new CMwFieldInfo("AliveUpdate_MinRefreshPeriodAsServer", 0x0000001F) },
		                { 0x00B, new CMwFieldInfo("AliveUpdate_MaxRefreshPeriodAsServer", 0x0000001F) },
		                { 0x00C, new CMwFieldInfo("AliveTick_Count", 0x0000001F) },
		                { 0x00D, new CMwFieldInfo("AliveTick_Last", 0x0000001F) },
		                { 0x00E, new CMwFieldInfo("AliveTick_RefreshPeriodAsPlayer", 0x0000001F) },
		                { 0x00F, new CMwFieldInfo("AliveTick_RefreshPeriodAsServerForPlayers", 0x0000001F) },
		                { 0x010, new CMwMethodInfo("Subscribe", 0x00000000, null, null) },
		                { 0x011, new CMwMethodInfo("Connect", 0x00000000, null, null) },
		                { 0x012, new CMwMethodInfo("Disconnect", 0x00000000, null, null) },
		                { 0x013, new CMwMethodInfo("GetOnlineProfile", 0x00000000, null, null) },
		                { 0x014, new CMwMethodInfo("UpdateOnlineProfile", 0x00000000, null, null) },
		                { 0x015, new CMwMethodInfo("MailAccount", 0x00000000, null, null) },
		                { 0x016, new CMwMethodInfo("SendCrashLogs", 0x00000000, null, null) },
		                { 0x017, new CMwMethodInfo("SendGeneralCaps", 0x00000000, null, null) },
		                { 0x018, new CMwMethodInfo("SendGfxPerformance", 0x00000000, null, null) },
		                { 0x019, new CMwMethodInfo("SendMessages", 0x00000000, null, null) },
		                { 0x01A, new CMwMethodInfo("ReceiveMessages", 0x00000000, null, null) },
		                { 0x01B, new CMwFieldInfo("RefreshOnlineNewsDoReply", 0x0000001F) },
		                { 0x01C, new CMwFieldInfo("FilesToSubmit", 0x00000021) },
		                { 0x01D, new CMwFieldInfo("FilesToConfirm", 0x00000021) },
		                { 0x01E, new CMwFieldInfo("ReturnedError", 0x0000001F) },
		                { 0x01F, new CMwFieldInfo("ReturnedIP", 0x00000029) },
		                { 0x020, new CMwFieldInfo("InboxMessages", 0x00000007) },
		                { 0x021, new CMwFieldInfo("OutboxMessages", 0x00000007) },
		                { 0x022, new CMwFieldInfo("InboxMessagesCount", 0x0000001F) },
		                { 0x023, new CMwFieldInfo("OutboxMessagesCount", 0x0000001F) },
		                { 0x024, new CMwFieldInfo("Pools", 0x00000007) },
		                { 0x025, new CMwFieldInfo("RemoteDataInfos", 0x00000007) },
		                { 0x026, new CMwFieldInfo("Search_TimeToWait", 0x0000001F) },
		                { 0x027, new CMwFieldInfo("Search_Last", 0x0000001F) },
		                { 0x028, new CMwFieldInfo("GetRankings_TimeToWait", 0x0000001F) },
		                { 0x029, new CMwFieldInfo("GetRankings_Last", 0x0000001F) },
		                { 0x02A, new CMwFieldInfo("LeaguesManager", 0x00000005) },
		                { 0x02B, new CMwFieldInfo("SubscribedGroupsManager", 0x00000005) }
	                  })
	                },
	                { 0x007, new CMwClassInfo("CGameMobil", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("SceneMobil", 0x00000005) },
		                { 0x001, new CMwFieldInfo("GameMobilId", 0x0000001F) }
	                  })
	                },
	                { 0x008, new CMwClassInfo("CGameNod", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("Game", 0x00000005) }
	                  })
	                },
	                { 0x009, new CMwClassInfo("CGameMenu", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("Frames", 0x00000006) },
		                { 0x001, new CMwFieldInfo("MainFrame", 0x00000005) },
		                { 0x002, new CMwFieldInfo("CurrentFrame", 0x00000005) },
		                { 0x003, new CMwFieldInfo("CurrentFocusedControl", 0x00000005) },
		                { 0x004, new CMwFieldInfo("Music", 0x00000005) },
		                { 0x005, new CMwFieldInfo("StyleSheet", 0x00000005) },
		                { 0x006, new CMwFieldInfo("BackgroundBitmapUnderlay", 0x00000005) },
		                { 0x007, new CMwFieldInfo("BackgroundScene", 0x00000005) },
		                { 0x008, new CMwFieldInfo("BackgroundCamera", 0x00000005) },
		                { 0x009, new CMwFieldInfo("BackgroundCameraFov", 0x00000024) },
		                { 0x00A, new CMwFieldInfo("BackgroundCameraNearZ", 0x00000024) },
		                { 0x00B, new CMwFieldInfo("BackgroundCameraFarZ", 0x00000024) },
		                { 0x00C, new CMwFieldInfo("EnableFrameStack", 0x00000001) },
		                { 0x00D, new CMwMethodInfo("Back", 0x00000000, null, null) },
		                { 0x00E, new CMwMethodInfo("BackToMain", 0x00000000, null, null) },
		                { 0x00F, new CMwMethodInfo("RedrawAll", 0x00000000, null, null) },
		                { 0x010, new CMwFieldInfo("Game", 0x00000005) },
		                { 0x011, new CMwFieldInfo("FrustumCenter", 0x00000035) },
		                { 0x012, new CMwFieldInfo("FurstrumHfDiag", 0x00000035) },
		                { 0x013, new CMwFieldInfo("SortPriority", 0x0000000E) },
		                { 0x014, new CMwFieldInfo("Overlay", 0x00000005) },
		                { 0x015, new CMwFieldInfo("Scene3d", 0x00000005) },
		                { 0x016, new CMwFieldInfo("MenuCamera", 0x00000005) },
		                { 0x017, new CMwFieldInfo("IsMenu3d", 0x00000001) },
		                { 0x018, new CMwFieldInfo("Is3dFrame", 0x00000002) },
		                { 0x019, new CMwFieldInfo("FramesLocation", 0x00000014) },
		                { 0x01A, new CMwFieldInfo("CamerasLocation", 0x00000014) },
		                { 0x01B, new CMwFieldInfo("FovY", 0x00000024) },
		                { 0x01C, new CMwFieldInfo("NearZ", 0x00000024) },
		                { 0x01D, new CMwFieldInfo("FarZ", 0x00000024) },
		                { 0x01E, new CMwFieldInfo("GridStep", 0x00000031) }
	                  })
	                },
	                { 0x00A, new CMwClassInfo("CGameNetFormGameSync", new Dictionary<int, CMwMemberInfo>()
	                  {
	                  })
	                },
	                { 0x00B, new CMwClassInfo("CGameMenuFrame", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("AutoBackButtonId", 0x0000001B) },
		                { 0x001, new CMwFieldInfo("UpDownSound", 0x00000005) },
		                { 0x002, new CMwFieldInfo("ShowSound", 0x00000005) },
		                { 0x003, new CMwFieldInfo("OnHideScript", 0x00000005) },
		                { 0x004, new CMwFieldInfo("OnBeforeShowScript", 0x00000005) },
		                { 0x005, new CMwFieldInfo("OnShowScript", 0x00000005) },
		                { 0x006, new CMwFieldInfo("Menu", 0x00000005) },
		                { 0x007, new CMwFieldInfo("FrameScene", 0x00000005) },
		                { 0x008, new CMwMethodInfo("RunOnShowScripts", 0x00000000, null, null) },
		                { 0x009, new CMwFieldInfo("AllowBgCamera", 0x00000001) }
	                  })
	                },
	                { 0x00C, new CMwClassInfo("CGameSystemOverlay", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("Overlay", 0x00000005) },
		                { 0x001, new CMwFieldInfo("InactivityDelay", 0x0000001F) },
		                { 0x002, new CMwFieldInfo("ShowDelay", 0x0000001F) },
		                { 0x003, new CMwFieldInfo("HideDelay", 0x0000001F) },
		                { 0x004, new CMwMethodInfo("GoWindowed", 0x00000000, null, null) },
		                { 0x005, new CMwMethodInfo("GoFullScreen", 0x00000000, null, null) },
		                { 0x006, new CMwFieldInfo("ToolBarIsActive", 0x00000001) },
		                { 0x007, new CMwMethodInfo("ToolBarActivate", 0x00000000, null, null) },
		                { 0x008, new CMwFieldInfo("ToolTip", 0x0000002D) }
	                  })
	                },
	                { 0x00D, new CMwClassInfo("CGamePlayground", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("Interface", 0x00000005) },
		                { 0x001, new CMwFieldInfo("Players", 0x00000007) },
		                { 0x002, new CMwFieldInfo("InDialogs", 0x00000001) }
	                  })
	                },
	                { 0x00E, new CMwClassInfo("CGameNetPlayerInfo", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("Name", 0x0000002D) },
		                { 0x001, new CMwFieldInfo("Login", 0x00000029) },
		                { 0x002, new CMwFieldInfo("Live_IsRegisteredToMasterServer", 0x00000001) },
		                { 0x003, new CMwFieldInfo("Live_UpdatingCount", 0x0000001F) },
		                { 0x004, new CMwFieldInfo("Live_UpdateLastTime", 0x0000001F) },
		                { 0x005, new CMwFieldInfo("LiveUpdate_Counter", 0x0000001F) },
		                { 0x006, new CMwEnumInfo("PlayerType", new string[] { "Human", "Computer", "Net", "Spectator" }) },
		                { 0x007, new CMwFieldInfo("PlayerUIdForParam", 0x0000001F) },
		                { 0x008, new CMwFieldInfo("State", 0x0000001F) },
		                { 0x009, new CMwFieldInfo("LatestNetUpdate", 0x0000001F) },
		                { 0x00A, new CMwFieldInfo("DownloadRate", 0x0000001F) },
		                { 0x00B, new CMwFieldInfo("UploadRate", 0x0000001F) },
		                { 0x00C, new CMwFieldInfo("CustomDataDeactivated", 0x00000001) },
		                { 0x00D, new CMwFieldInfo("NbSpectators", 0x0000001F) },
		                { 0x00E, new CMwEnumInfo("SpectatorMode", new string[] { "Void", "Watcher", "LocalWatcher", "Target" }) },
		                { 0x00F, new CMwFieldInfo("Language", 0x00000029) },
		                { 0x010, new CMwEnumInfo("OnlineRights", new string[] { "None", "Accepted", "Approved", "Buyer" }) }
	                  })
	                },
	                { 0x00F, new CMwClassInfo("CGameNetwork", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("ServerInfo", 0x00000005) },
		                { 0x001, new CMwFieldInfo("MasterServer", 0x00000005) },
		                { 0x002, new CMwFieldInfo("FileTransfer", 0x00000005) },
		                { 0x003, new CMwFieldInfo("PackDescs", 0x00000007) },
		                { 0x004, new CMwFieldInfo("OnlineServers", 0x00000007) },
		                { 0x005, new CMwFieldInfo("OnlinePlayers", 0x00000007) },
		                { 0x006, new CMwFieldInfo("PlayerInfos", 0x00000007) },
		                { 0x007, new CMwFieldInfo("PlayingPlayers", 0x00000007) },
		                { 0x008, new CMwFieldInfo("VoteDefaultRatio", 0x00000024) },
		                { 0x009, new CMwFieldInfo("CallVoteTimeOut", 0x0000001F) },
		                { 0x00A, new CMwFieldInfo("CallVotePercent", 0x0000001F) },
		                { 0x00B, new CMwFieldInfo("InCallvote", 0x00000001) },
		                { 0x00C, new CMwFieldInfo("VoteNbYes", 0x0000001F) },
		                { 0x00D, new CMwFieldInfo("VoteNbNo", 0x0000001F) },
		                { 0x00E, new CMwMethodInfo("FindServers", 0x00000000, null, null) },
		                { 0x00F, new CMwFieldInfo("RecvNetRate", 0x0000001F) },
		                { 0x010, new CMwFieldInfo("SendNetRate", 0x0000001F) },
		                { 0x011, new CMwFieldInfo("PacketLossRate", 0x00000024) },
		                { 0x012, new CMwFieldInfo("LatestPing", 0x0000001F) },
		                { 0x013, new CMwFieldInfo("LatestEpsilon", 0x00000024) },
		                { 0x014, new CMwFieldInfo("SmoothedEpsilon", 0x00000024) },
		                { 0x015, new CMwFieldInfo("TotalSendingSize", 0x0000001F) },
		                { 0x016, new CMwFieldInfo("TotalReceivingSize", 0x0000001F) },
		                { 0x017, new CMwFieldInfo("TotalHttpReceivingSize", 0x0000001F) },
		                { 0x018, new CMwFieldInfo("TotalTcpUdpReceivingSize", 0x0000001F) },
		                { 0x019, new CMwFieldInfo("TcpReceivingSize", 0x0000001F) },
		                { 0x01A, new CMwFieldInfo("UdpReceivingSize", 0x0000001F) },
		                { 0x01B, new CMwFieldInfo("TcpSendingSize", 0x0000001F) },
		                { 0x01C, new CMwFieldInfo("UdpSendingSize", 0x0000001F) },
		                { 0x01D, new CMwFieldInfo("NbrTotalConnection", 0x0000001F) },
		                { 0x01E, new CMwFieldInfo("NbrConnectionsDone", 0x0000001F) },
		                { 0x01F, new CMwFieldInfo("NbrConnectionsDisconnecting", 0x0000001F) },
		                { 0x020, new CMwFieldInfo("NbrConnectionsInProgress", 0x0000001F) },
		                { 0x021, new CMwFieldInfo("NbrConnectionsPending", 0x0000001F) },
		                { 0x022, new CMwFieldInfo("NbrAcceptPerSecond", 0x0000001F) },
		                { 0x023, new CMwFieldInfo("NbrNewConnectionPerSecond", 0x0000001F) },
		                { 0x024, new CMwFieldInfo("IsInternet", 0x00000001) },
		                { 0x025, new CMwFieldInfo("IsEnabled", 0x00000001) },
		                { 0x026, new CMwFieldInfo("IsServer", 0x00000001) },
		                { 0x027, new CMwFieldInfo("Server", 0x00000005) },
		                { 0x028, new CMwFieldInfo("Client", 0x00000005) },
		                { 0x029, new CMwFieldInfo("ChatHistoryText", 0x0000002F) },
		                { 0x02A, new CMwFieldInfo("ChatHistoryUid", 0x00000021) },
		                { 0x02B, new CMwFieldInfo("ChatHistoryLines", 0x0000002F) }
	                  })
	                },
	                { 0x010, new CMwClassInfo("CGameNetFormTunnel", new Dictionary<int, CMwMemberInfo>()
	                  {
	                  })
	                },
	                { 0x012, new CMwClassInfo("CGameControlPlayerNet", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("Bidon", 0x00000024) }
	                  })
	                },
	                { 0x013, new CMwClassInfo("CGameControlPlayerInput", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("Keyboard", 0x00000005) }
	                  })
	                },
	                { 0x015, new CMwClassInfo("CGameBulletModel", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("LinesMaterial", 0x00000005) },
		                { 0x001, new CMwFieldInfo("BallsMaterial", 0x00000005) },
		                { 0x002, new CMwFieldInfo("ColorHead", 0x00000009) },
		                { 0x003, new CMwFieldInfo("ColorTail", 0x00000009) },
		                { 0x004, new CMwFieldInfo("DiameterHead", 0x00000024) },
		                { 0x005, new CMwFieldInfo("DiameterTail", 0x00000024) },
		                { 0x006, new CMwFieldInfo("TexCoordHead", 0x00000024) },
		                { 0x007, new CMwFieldInfo("TexCoordTail", 0x00000024) },
		                { 0x008, new CMwFieldInfo("Speed", 0x00000024) },
		                { 0x009, new CMwFieldInfo("LifeTime", 0x00000024) },
		                { 0x00A, new CMwFieldInfo("Mass", 0x00000024) },
		                { 0x00B, new CMwFieldInfo("Damage", 0x00000024) },
		                { 0x00C, new CMwFieldInfo("RayLength", 0x00000024) },
		                { 0x00D, new CMwFieldInfo("Radius", 0x00000024) },
		                { 0x00E, new CMwFieldInfo("FirePeriod", 0x0000001F) },
		                { 0x00F, new CMwFieldInfo("ColorSplashBuild", 0x00000009) },
		                { 0x010, new CMwFieldInfo("ColorSplashVictim", 0x00000009) },
		                { 0x011, new CMwFieldInfo("SizeSplashBuild", 0x00000024) },
		                { 0x012, new CMwFieldInfo("SizeSplashVictim", 0x00000024) },
		                { 0x013, new CMwFieldInfo("SoundShoot", 0x00000005) },
		                { 0x014, new CMwFieldInfo("SoundHit", 0x00000005) },
		                { 0x015, new CMwFieldInfo("SoundFly", 0x00000005) }
	                  })
	                },
	                { 0x017, new CMwClassInfo("CGameManialinkEntry", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("Value", 0x0000002D) }
	                  })
	                },
	                { 0x018, new CMwClassInfo("CGameScene", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("GameMobils", 0x00000007) },
		                { 0x001, new CMwFieldInfo("Scene", 0x00000005) }
	                  })
	                },
	                { 0x019, new CMwClassInfo("CGameMenuColorEffect", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("ColorsBeam", 0x00000006) },
		                { 0x001, new CMwFieldInfo("ColorPeriods", 0x00000025) },
		                { 0x002, new CMwFieldInfo("BeamWidths", 0x00000025) },
		                { 0x003, new CMwFieldInfo("InterWidths", 0x00000025) },
		                { 0x004, new CMwFieldInfo("Period", 0x00000024) },
		                { 0x005, new CMwFieldInfo("IsBeamColorEvolving", 0x00000001) },
		                { 0x006, new CMwFieldInfo("IsBeamMoving", 0x00000001) },
		                { 0x007, new CMwFieldInfo("IsMoveHalf", 0x00000001) },
		                { 0x008, new CMwFieldInfo("IsMoveInverse", 0x00000001) },
		                { 0x009, new CMwFieldInfo("IsColorEvolveHalf", 0x00000001) },
		                { 0x00A, new CMwFieldInfo("IsColorEvolveInverse", 0x00000001) },
		                { 0x00B, new CMwFieldInfo("ForceFirstColorWord", 0x00000001) },
		                { 0x00C, new CMwFieldInfo("ForceNoMove", 0x00000001) }
	                  })
	                },
	                { 0x01A, new CMwClassInfo("CGameCtnCollector", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("Collection", 0x00000029) },
		                { 0x001, new CMwFieldInfo("Author", 0x00000029) },
		                { 0x002, new CMwFieldInfo("IsInternal", 0x00000001) },
		                { 0x003, new CMwFieldInfo("ParentCollectorId", 0x00000029) },
		                { 0x004, new CMwFieldInfo("PageName", 0x00000029) },
		                { 0x005, new CMwFieldInfo("CatalogPosition", 0x0000001F) },
		                { 0x006, new CMwFieldInfo("IconFid", 0x00000005) }
	                  })
	                },
	                { 0x01B, new CMwClassInfo("CGameCtnCollectorList", new Dictionary<int, CMwMemberInfo>()
	                  {
	                  })
	                },
	                { 0x01C, new CMwClassInfo("CGameCtnCollectorVehicle", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("Vehicle", 0x00000005) },
		                { 0x001, new CMwFieldInfo("RaceInterfaceFid", 0x00000005) },
		                { 0x002, new CMwFieldInfo("LowQualitySolid", 0x00000005) },
		                { 0x003, new CMwFieldInfo("Cameras", 0x00000007) },
		                { 0x004, new CMwFieldInfo("DefaultCamIndex", 0x0000001F) },
		                { 0x005, new CMwFieldInfo("MaterialSkin", 0x00000005) },
		                { 0x006, new CMwFieldInfo("MaterialGlass", 0x00000005) },
		                { 0x007, new CMwFieldInfo("MaterialDetails", 0x00000005) },
		                { 0x008, new CMwFieldInfo("MaterialPilot", 0x00000005) },
		                { 0x009, new CMwFieldInfo("FrontLight", 0x00000005) },
		                { 0x00A, new CMwFieldInfo("FrontLightSmall", 0x00000005) },
		                { 0x00B, new CMwFieldInfo("RearLight", 0x00000005) },
		                { 0x00C, new CMwFieldInfo("ProjShadow", 0x00000005) },
		                { 0x00D, new CMwFieldInfo("ProjFront", 0x00000005) },
		                { 0x00E, new CMwFieldInfo("NadeoSkinsFids", 0x00000006) },
		                { 0x00F, new CMwFieldInfo("ForcedSkinsFids", 0x00000007) },
		                { 0x010, new CMwFieldInfo("DecoratorSolid", 0x00000005) },
		                { 0x011, new CMwFieldInfo("StemMaterial", 0x00000005) },
		                { 0x012, new CMwFieldInfo("StemBumpMaterial", 0x00000005) },
		                { 0x013, new CMwFieldInfo("GroundPoint", 0x00000035) },
		                { 0x014, new CMwFieldInfo("PainterGroundMargin", 0x00000024) },
		                { 0x015, new CMwFieldInfo("OrbitalCenterHeightFromGround", 0x00000024) },
		                { 0x016, new CMwFieldInfo("OrbitalRadiusBase", 0x00000024) },
		                { 0x017, new CMwFieldInfo("OrbitalPreviewAngle", 0x00000024) },
		                { 0x018, new CMwFieldInfo("BannerProfileFid", 0x00000005) },
		                { 0x019, new CMwFieldInfo("AudioEnvironmentInCar", 0x00000005) }
	                  })
	                },
	                { 0x01D, new CMwClassInfo("CGameCtnChapter", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("CollectionFid", 0x00000005) },
		                { 0x001, new CMwFieldInfo("Desc", 0x00000029) },
		                { 0x002, new CMwFieldInfo("LongDesc", 0x0000002D) },
		                { 0x003, new CMwFieldInfo("Articles", 0x00000007) },
		                { 0x004, new CMwFieldInfo("MapCoordElem", 0x00000031) },
		                { 0x005, new CMwFieldInfo("MapCoordIcon", 0x00000031) },
		                { 0x006, new CMwFieldInfo("MapCoordDesc", 0x00000031) },
		                { 0x007, new CMwFieldInfo("Icon", 0x00000005) },
		                { 0x008, new CMwFieldInfo("Unlocked", 0x00000001) }
	                  })
	                },
	                { 0x01E, new CMwClassInfo("CGameCtnCatalog", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("Chapters", 0x00000007) }
	                  })
	                },
	                { 0x01F, new CMwClassInfo("CGameCtnArticle", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("CollectorFid", 0x00000005) },
		                { 0x001, new CMwFieldInfo("IsLoaded", 0x00000001) },
		                { 0x002, new CMwMethodInfo("Preload", 0x00000000, null, null) },
		                { 0x003, new CMwMethodInfo("Purge", 0x00000000, null, null) },
		                { 0x004, new CMwFieldInfo("IdentId", 0x00000029) },
		                { 0x005, new CMwFieldInfo("IdentCollection", 0x00000029) },
		                { 0x006, new CMwFieldInfo("IdentAuthor", 0x00000029) },
		                { 0x007, new CMwFieldInfo("BitmapIcon", 0x00000005) },
		                { 0x008, new CMwFieldInfo("GameSkin", 0x00000005) },
		                { 0x009, new CMwFieldInfo("SkinPackDescs", 0x00000007) },
		                { 0x00A, new CMwFieldInfo("CurrentSkin", 0x0000001F) },
		                { 0x00B, new CMwMethodInfo("RefreshBitmap", 0x00000000, null, null) },
		                { 0x00C, new CMwFieldInfo("NbAvailableCurrent", 0x0000001F) },
		                { 0x00D, new CMwFieldInfo("NbAvailableMax", 0x0000001F) },
		                { 0x00E, new CMwFieldInfo("PageName", 0x00000029) }
	                  })
	                },
	                { 0x020, new CMwClassInfo("CGameNetOnlineEvent", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("Id", 0x0000001F) },
		                { 0x001, new CMwFieldInfo("Priority", 0x0000001F) },
		                { 0x002, new CMwFieldInfo("StartDate", 0x00000029) },
		                { 0x003, new CMwFieldInfo("EndDate", 0x00000029) },
		                { 0x004, new CMwFieldInfo("Path", 0x0000002D) },
		                { 0x005, new CMwFieldInfo("SenderName", 0x0000002D) },
		                { 0x006, new CMwFieldInfo("Title", 0x0000002D) },
		                { 0x007, new CMwFieldInfo("Content", 0x0000002D) },
		                { 0x008, new CMwFieldInfo("ManiaCodeText", 0x0000002D) },
		                { 0x009, new CMwFieldInfo("ManiaCode", 0x00000029) }
	                  })
	                },
	                { 0x022, new CMwClassInfo("CGameNetOnlineNews", new Dictionary<int, CMwMemberInfo>()
	                  {
	                  })
	                },
	                { 0x023, new CMwClassInfo("CGameCamera", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("SceneCamera", 0x00000005) }
	                  })
	                },
	                { 0x024, new CMwClassInfo("CGameCtnMediaBlock3dStereo", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("Separation", 0x00000024) },
		                { 0x001, new CMwFieldInfo("ScreenDist", 0x00000024) }
	                  })
	                },
	                { 0x025, new CMwClassInfo("CGameNetTeamInfo", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("Path", 0x0000002D) },
		                { 0x001, new CMwFieldInfo("Name", 0x0000002D) },
		                { 0x002, new CMwFieldInfo("Description", 0x0000002D) },
		                { 0x003, new CMwFieldInfo("Logo", 0x00000005) },
		                { 0x004, new CMwFieldInfo("MembersCount", 0x0000001F) },
		                { 0x005, new CMwFieldInfo("Wins", 0x0000001F) },
		                { 0x006, new CMwFieldInfo("Losses", 0x0000001F) },
		                { 0x007, new CMwFieldInfo("Draws", 0x0000001F) },
		                { 0x008, new CMwFieldInfo("Ranking", 0x0000001F) },
		                { 0x009, new CMwFieldInfo("MembersLogin", 0x0000002B) },
		                { 0x00A, new CMwFieldInfo("MembersNickName", 0x0000002F) }
	                  })
	                },
	                { 0x027, new CMwClassInfo("CGameAvatar", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("PackDesc", 0x00000005) }
	                  })
	                },
	                { 0x028, new CMwClassInfo("CGameNetOnlineMessage", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("ReceiverLogin", 0x00000029) },
		                { 0x001, new CMwFieldInfo("SenderLogin", 0x00000029) },
		                { 0x002, new CMwFieldInfo("Subject", 0x0000002D) },
		                { 0x003, new CMwFieldInfo("Message", 0x0000002D) },
		                { 0x004, new CMwFieldInfo("Donation", 0x0000001F) }
	                  })
	                },
	                { 0x029, new CMwClassInfo("CGameCtnMediaBlockTriangles", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("Mobil", 0x00000005) }
	                  })
	                },
	                { 0x02A, new CMwClassInfo("CGameRemoteBuffer", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("LocalData", 0x00000005) },
		                { 0x001, new CMwFieldInfo("LastServerUpdate", 0x00000029) },
		                { 0x002, new CMwFieldInfo("UseRefs", 0x00000001) },
		                { 0x003, new CMwEnumInfo("Mode", new string[] { "None", "Get", "Set", "Get/Set" }) },
		                { 0x004, new CMwFieldInfo("TotalCount", 0x0000001F) },
		                { 0x005, new CMwFieldInfo("SpecificCount", 0x0000001F) },
		                { 0x006, new CMwFieldInfo("PerPageCount", 0x0000001F) },
		                { 0x007, new CMwFieldInfo("CacheDuration", 0x0000001F) },
		                { 0x008, new CMwFieldInfo("RegisteredUsersCount", 0x0000001F) },
		                { 0x009, new CMwFieldInfo("Datas", 0x00000007) }
	                  })
	                },
	                { 0x02B, new CMwClassInfo("CGameRemoteBufferPool", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("DataInfo", 0x00000005) },
		                { 0x001, new CMwFieldInfo("BuffersCount", 0x0000001F) }
	                  })
	                },
	                { 0x02C, new CMwClassInfo("CGameRemoteBufferDataInfo", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("Default_CacheDuration", 0x0000001F) },
		                { 0x001, new CMwFieldInfo("Default_PerPageCount", 0x0000001F) }
	                  })
	                },
	                { 0x02D, new CMwClassInfo("CGameInterface", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("Sounds", 0x00000006) },
		                { 0x001, new CMwFieldInfo("Musics", 0x00000006) },
		                { 0x002, new CMwFieldInfo("AudioSounds", 0x00000006) },
		                { 0x003, new CMwFieldInfo("HymnsFolder", 0x00000005) },
		                { 0x004, new CMwFieldInfo("MenuBackgroundsFolder", 0x00000005) },
		                { 0x005, new CMwFieldInfo("FolderGlobalEnvBanners", 0x00000005) },
		                { 0x006, new CMwFieldInfo("InterfaceBg", 0x00000005) },
		                { 0x007, new CMwFieldInfo("ShaderActiveAvailable", 0x00000005) },
		                { 0x008, new CMwFieldInfo("ShaderInactiveAvailable", 0x00000005) },
		                { 0x009, new CMwFieldInfo("ShaderActiveUnavailable", 0x00000005) },
		                { 0x00A, new CMwFieldInfo("ShaderInactiveUnavailable", 0x00000005) },
		                { 0x00B, new CMwFieldInfo("ArticleStyle", 0x00000005) },
		                { 0x00C, new CMwFieldInfo("ArticleIconStyle", 0x00000005) },
		                { 0x00D, new CMwFieldInfo("DirectoryIconFid", 0x00000005) },
		                { 0x00E, new CMwFieldInfo("StyleSheetFid", 0x00000005) },
		                { 0x00F, new CMwFieldInfo("InterfaceFont", 0x00000005) },
		                { 0x010, new CMwFieldInfo("ReplayInterfaceFid", 0x00000005) },
		                { 0x011, new CMwFieldInfo("InterfaceEditorFid", 0x00000005) },
		                { 0x012, new CMwFieldInfo("InterfacePuzzleFid", 0x00000005) },
		                { 0x013, new CMwFieldInfo("InterfaceEditorSimpleFid", 0x00000005) },
		                { 0x014, new CMwFieldInfo("RaceInterfaceFid", 0x00000005) },
		                { 0x015, new CMwFieldInfo("RaceNetInterfaceFid", 0x00000005) },
		                { 0x016, new CMwFieldInfo("MenusFid", 0x00000005) },
		                { 0x017, new CMwFieldInfo("DialogsFid", 0x00000005) },
		                { 0x018, new CMwFieldInfo("BasicDialogsFid", 0x00000005) },
		                { 0x019, new CMwFieldInfo("SystemDialogsFid", 0x00000005) },
		                { 0x01A, new CMwFieldInfo("ManialinkBrowserFid", 0x00000005) },
		                { 0x01B, new CMwFieldInfo("ProgressOverlayFid", 0x00000005) },
		                { 0x01C, new CMwFieldInfo("SystemOverlayFid", 0x00000005) },
		                { 0x01D, new CMwFieldInfo("MenusRemapFolder1", 0x00000005) },
		                { 0x01E, new CMwFieldInfo("MenusRemapFolder2", 0x00000005) },
		                { 0x01F, new CMwFieldInfo("MenusRemapping", 0x00000005) },
		                { 0x020, new CMwFieldInfo("DefaultAvatarBitmapFid", 0x00000005) },
		                { 0x021, new CMwFieldInfo("DefaultMTBlockBitmapFid", 0x00000005) },
		                { 0x022, new CMwFieldInfo("DefaultMTBlockSoundFid", 0x00000005) },
		                { 0x023, new CMwFieldInfo("DefaultLeagueLogoBitmapFid", 0x00000005) },
		                { 0x024, new CMwFieldInfo("DefaultChampionshipLogoBitmapFid", 0x00000005) },
		                { 0x025, new CMwFieldInfo("DefaultTournamentLogoBitmapFid", 0x00000005) },
		                { 0x026, new CMwFieldInfo("DefaultOnlineNewsIconBitmapFid", 0x00000005) },
		                { 0x027, new CMwFieldInfo("DefaultTeamLogoBitmapFid", 0x00000005) },
		                { 0x028, new CMwFieldInfo("DefaultTagBitmapFid", 0x00000005) },
		                { 0x029, new CMwFieldInfo("CreditsMusicFid", 0x00000005) },
		                { 0x02A, new CMwFieldInfo("EmptyChallengeCustomMusicFid", 0x00000005) },
		                { 0x02B, new CMwFieldInfo("PainterSetting", 0x00000005) },
		                { 0x02C, new CMwFieldInfo("UnknownFlagBitmap", 0x00000005) },
		                { 0x02D, new CMwFieldInfo("MediaTrackerInterfaceFid", 0x00000005) },
		                { 0x02E, new CMwFieldInfo("GeneralCardManager", 0x00000005) },
		                { 0x02F, new CMwFieldInfo("SceneFxNodRoot", 0x00000005) },
		                { 0x030, new CMwFieldInfo("PodiumScene", 0x00000005) },
		                { 0x031, new CMwFieldInfo("DefaultSkillScoreComputerFid", 0x00000005) },
		                { 0x032, new CMwFieldInfo("ImageTurboRoulette", 0x00000005) }
	                  })
	                },
	                { 0x02E, new CMwClassInfo("CGameNetServerInfo", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("IsIdle", 0x00000001) },
		                { 0x001, new CMwFieldInfo("IsOnline", 0x00000001) },
		                { 0x002, new CMwFieldInfo("IsServer", 0x00000001) },
		                { 0x003, new CMwFieldInfo("IsPrivate", 0x00000001) },
		                { 0x004, new CMwFieldInfo("IsPrivateForSpectator", 0x00000001) },
		                { 0x005, new CMwFieldInfo("AcceptReferees", 0x00000001) },
		                { 0x006, new CMwFieldInfo("RefereesCount", 0x0000001F) },
		                { 0x007, new CMwFieldInfo("LadderMatchId", 0x0000001F) },
		                { 0x008, new CMwFieldInfo("DownloadRate", 0x0000001F) },
		                { 0x009, new CMwFieldInfo("UploadRate", 0x0000001F) },
		                { 0x00A, new CMwEnumInfo("PingEnum", new string[] { "    ", "*   ", "**  ", "*** ", "****" }) },
		                { 0x00B, new CMwFieldInfo("Ping", 0x0000001F) },
		                { 0x00C, new CMwFieldInfo("ServerHostName", 0x00000029) },
		                { 0x00D, new CMwFieldInfo("State", 0x0000001F) },
		                { 0x00E, new CMwFieldInfo("QuickInfoReceived", 0x00000001) },
		                { 0x00F, new CMwFieldInfo("RoundTrip", 0x0000001F) },
		                { 0x010, new CMwFieldInfo("CallVoteEnabled", 0x00000001) },
		                { 0x011, new CMwFieldInfo("AdvertisingSuffix", 0x00000029) }
	                  })
	                },
	                { 0x02F, new CMwClassInfo("CGameNetForm", new Dictionary<int, CMwMemberInfo>()
	                  {
	                  })
	                },
	                { 0x030, new CMwClassInfo("CGameDialogs", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("Dialogs", 0x00000005) },
		                { 0x001, new CMwMethodInfo("HideDialogs", 0x00000000, null, null) },
		                { 0x002, new CMwMethodInfo("DoMessage_Ok", 0x00000000, null, null) },
		                { 0x003, new CMwMethodInfo("AskYesNo_No", 0x00000000, null, null) },
		                { 0x004, new CMwMethodInfo("AskYesNo_Yes", 0x00000000, null, null) },
		                { 0x005, new CMwFieldInfo("String", 0x0000002D) },
		                { 0x006, new CMwMethodInfo("DialogSaveAs_HierarchyUp", 0x00000000, null, null) },
		                { 0x007, new CMwFieldInfo("DialogSaveAs_Path", 0x0000002D) },
		                { 0x008, new CMwMethodInfo("DialogSaveAs_OnSelect", 0x00000000, null, null) },
		                { 0x009, new CMwMethodInfo("DialogSaveAs_OnValidate", 0x00000000, null, null) },
		                { 0x00A, new CMwMethodInfo("DialogSaveAs_OnCancel", 0x00000000, null, null) },
		                { 0x00B, new CMwFieldInfo("DialogSaveAs_Files", 0x00000007) },
		                { 0x00C, new CMwFieldInfo("Progress", 0x00000028) }
	                  })
	                },
	                { 0x031, new CMwClassInfo("CGameSkin", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("DirName", 0x00000029) },
		                { 0x001, new CMwFieldInfo("DirNameAlt", 0x00000029) },
		                { 0x002, new CMwFieldInfo("PainterTextureName", 0x00000029) },
		                { 0x003, new CMwFieldInfo("PainterSceneId", 0x00000029) },
		                { 0x004, new CMwFieldInfo("CustomizableFids", 0x00000007) },
		                { 0x005, new CMwFieldInfo("CustomizableNames", 0x0000002B) },
		                { 0x006, new CMwFieldInfo("CustomizableClassIds", 0x00000021) },
		                { 0x007, new CMwFieldInfo("CustomizableMipMaps", 0x00000003) }
	                  })
	                },
	                { 0x032, new CMwClassInfo("CGameMenuScaleEffect", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("LeftBorderScale", 0x00000024) },
		                { 0x001, new CMwFieldInfo("RightBorderScale", 0x00000024) },
		                { 0x002, new CMwFieldInfo("UpBorderScale", 0x00000024) },
		                { 0x003, new CMwFieldInfo("DownBorderScale", 0x00000024) },
		                { 0x004, new CMwFieldInfo("Shift", 0x00000024) },
		                { 0x005, new CMwFieldInfo("Period", 0x0000001F) },
		                { 0x006, new CMwFieldInfo("MaxLetterScaling", 0x0000001F) },
		                { 0x007, new CMwFieldInfo("AllowDecalage", 0x00000001) },
		                { 0x008, new CMwFieldInfo("AllowHideBeforeEffect", 0x00000001) },
		                { 0x009, new CMwFieldInfo("IsHalf", 0x00000001) },
		                { 0x00A, new CMwFieldInfo("IsInverse", 0x00000001) },
		                { 0x00B, new CMwFieldInfo("ReCenter", 0x00000001) }
	                  })
	                },
	                { 0x033, new CMwClassInfo("CGameCtnCollection", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("CompleteZoneList", 0x00000007) },
		                { 0x001, new CMwFieldInfo("ArticleList", 0x00000007) },
		                { 0x002, new CMwFieldInfo("DisplayName", 0x0000002D) },
		                { 0x003, new CMwFieldInfo("DefaultZone", 0x00000005) },
		                { 0x004, new CMwFieldInfo("SquareSize", 0x00000024) },
		                { 0x005, new CMwFieldInfo("SquareHeight", 0x00000024) },
		                { 0x006, new CMwFieldInfo("WaterTop", 0x00000024) },
		                { 0x007, new CMwFieldInfo("WaterBottom", 0x00000024) },
		                { 0x008, new CMwFieldInfo("IsWaterOutsidePlayField", 0x00000001) },
		                { 0x009, new CMwFieldInfo("IsWaterMultiHeight", 0x00000001) },
		                { 0x00A, new CMwFieldInfo("CameraMinHeight", 0x00000024) },
		                { 0x00B, new CMwFieldInfo("NeedUnlock", 0x00000001) },
		                { 0x00C, new CMwEnumInfo("BlocksShadow", new string[] { "None", "AllButLandscape", "AllButWaterOrUnderGnd", "All" }) },
		                { 0x00D, new CMwEnumInfo("BackgroundShadow", new string[] { "None", "Receive", "CastAndReceive" }) },
		                { 0x00E, new CMwFieldInfo("ShadowCastBack", 0x00000001) },
		                { 0x00F, new CMwFieldInfo("ShadowSoftSizeInWorld", 0x00000024) },
		                { 0x010, new CMwEnumInfo("VertexLighting", new string[] { "None", "Sunrise", "Nations" }) },
		                { 0x011, new CMwFieldInfo("ColorVertexMin", 0x00000028) },
		                { 0x012, new CMwFieldInfo("ColorVertexMax", 0x00000028) },
		                { 0x013, new CMwFieldInfo("CarCanBeDirty", 0x00000001) },
		                { 0x014, new CMwFieldInfo("DefaultDecoration", 0x00000005) },
		                { 0x015, new CMwFieldInfo("VehicleName", 0x00000029) },
		                { 0x016, new CMwFieldInfo("VehicleAuthor", 0x00000029) },
		                { 0x017, new CMwFieldInfo("VehicleCollection", 0x00000029) },
		                { 0x018, new CMwFieldInfo("IsEditable", 0x00000001) },
		                { 0x019, new CMwEnumInfo("CollectionType", new string[] { "Environment", "Vehicle" }) },
		                { 0x01A, new CMwFieldInfo("IconFid", 0x00000005) },
		                { 0x01B, new CMwFieldInfo("IconSmallFid", 0x00000005) },
		                { 0x01C, new CMwFieldInfo("LoadScreenFid", 0x00000005) },
		                { 0x01D, new CMwFieldInfo("SortIndex", 0x0000000E) },
		                { 0x01E, new CMwFieldInfo("FolderBlockInfo", 0x00000005) },
		                { 0x01F, new CMwFieldInfo("FolderObjectInfo", 0x00000005) },
		                { 0x020, new CMwFieldInfo("FolderDecoration", 0x00000005) },
		                { 0x021, new CMwFieldInfo("FolderMenusIcons", 0x00000005) },
		                { 0x022, new CMwFieldInfo("MapFid", 0x00000005) },
		                { 0x023, new CMwFieldInfo("MapRectMin", 0x00000031) },
		                { 0x024, new CMwFieldInfo("MapRectMax", 0x00000031) },
		                { 0x025, new CMwFieldInfo("MapCoordElem", 0x00000031) },
		                { 0x026, new CMwFieldInfo("MapCoordIcon", 0x00000031) },
		                { 0x027, new CMwFieldInfo("MapCoordDesc", 0x00000031) },
		                { 0x028, new CMwFieldInfo("LongDesc", 0x0000002D) },
		                { 0x029, new CMwMethodInfo("SetMapCoordFromRect", 0x00000000, null, null) },
		                { 0x02A, new CMwFieldInfo("BaseZoneStrings", 0x0000002F) },
		                { 0x02B, new CMwFieldInfo("ReplacementZoneStrings", 0x0000002F) },
		                { 0x02C, new CMwFieldInfo("ReplacementTerrainModifiers", 0x00000007) },
		                { 0x02D, new CMwMethodInfo("AddReplacementZone", 0x00000000, null, null) },
		                { 0x02E, new CMwMethodInfo("RemoveReplacementZone", 0x00000000, null, null) },
		                { 0x02F, new CMwFieldInfo("ParticleEmitterModelsFids", 0x00000007) }
	                  })
	                },
	                { 0x034, new CMwClassInfo("CGameCtnMediaBlockEditor", new Dictionary<int, CMwMemberInfo>()
	                  {
	                  })
	                },
	                { 0x035, new CMwClassInfo("CGameCtnObjectInfo", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("Helper", 0x00000005) },
		                { 0x001, new CMwFieldInfo("Mobil", 0x00000005) },
		                { 0x002, new CMwFieldInfo("AltMobil", 0x00000005) },
		                { 0x003, new CMwFieldInfo("Properties", 0x00000007) }
	                  })
	                },
	                { 0x036, new CMwClassInfo("CGameCtnBlockUnitInfo", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("Surface", 0x00000029) },
		                { 0x001, new CMwFieldInfo("OffsetX", 0x0000001F) },
		                { 0x002, new CMwFieldInfo("OffsetY", 0x0000001F) },
		                { 0x003, new CMwFieldInfo("OffsetZ", 0x0000001F) },
		                { 0x004, new CMwFieldInfo("Underground", 0x00000001) },
		                { 0x005, new CMwFieldInfo("AcceptPylons", 0x0000001F) },
		                { 0x006, new CMwFieldInfo("Pylons", 0x0000001F) },
		                { 0x007, new CMwEnumInfo("Frontier", new string[] { "DeadEnd", "Corner", "Straight", "TShaped", "Cross" }) },
		                { 0x008, new CMwEnumInfo("Dir", new string[] { "North", "East", "South", "West" }) },
		                { 0x009, new CMwFieldInfo("ReplacementBlockInfo", 0x00000005) },
		                { 0x00A, new CMwFieldInfo("ReplacementId", 0x00000029) },
		                { 0x00B, new CMwEnumInfo("MultiDir", new string[] { "Simple", "Symetrical", "All" }) },
		                { 0x00C, new CMwFieldInfo("TerrainModifierId", 0x00000029) },
		                { 0x00D, new CMwFieldInfo("Junctions", 0x00000006) }
	                  })
	                },
	                { 0x037, new CMwClassInfo("CGameFid", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("Fid", 0x00000005) },
		                { 0x001, new CMwFieldInfo("Fids", 0x00000005) },
		                { 0x002, new CMwFieldInfo("Name", 0x0000002D) },
		                { 0x003, new CMwFieldInfo("Path", 0x0000002D) },
		                { 0x004, new CMwFieldInfo("Selected", 0x00000001) },
		                { 0x005, new CMwFieldInfo("IsOnServer", 0x00000001) }
	                  })
	                },
	                { 0x038, new CMwClassInfo("CGameCtnDecoration", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("DecoSize", 0x00000005) },
		                { 0x001, new CMwFieldInfo("DecoAudio", 0x00000005) },
		                { 0x002, new CMwFieldInfo("DecoMood", 0x00000005) },
		                { 0x003, new CMwFieldInfo("TerrainModifierBase", 0x00000005) },
		                { 0x004, new CMwFieldInfo("TerrainModifierCovered", 0x00000005) },
		                { 0x005, new CMwMethodInfo("InitWithNoSkin", 0x00000000, null, null) },
		                { 0x006, new CMwMethodInfo("Reset", 0x00000000, null, null) },
		                { 0x007, new CMwFieldInfo("DecoratorSolidWarp", 0x00000005) }
	                  })
	                },
	                { 0x039, new CMwClassInfo("CGameCtnDecorationAudio", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("Sounds", 0x00000006) },
		                { 0x001, new CMwFieldInfo("Musics", 0x00000006) },
		                { 0x002, new CMwFieldInfo("SoundAttenuationInEditor", 0x00000028) },
		                { 0x003, new CMwFieldInfo("AudioEnvironmentOutside", 0x00000005) },
		                { 0x004, new CMwFieldInfo("AudioEnvironmentUnderground", 0x00000005) }
	                  })
	                },
	                { 0x03A, new CMwClassInfo("CGameCtnDecorationMood", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("ShadowCountCarHuman", 0x0000001F) },
		                { 0x001, new CMwFieldInfo("ShadowCountCarOpponent", 0x0000001F) },
		                { 0x002, new CMwFieldInfo("ShadowCarIntensity", 0x00000028) },
		                { 0x003, new CMwFieldInfo("ShadowScene", 0x00000001) },
		                { 0x004, new CMwFieldInfo("BackgroundIsLocallyLighted", 0x00000001) },
		                { 0x005, new CMwFieldInfo("SolidLightAreSkinned", 0x00000001) },
		                { 0x006, new CMwFieldInfo("Latitude", 0x00000024) },
		                { 0x007, new CMwFieldInfo("Longitude", 0x00000024) },
		                { 0x008, new CMwFieldInfo("DeltaGMT", 0x00000024) },
		                { 0x009, new CMwFieldInfo("TimeSunRise", 0x00000029) },
		                { 0x00A, new CMwFieldInfo("TimeSunFall", 0x00000029) },
		                { 0x00B, new CMwFieldInfo("RemappedStartDayTime", 0x00000028) },
		                { 0x00C, new CMwFieldInfo("IsNight", 0x00000001) },
		                { 0x00D, new CMwFieldInfo("Remapping", 0x00000005) },
		                { 0x00E, new CMwFieldInfo("RemapFolder", 0x00000005) },
		                { 0x00F, new CMwFieldInfo("HmsPackLightMap", 0x00000005) },
		                { 0x010, new CMwFieldInfo("HmsAmbientOcc", 0x00000005) }
	                  })
	                },
	                { 0x03B, new CMwClassInfo("CGameCtnDecorationSize", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("Scene", 0x00000005) },
		                { 0x001, new CMwFieldInfo("SizeX", 0x0000001F) },
		                { 0x002, new CMwFieldInfo("SizeY", 0x0000001F) },
		                { 0x003, new CMwFieldInfo("SizeZ", 0x0000001F) },
		                { 0x004, new CMwFieldInfo("BaseHeight", 0x0000001F) },
		                { 0x005, new CMwFieldInfo("EditionZoneMin", 0x00000031) },
		                { 0x006, new CMwFieldInfo("EditionZoneMax", 0x00000031) },
		                { 0x007, new CMwFieldInfo("VertexCount", 0x0000001F) }
	                  })
	                },
	                { 0x03C, new CMwClassInfo("CGameCtnDecorationTerrainModifier", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("IdName", 0x00000029) },
		                { 0x001, new CMwFieldInfo("Remapping", 0x00000005) },
		                { 0x002, new CMwFieldInfo("RemapFolder", 0x00000005) }
	                  })
	                },
	                { 0x03D, new CMwClassInfo("CGameAdvertising", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwEnumInfo("Mode", new string[] { "Environement", "Vehicle" }) },
		                { 0x001, new CMwFieldInfo("ImpressionHelpers_Enable", 0x00000001) },
		                { 0x002, new CMwFieldInfo("DisableOcclusion", 0x00000001) },
		                { 0x003, new CMwFieldInfo("EditionDummies", 0x00000001) },
		                { 0x004, new CMwFieldInfo("Files", 0x00000007) },
		                { 0x005, new CMwFieldInfo("EditonDummyImage", 0x00000005) },
		                { 0x006, new CMwFieldInfo("Identifier", 0x00000029) },
		                { 0x007, new CMwFieldInfo("PublicKey", 0x00000029) },
		                { 0x008, new CMwFieldInfo("Radial_Config", 0x00000029) },
		                { 0x009, new CMwFieldInfo("Impression_Time", 0x0000001F) },
		                { 0x00A, new CMwFieldInfo("Impression_Size", 0x00000024) },
		                { 0x00B, new CMwFieldInfo("Impression_CosAngle", 0x00000024) },
		                { 0x00C, new CMwFieldInfo("DefaultAdvertisingSuffix", 0x00000029) },
		                { 0x00D, new CMwMethodInfo("Init", 0x00000000, null, null) },
		                { 0x00E, new CMwMethodInfo("Destroy", 0x00000000, null, null) },
		                { 0x00F, new CMwMethodInfo("Flush", 0x00000000, null, null) },
		                { 0x010, new CMwMethodInfo("ImpressionHelpers_Update", 0x00000000, null, null) },
		                { 0x011, new CMwFieldInfo("ZoneName", 0x00000029) },
		                { 0x012, new CMwFieldInfo("ZoneAuthor", 0x00000029) },
		                { 0x013, new CMwFieldInfo("ZoneHost", 0x00000029) },
		                { 0x014, new CMwFieldInfo("Nation", 0x00000029) },
		                { 0x015, new CMwFieldInfo("Login", 0x00000029) },
		                { 0x016, new CMwFieldInfo("ZoneElements", 0x00000007) },
		                { 0x017, new CMwFieldInfo("ImpressionHelpers_Mobil", 0x00000005) }
	                  })
	                },
	                { 0x03E, new CMwClassInfo("CGameAdvertisingElement", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("Name", 0x00000029) },
		                { 0x001, new CMwFieldInfo("ContentDisplayed", 0x0000001F) },
		                { 0x002, new CMwFieldInfo("OwnerNods", 0x00000007) },
		                { 0x003, new CMwFieldInfo("File", 0x00000005) },
		                { 0x004, new CMwMethodInfo("DisplayOrig", 0x00000000, null, null) },
		                { 0x005, new CMwFieldInfo("Nadeo_MaxImpression", 0x00000024) }
	                  })
	                },
	                { 0x03F, new CMwClassInfo("CGameGhost", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("Duration", 0x0000001F) },
		                { 0x001, new CMwFieldInfo("Size", 0x0000001F) },
		                { 0x002, new CMwFieldInfo("RecordMobil", 0x00000005) },
		                { 0x003, new CMwFieldInfo("IsRecording", 0x00000001) },
		                { 0x004, new CMwFieldInfo("ReplayMobil", 0x00000005) },
		                { 0x005, new CMwFieldInfo("IsReplaying", 0x00000001) },
		                { 0x006, new CMwFieldInfo("SavedPeriod", 0x0000001F) }
	                  })
	                },
	                { 0x040, new CMwClassInfo("CGameControlCameraMaster", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("ManagedCams", 0x00000007) },
		                { 0x001, new CMwFieldInfo("GlobalEffects", 0x00000005) },
		                { 0x002, new CMwFieldInfo("CurrentCam", 0x0000001F) },
		                { 0x003, new CMwFieldInfo("IsSwitching", 0x00000001) },
		                { 0x004, new CMwFieldInfo("DefaultCameraId", 0x0000001B) }
	                  })
	                },
	                { 0x042, new CMwClassInfo("CGameControlCameraEffectAdaptativeNearZ", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("HeightMin", 0x00000024) },
		                { 0x001, new CMwFieldInfo("HeightMax", 0x00000024) },
		                { 0x002, new CMwFieldInfo("MinNearZ", 0x00000024) },
		                { 0x003, new CMwFieldInfo("MaxNearZ", 0x00000024) },
		                { 0x004, new CMwFieldInfo("NearZBlend", 0x00000024) },
		                { 0x005, new CMwFieldInfo("CurrentNearZ", 0x00000024) }
	                  })
	                },
	                { 0x043, new CMwClassInfo("CGameCtnChallenge", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("SizeX", 0x0000001F) },
		                { 0x001, new CMwFieldInfo("SizeY", 0x0000001F) },
		                { 0x002, new CMwFieldInfo("SizeZ", 0x0000001F) },
		                { 0x003, new CMwFieldInfo("ChallengeName", 0x0000002D) },
		                { 0x004, new CMwFieldInfo("Name", 0x00000029) },
		                { 0x005, new CMwFieldInfo("CollectionStr", 0x00000029) },
		                { 0x006, new CMwFieldInfo("Author", 0x00000029) },
		                { 0x007, new CMwFieldInfo("CopperPrice", 0x0000001F) },
		                { 0x008, new CMwFieldInfo("HashedPassword", 0x00000029) },
		                { 0x009, new CMwFieldInfo("NbLaps", 0x0000001F) },
		                { 0x00A, new CMwFieldInfo("VehicleName", 0x00000029) },
		                { 0x00B, new CMwFieldInfo("VehicleAuthor", 0x00000029) },
		                { 0x00C, new CMwFieldInfo("VehicleCollection", 0x00000029) },
		                { 0x00D, new CMwFieldInfo("NeedUnlock", 0x00000001) },
		                { 0x00E, new CMwEnumInfo("Kind", new string[] { "(internal)EndMarker", "(old)Campaign", "(old)Puzzle", "(old)Retro", "(old)TimeAttack", "(old)Rounds", "InProgress", "Campaign", "Custom", "Solo", "Site", "SoloNadeo", "MultiNadeo" }) },
		                { 0x00F, new CMwEnumInfo("PlayMode", new string[] { "Race", "Platform", "Puzzle", "Crazy(Obsolete)", "Shortcut(Obsolete)", "Stunts" }) },
		                { 0x010, new CMwEnumInfo("Difficulty", new string[] { "White", "Green", "Blue", "Red", "Black", "Custom" }) },
		                { 0x011, new CMwFieldInfo("Decoration", 0x00000005) },
		                { 0x012, new CMwFieldInfo("ModPackDesc", 0x00000005) },
		                { 0x013, new CMwFieldInfo("CustomMusicPackDesc", 0x00000005) },
		                { 0x014, new CMwFieldInfo("CustomMusic", 0x00000005) },
		                { 0x015, new CMwFieldInfo("Collection", 0x00000005) },
		                { 0x016, new CMwFieldInfo("CurrentBlockInfo", 0x00000005) },
		                { 0x017, new CMwFieldInfo("Blocks", 0x00000007) },
		                { 0x018, new CMwMethodInfo("CheckPlayField", 0x00000000, null, null) },
		                { 0x019, new CMwMethodInfo("ClearStock", 0x00000000, null, null) },
		                { 0x01A, new CMwFieldInfo("UndergroundBox", 0x00000005) },
		                { 0x01B, new CMwFieldInfo("SkinBox", 0x00000005) },
		                { 0x01C, new CMwFieldInfo("UndergroundMobil", 0x00000005) },
		                { 0x01D, new CMwFieldInfo("SkinMobil", 0x00000005) },
		                { 0x01E, new CMwFieldInfo("ChallengeParameters", 0x00000005) },
		                { 0x01F, new CMwFieldInfo("TargetTime", 0x0000001F) },
		                { 0x020, new CMwFieldInfo("IsBlockHelpers", 0x00000001) },
		                { 0x021, new CMwMethodInfo("ComputeCrc32", 0x00000041,
			                new uint[] { 0x01001000, 0x0000001F },
			                new string[] { "Nod", "Crc32" })
		                },
		                { 0x022, new CMwFieldInfo("ClipIntro", 0x00000005) },
		                { 0x023, new CMwFieldInfo("ClipGlobal", 0x00000005) },
		                { 0x024, new CMwFieldInfo("ClipGroupInGame", 0x00000005) },
		                { 0x025, new CMwFieldInfo("ClipGroupEndRace", 0x00000005) },
		                { 0x026, new CMwFieldInfo("MapCoordTarget", 0x00000031) },
		                { 0x027, new CMwFieldInfo("MapCoordOrigin", 0x00000031) },
		                { 0x028, new CMwFieldInfo("Comments", 0x0000002D) }
	                  })
	                },
	                { 0x044, new CMwClassInfo("CGameCtnChallengeInfo", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("Name", 0x0000002D) },
		                { 0x001, new CMwFieldInfo("CollectionId", 0x0000002D) },
		                { 0x002, new CMwFieldInfo("TmpCollectionIcon", 0x00000005) },
		                { 0x003, new CMwFieldInfo("Author", 0x00000029) },
		                { 0x004, new CMwFieldInfo("Comments", 0x0000002D) },
		                { 0x005, new CMwFieldInfo("MapCoordOrigin", 0x00000031) },
		                { 0x006, new CMwFieldInfo("MapCoordTarget", 0x00000031) },
		                { 0x007, new CMwFieldInfo("LapRace", 0x00000001) },
		                { 0x008, new CMwFieldInfo("CreatedWithEditorSimple", 0x00000001) },
		                { 0x009, new CMwEnumInfo("Medal", new string[] { "None", "Finished", "Bronze", "Silver", "Gold", "Nadeo" }) },
		                { 0x00A, new CMwEnumInfo("OfficialMedal", new string[] { "None", "Finished", "Bronze", "Silver", "Gold", "Nadeo" }) },
		                { 0x00B, new CMwFieldInfo("BestTime", 0x0000001F) },
		                { 0x00C, new CMwFieldInfo("OfficialBestRecord", 0x0000001F) },
		                { 0x00D, new CMwFieldInfo("GoldTime", 0x0000001F) },
		                { 0x00E, new CMwFieldInfo("CopperPrice", 0x0000001F) },
		                { 0x00F, new CMwFieldInfo("CopperString", 0x00000029) },
		                { 0x010, new CMwFieldInfo("OfficialSkillMedal", 0x0000001F) },
		                { 0x011, new CMwFieldInfo("SoloScores", 0x00000007) },
		                { 0x012, new CMwFieldInfo("GeneralOfficialScores", 0x00000005) },
		                { 0x013, new CMwEnumInfo("Kind", new string[] { "(internal)EndMarker", "(old)Campaign", "(old)Puzzle", "(old)Retro", "(old)TimeAttack", "(old)Rounds", "InProgress", "Campaign", "Multi", "Solo", "Site", "SoloNadeo", "MultiNadeo" }) },
		                { 0x014, new CMwEnumInfo("PlayMode", new string[] { "Race", "Platform", "Puzzle", "Crazy(Obsolete)", "Shortcut(Obsolete)", "Stunts" }) },
		                { 0x015, new CMwFieldInfo("Unlocked", 0x00000001) },
		                { 0x016, new CMwEnumInfo("Difficulty", new string[] { "White", "Green", "Blue", "Red", "Black", "Custom", "Unknown" }) },
		                { 0x017, new CMwMethodInfo("UnlockChallenge", 0x00000000, null, null) }
	                  })
	                },
	                { 0x045, new CMwClassInfo("CGameOutlineBox", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("IsShowQuads", 0x00000001) },
		                { 0x001, new CMwFieldInfo("IsShowLines", 0x00000001) },
		                { 0x002, new CMwFieldInfo("HardLinesColorCoef", 0x00000024) },
		                { 0x003, new CMwFieldInfo("SoftLinesColorCoef", 0x00000024) },
		                { 0x004, new CMwFieldInfo("BottomLinesDeltaY", 0x00000024) }
	                  })
	                },
	                { 0x046, new CMwClassInfo("CGameCtnParticleParam", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("ParticleModelId", 0x00000029) }
	                  })
	                },
	                { 0x047, new CMwClassInfo("CGameHighScore", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("Name", 0x0000002D) },
		                { 0x001, new CMwFieldInfo("Time", 0x0000001F) },
		                { 0x002, new CMwFieldInfo("Score", 0x0000002D) },
		                { 0x003, new CMwFieldInfo("Rank", 0x0000001F) },
		                { 0x004, new CMwFieldInfo("Count", 0x0000001F) }
	                  })
	                },
	                { 0x048, new CMwClassInfo("CGameCtnPainterSetting", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("MinMaxScaleFill", 0x00000031) },
		                { 0x001, new CMwFieldInfo("MinMaxScaleSticker", 0x00000031) },
		                { 0x002, new CMwFieldInfo("MinMaxScaleStickerText", 0x00000031) },
		                { 0x003, new CMwFieldInfo("MinMaxScaleBrush", 0x00000031) },
		                { 0x004, new CMwFieldInfo("MinMaxScaleBrushText", 0x00000031) },
		                { 0x005, new CMwFieldInfo("Camera", 0x00000005) },
		                { 0x006, new CMwMethodInfo("SetDefaultCameraSettings", 0x00000000, null, null) },
		                { 0x007, new CMwFieldInfo("FidMaterialPaint", 0x00000005) },
		                { 0x008, new CMwFieldInfo("FidBitmapBrushFade", 0x00000005) },
		                { 0x009, new CMwFieldInfo("FidBitmapStickerFade", 0x00000005) },
		                { 0x00A, new CMwFieldInfo("FidMaterialBlendLayer", 0x00000005) },
		                { 0x00B, new CMwFieldInfo("FidImageSubObjectAllIcon", 0x00000005) },
		                { 0x00C, new CMwFieldInfo("ScenesFids", 0x00000007) },
		                { 0x00D, new CMwFieldInfo("MouseZDeltaRot", 0x00000024) },
		                { 0x00E, new CMwFieldInfo("MouseZDeltaScale", 0x00000024) },
		                { 0x00F, new CMwFieldInfo("CameraBottomClipGeometry", 0x00000001) },
		                { 0x010, new CMwFieldInfo("CameraBottomIn_m1p1", 0x00000024) },
		                { 0x011, new CMwFieldInfo("Remapping", 0x00000005) },
		                { 0x012, new CMwFieldInfo("RemapFolder", 0x00000005) }
	                  })
	                },
	                { 0x049, new CMwClassInfo("CGameLeagueManager", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("CacheDuration", 0x0000001F) },
		                { 0x001, new CMwFieldInfo("Leagues", 0x00000007) }
	                  })
	                },
	                { 0x04A, new CMwClassInfo("CGameCtnMediaBlockEditorTriangles", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwMethodInfo("ModeMoveVertexs", 0x00000000, null, null) },
		                { 0x001, new CMwMethodInfo("ModeCreateTriangles", 0x00000000, null, null) },
		                { 0x002, new CMwMethodInfo("ModeDeleteVertexs", 0x00000000, null, null) },
		                { 0x003, new CMwFieldInfo("VertPos", 0x00000035) },
		                { 0x004, new CMwFieldInfo("VertRGB", 0x00000009) },
		                { 0x005, new CMwFieldInfo("VertAlpha", 0x00000028) },
		                { 0x006, new CMwFieldInfo("HelperMobil", 0x00000005) },
		                { 0x007, new CMwFieldInfo("Helper3DMobil", 0x00000005) },
		                { 0x008, new CMwFieldInfo("Frame", 0x00000005) }
	                  })
	                },
	                { 0x04B, new CMwClassInfo("CGameCtnMediaBlockTriangles2D", new Dictionary<int, CMwMemberInfo>()
	                  {
	                  })
	                },
	                { 0x04C, new CMwClassInfo("CGameCtnMediaBlockTriangles3D", new Dictionary<int, CMwMemberInfo>()
	                  {
	                  })
	                },
	                { 0x04D, new CMwClassInfo("CGameNetOnlineNewsReply", new Dictionary<int, CMwMemberInfo>()
	                  {
	                  })
	                },
	                { 0x04E, new CMwClassInfo("CGameCtnBlockInfo", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwEnumInfo("WayPointType", new string[] { "Start", "Finish", "Checkpoint", "None", "StartFinish" }) },
		                { 0x001, new CMwEnumInfo("Selection", new string[] { "Random", "Obsolete", "AutoRotate" }) },
		                { 0x002, new CMwFieldInfo("IsPillar", 0x00000001) },
		                { 0x003, new CMwFieldInfo("IsReplacement", 0x00000001) },
		                { 0x004, new CMwFieldInfo("GroundBlockUnitInfos", 0x00000006) },
		                { 0x005, new CMwFieldInfo("AirBlockUnitInfos", 0x00000006) },
		                { 0x006, new CMwFieldInfo("Pillars", 0x00000005) },
		                { 0x007, new CMwFieldInfo("NoRespawn", 0x00000001) },
		                { 0x008, new CMwFieldInfo("SpawnLocGround", 0x00000013) },
		                { 0x009, new CMwFieldInfo("SpawnLocAir", 0x00000013) },
		                { 0x00A, new CMwFieldInfo("GroundHelperMobil", 0x00000005) },
		                { 0x00B, new CMwFieldInfo("AirHelperMobil", 0x00000005) },
		                { 0x00C, new CMwFieldInfo("ConstructionModeHelperMobil", 0x00000005) }
	                  })
	                },
	                { 0x04F, new CMwClassInfo("CGameCtnBlockInfoFlat", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("MobilsBase", 0x00000007) }
	                  })
	                },
	                { 0x050, new CMwClassInfo("CGameCtnBlockInfoFrontier", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("MobilsBase1", 0x00000007) },
		                { 0x001, new CMwFieldInfo("MobilsBase3", 0x00000007) },
		                { 0x002, new CMwFieldInfo("MobilsBase5", 0x00000007) },
		                { 0x003, new CMwFieldInfo("MobilsBase7", 0x00000007) },
		                { 0x004, new CMwFieldInfo("MobilsBase15", 0x00000007) },
		                { 0x005, new CMwFieldInfo("MobilsDeadend", 0x00000007) },
		                { 0x006, new CMwFieldInfo("MobilsDeadend4", 0x00000007) },
		                { 0x007, new CMwFieldInfo("MobilsDeadend8", 0x00000007) },
		                { 0x008, new CMwFieldInfo("MobilsDeadend12", 0x00000007) },
		                { 0x009, new CMwFieldInfo("MobilsCorner", 0x00000007) },
		                { 0x00A, new CMwFieldInfo("MobilsCorner8", 0x00000007) },
		                { 0x00B, new CMwFieldInfo("MobilsStraight", 0x00000007) },
		                { 0x00C, new CMwFieldInfo("MobilsTshaped", 0x00000007) },
		                { 0x00D, new CMwFieldInfo("MobilsCross", 0x00000007) }
	                  })
	                },
	                { 0x051, new CMwClassInfo("CGameCtnBlockInfoClassic", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("MobilsGround", 0x00000007) },
		                { 0x001, new CMwFieldInfo("MobilsAir", 0x00000007) }
	                  })
	                },
	                { 0x052, new CMwClassInfo("CGameCtnBlockInfoRoad", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("Slope", 0x00000005) },
		                { 0x001, new CMwFieldInfo("MobilsGroundBase", 0x00000007) },
		                { 0x002, new CMwFieldInfo("MobilsGroundCorner", 0x00000007) },
		                { 0x003, new CMwFieldInfo("MobilsGroundCross", 0x00000007) },
		                { 0x004, new CMwFieldInfo("MobilsGroundDeadend", 0x00000007) },
		                { 0x005, new CMwFieldInfo("MobilsGroundStraight", 0x00000007) },
		                { 0x006, new CMwFieldInfo("MobilsGroundTshaped", 0x00000007) },
		                { 0x007, new CMwFieldInfo("MobilsAirBase", 0x00000007) },
		                { 0x008, new CMwFieldInfo("MobilsAirCorner", 0x00000007) },
		                { 0x009, new CMwFieldInfo("MobilsAirCross", 0x00000007) },
		                { 0x00A, new CMwFieldInfo("MobilsAirDeadend", 0x00000007) },
		                { 0x00B, new CMwFieldInfo("MobilsAirStraight", 0x00000007) },
		                { 0x00C, new CMwFieldInfo("MobilsAirTshaped", 0x00000007) }
	                  })
	                },
	                { 0x053, new CMwClassInfo("CGameCtnBlockInfoClip", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("MobilsGroundBase", 0x00000007) },
		                { 0x001, new CMwFieldInfo("MobilsAirBase", 0x00000007) },
		                { 0x002, new CMwFieldInfo("AsymetricalClipId", 0x00000029) }
	                  })
	                },
	                { 0x054, new CMwClassInfo("CGameCtnBlockInfoSlope", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("MobilsGroundBase", 0x00000007) },
		                { 0x001, new CMwFieldInfo("MobilsGroundStart", 0x00000007) },
		                { 0x002, new CMwFieldInfo("MobilsAirBase", 0x00000007) },
		                { 0x003, new CMwFieldInfo("MobilsAirStart", 0x00000007) },
		                { 0x004, new CMwFieldInfo("MobilsAirStraight", 0x00000007) },
		                { 0x005, new CMwFieldInfo("MobilsAirEnd", 0x00000007) }
	                  })
	                },
	                { 0x055, new CMwClassInfo("CGameCtnBlockInfoPylon", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("MobilsGroundHeight1", 0x00000007) },
		                { 0x001, new CMwFieldInfo("MobilsGroundHeight2", 0x00000007) },
		                { 0x002, new CMwFieldInfo("MobilsGroundHeight3", 0x00000007) },
		                { 0x003, new CMwFieldInfo("MobilsGroundHeight4", 0x00000007) },
		                { 0x004, new CMwFieldInfo("MobilsGroundHeight5", 0x00000007) },
		                { 0x005, new CMwFieldInfo("MobilsGroundHeight6", 0x00000007) },
		                { 0x006, new CMwFieldInfo("MobilsGroundHeight7", 0x00000007) },
		                { 0x007, new CMwFieldInfo("MobilsGroundHeight8", 0x00000007) },
		                { 0x008, new CMwFieldInfo("MobilsGroundHeight9", 0x00000007) },
		                { 0x009, new CMwFieldInfo("MobilsGroundHeight10", 0x00000007) },
		                { 0x00A, new CMwFieldInfo("MobilsGroundHeight11", 0x00000007) },
		                { 0x00B, new CMwFieldInfo("MobilsGroundHeight12", 0x00000007) },
		                { 0x00C, new CMwFieldInfo("MobilsGroundHeight13", 0x00000007) },
		                { 0x00D, new CMwFieldInfo("MobilsGroundHeight14", 0x00000007) },
		                { 0x00E, new CMwFieldInfo("MobilsGroundHeight15", 0x00000007) },
		                { 0x00F, new CMwFieldInfo("PylonMobilStart", 0x00000005) },
		                { 0x010, new CMwFieldInfo("PylonMobilStraight", 0x00000005) },
		                { 0x011, new CMwFieldInfo("PylonMobilEnd", 0x00000005) }
	                  })
	                },
	                { 0x056, new CMwClassInfo("CGameCtnBlockInfoRectAsym", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("MobilsGroundCornerNE", 0x00000007) },
		                { 0x001, new CMwFieldInfo("MobilsGroundCornerNW", 0x00000007) },
		                { 0x002, new CMwFieldInfo("MobilsGroundCornerSE", 0x00000007) },
		                { 0x003, new CMwFieldInfo("MobilsGroundCornerSW", 0x00000007) },
		                { 0x004, new CMwFieldInfo("MobilsGroundCross", 0x00000007) },
		                { 0x005, new CMwFieldInfo("MobilsGroundTShapedE", 0x00000007) },
		                { 0x006, new CMwFieldInfo("MobilsGroundTShapedN", 0x00000007) },
		                { 0x007, new CMwFieldInfo("MobilsGroundTShapedS", 0x00000007) },
		                { 0x008, new CMwFieldInfo("MobilsGroundTShapedW", 0x00000007) },
		                { 0x009, new CMwFieldInfo("MobilsAirCornerNE", 0x00000007) },
		                { 0x00A, new CMwFieldInfo("MobilsAirCornerNW", 0x00000007) },
		                { 0x00B, new CMwFieldInfo("MobilsAirCornerSE", 0x00000007) },
		                { 0x00C, new CMwFieldInfo("MobilsAirCornerSW", 0x00000007) },
		                { 0x00D, new CMwFieldInfo("MobilsAirCross", 0x00000007) },
		                { 0x00E, new CMwFieldInfo("MobilsAirTShapedE", 0x00000007) },
		                { 0x00F, new CMwFieldInfo("MobilsAirTShapedN", 0x00000007) },
		                { 0x010, new CMwFieldInfo("MobilsAirTShapedS", 0x00000007) },
		                { 0x011, new CMwFieldInfo("MobilsAirTShapedW", 0x00000007) }
	                  })
	                },
	                { 0x057, new CMwClassInfo("CGameCtnBlock", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("DescId", 0x00000029) },
		                { 0x001, new CMwFieldInfo("DescCollection", 0x00000029) },
		                { 0x002, new CMwFieldInfo("DescAuthor", 0x00000029) },
		                { 0x003, new CMwFieldInfo("CoordX", 0x0000001F) },
		                { 0x004, new CMwFieldInfo("CoordY", 0x0000001F) },
		                { 0x005, new CMwFieldInfo("CoordZ", 0x0000001F) },
		                { 0x006, new CMwEnumInfo("Dir", new string[] { "North", "East", "South", "West" }) },
		                { 0x007, new CMwFieldInfo("Mobil", 0x00000005) },
		                { 0x008, new CMwFieldInfo("Skin", 0x00000005) },
		                { 0x009, new CMwMethodInfo("ApplySkin", 0x00000000, null, null) },
		                { 0x00A, new CMwFieldInfo("Editable", 0x00000001) },
		                { 0x00B, new CMwFieldInfo("BlockInfo", 0x00000005) }
	                  })
	                },
	                { 0x058, new CMwClassInfo("CGameCtnBlockUnit", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("Junctions", 0x0000001F) }
	                  })
	                },
	                { 0x059, new CMwClassInfo("CGameCtnBlockSkin", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("IsDirty", 0x00000001) },
		                { 0x001, new CMwFieldInfo("PackDesc", 0x00000005) },
		                { 0x002, new CMwFieldInfo("ParentPackDesc", 0x00000005) },
		                { 0x003, new CMwFieldInfo("Text", 0x0000002D) }
	                  })
	                },
	                { 0x05A, new CMwClassInfo("CGameCtnPylonColumn", new Dictionary<int, CMwMemberInfo>()
	                  {
	                  })
	                },
	                { 0x05B, new CMwClassInfo("CGameCtnChallengeParameters", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("AuthorScore", 0x0000001F) },
		                { 0x001, new CMwFieldInfo("AuthorTime", 0x0000001F) },
		                { 0x002, new CMwFieldInfo("GoldTime", 0x0000001F) },
		                { 0x003, new CMwFieldInfo("SilverTime", 0x0000001F) },
		                { 0x004, new CMwFieldInfo("BronzeTime", 0x0000001F) },
		                { 0x005, new CMwFieldInfo("TimeLimit", 0x0000001F) },
		                { 0x006, new CMwMethodInfo("ResetGoldSilverBronzeStunts", 0x00000000, null, null) },
		                { 0x007, new CMwFieldInfo("Tip", 0x00000029) }
	                  })
	                },
	                { 0x05C, new CMwClassInfo("CGameCtnZone", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("ZoneId", 0x00000029) },
		                { 0x001, new CMwFieldInfo("SurfaceId", 0x00000029) },
		                { 0x002, new CMwFieldInfo("Height", 0x0000001F) },
		                { 0x003, new CMwFieldInfo("Depth", 0x0000001F) },
		                { 0x004, new CMwFieldInfo("OldZone", 0x00000001) },
		                { 0x005, new CMwFieldInfo("HasWater", 0x00000001) }
	                  })
	                },
	                { 0x05D, new CMwClassInfo("CGameCtnZoneFlat", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("BlockInfoFlat", 0x00000005) },
		                { 0x001, new CMwFieldInfo("BlockInfoClip", 0x00000005) },
		                { 0x002, new CMwFieldInfo("BlockInfoRoad", 0x00000005) },
		                { 0x003, new CMwFieldInfo("BlockInfoPylon", 0x00000005) },
		                { 0x004, new CMwFieldInfo("GroundOnly", 0x00000001) }
	                  })
	                },
	                { 0x05E, new CMwClassInfo("CGameCtnZoneFrontier", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("BlockInfoFrontier", 0x00000005) },
		                { 0x001, new CMwFieldInfo("ParentZone", 0x00000029) },
		                { 0x002, new CMwFieldInfo("ChildZone", 0x00000029) }
	                  })
	                },
	                { 0x05F, new CMwClassInfo("CGameCtnZoneTest", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("BlockInfoFlat", 0x00000005) },
		                { 0x001, new CMwFieldInfo("BlockInfoClip", 0x00000005) },
		                { 0x002, new CMwFieldInfo("BlockInfoPylon", 0x00000005) },
		                { 0x003, new CMwFieldInfo("BlockInfoTest", 0x00000005) },
		                { 0x004, new CMwFieldInfo("BlockInfoBlockClassic", 0x00000005) },
		                { 0x005, new CMwFieldInfo("BlockInfoBlockAccepPylons", 0x00000005) }
	                  })
	                },
	                { 0x060, new CMwClassInfo("CGameCtnMediaVideoParams", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("Fps", 0x0000001F) },
		                { 0x001, new CMwFieldInfo("Width", 0x0000001F) },
		                { 0x002, new CMwFieldInfo("Height", 0x0000001F) },
		                { 0x003, new CMwFieldInfo("Hq", 0x00000001) },
		                { 0x004, new CMwFieldInfo("HqSampleSize", 0x0000001F) },
		                { 0x005, new CMwFieldInfo("HqMB", 0x00000001) },
		                { 0x006, new CMwFieldInfo("HqSoftShadows", 0x00000001) },
		                { 0x007, new CMwFieldInfo("HqAmbientOcc", 0x00000001) },
		                { 0x008, new CMwEnumInfo("Stereo3d", new string[] { "None", "Red-Cyan", "Left-Right" }) },
		                { 0x009, new CMwFieldInfo("IsAudioStream", 0x00000001) }
	                  })
	                },
	                { 0x061, new CMwClassInfo("CGameCampaignsScoresManager", new Dictionary<int, CMwMemberInfo>()
	                  {
	                  })
	                },
	                { 0x062, new CMwClassInfo("CGameSkillScoreComputer", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("GameMode", 0x0000001F) },
		                { 0x001, new CMwFieldInfo("Record", 0x0000001F) },
		                { 0x002, new CMwFieldInfo("TotalPlayersCount", 0x00000024) },
		                { 0x003, new CMwFieldInfo("PlayersWithSameRecordCount", 0x00000024) },
		                { 0x004, new CMwFieldInfo("PlayerRank", 0x00000024) },
		                { 0x005, new CMwFieldInfo("SkillScore", 0x0000001F) }
	                  })
	                },
	                { 0x063, new CMwClassInfo("CGameCampaignScores", new Dictionary<int, CMwMemberInfo>()
	                  {
	                  })
	                },
	                { 0x064, new CMwClassInfo("CGameChallengeScores", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("ChallengeId", 0x0000001B) }
	                  })
	                },
	                { 0x065, new CMwClassInfo("CGameGeneralScores", new Dictionary<int, CMwMemberInfo>()
	                  {
	                  })
	                },
	                { 0x066, new CMwClassInfo("CGameManialinkBrowser", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("Menu", 0x00000005) },
		                { 0x001, new CMwFieldInfo("FiberCmd", 0x00000005) },
		                { 0x002, new CMwFieldInfo("Manialink_Enabled", 0x00000001) },
		                { 0x003, new CMwFieldInfo("Manialink_Active", 0x00000001) },
		                { 0x004, new CMwFieldInfo("ManialinkBrowser_Link", 0x0000002D) },
		                { 0x005, new CMwMethodInfo("ManialinkBrowser_OnHome", 0x00000000, null, null) },
		                { 0x006, new CMwMethodInfo("ManialinkBrowser_OnQuit", 0x00000000, null, null) },
		                { 0x007, new CMwMethodInfo("ManialinkBrowser_OnBack", 0x00000000, null, null) },
		                { 0x008, new CMwMethodInfo("ManialinkBrowser_OnForward", 0x00000000, null, null) }
	                  })
	                },
	                { 0x067, new CMwClassInfo("CGameNetFormAdmin", new Dictionary<int, CMwMemberInfo>()
	                  {
	                  })
	                },
	                { 0x068, new CMwClassInfo("CGameNetFileTransfer", new Dictionary<int, CMwMemberInfo>()
	                  {
	                  })
	                },
	                { 0x069, new CMwClassInfo("CGameNetFormTimeSync", new Dictionary<int, CMwMemberInfo>()
	                  {
	                  })
	                },
	                { 0x06A, new CMwClassInfo("CGameNetFormCallVote", new Dictionary<int, CMwMemberInfo>()
	                  {
	                  })
	                },
	                { 0x06B, new CMwClassInfo("CGameControlCamera", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwMethodInfo("Reset", 0x00000000, null, null) },
		                { 0x001, new CMwFieldInfo("IsActive", 0x00000001) },
		                { 0x002, new CMwFieldInfo("IsFirstPerson", 0x00000001) },
		                { 0x003, new CMwFieldInfo("CanCameraMove", 0x00000001) },
		                { 0x004, new CMwFieldInfo("UseOnlyFollowedMobilPosition", 0x00000001) },
		                { 0x005, new CMwFieldInfo("IsFollowing", 0x00000001) },
		                { 0x006, new CMwFieldInfo("RelativeFollowedPos", 0x00000035) },
		                { 0x007, new CMwFieldInfo("FollowedGameMobilId", 0x0000001F) },
		                { 0x008, new CMwFieldInfo("PlaneDist", 0x00000024) },
		                { 0x009, new CMwFieldInfo("MinDist", 0x00000024) },
		                { 0x00A, new CMwFieldInfo("MaxDist", 0x00000024) },
		                { 0x00B, new CMwFieldInfo("Fov", 0x00000024) },
		                { 0x00C, new CMwFieldInfo("DefaultFov", 0x00000024) },
		                { 0x00D, new CMwFieldInfo("MinFov", 0x00000024) },
		                { 0x00E, new CMwFieldInfo("MaxFov", 0x00000024) },
		                { 0x00F, new CMwFieldInfo("NearZ", 0x00000024) },
		                { 0x010, new CMwFieldInfo("DefaultNearZ", 0x00000024) },
		                { 0x011, new CMwFieldInfo("MinNearZ", 0x00000024) },
		                { 0x012, new CMwFieldInfo("MaxNearZ", 0x00000024) },
		                { 0x013, new CMwFieldInfo("FarZ", 0x00000024) },
		                { 0x014, new CMwFieldInfo("DefaultFarZ", 0x00000024) },
		                { 0x015, new CMwFieldInfo("MinFarZ", 0x00000024) },
		                { 0x016, new CMwFieldInfo("MaxFarZ", 0x00000024) },
		                { 0x017, new CMwFieldInfo("Speed", 0x00000035) },
		                { 0x018, new CMwFieldInfo("MaxSpeed", 0x00000024) },
		                { 0x019, new CMwFieldInfo("UseFollowedOrientation", 0x00000001) },
		                { 0x01A, new CMwFieldInfo("UseForcedLocation", 0x00000001) },
		                { 0x01B, new CMwFieldInfo("ForcedLocation", 0x00000013) },
		                { 0x01C, new CMwFieldInfo("UseForcedUp", 0x00000001) },
		                { 0x01D, new CMwFieldInfo("ForcedUp", 0x00000035) },
		                { 0x01E, new CMwFieldInfo("Effects", 0x00000005) },
		                { 0x01F, new CMwFieldInfo("GameScene", 0x00000005) }
	                  })
	                },
	                { 0x06D, new CMwClassInfo("CGameControlCameraFree", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("Translation", 0x00000035) },
		                { 0x001, new CMwFieldInfo("X", 0x00000024) },
		                { 0x002, new CMwFieldInfo("Y", 0x00000024) },
		                { 0x003, new CMwFieldInfo("Z", 0x00000024) },
		                { 0x004, new CMwFieldInfo("Pitch", 0x00000024) },
		                { 0x005, new CMwFieldInfo("Yaw", 0x00000024) },
		                { 0x006, new CMwFieldInfo("Roll", 0x00000024) },
		                { 0x007, new CMwFieldInfo("ClampPitch", 0x00000001) },
		                { 0x008, new CMwFieldInfo("ClampPitchMin", 0x00000024) },
		                { 0x009, new CMwFieldInfo("ClampPitchMax", 0x00000024) },
		                { 0x00A, new CMwFieldInfo("Acceleration", 0x00000024) },
		                { 0x00B, new CMwFieldInfo("StartMoveSpeed", 0x00000024) },
		                { 0x00C, new CMwFieldInfo("MoveSpeedCoef", 0x00000023) },
		                { 0x00D, new CMwFieldInfo("MoveSpeed", 0x00000024) },
		                { 0x00E, new CMwFieldInfo("MoveInertia", 0x00000024) },
		                { 0x00F, new CMwFieldInfo("RotateSpeed", 0x00000024) },
		                { 0x010, new CMwFieldInfo("RotateInertia", 0x00000024) },
		                { 0x011, new CMwFieldInfo("Radius", 0x00000024) },
		                { 0x012, new CMwFieldInfo("UseForcedRoll", 0x00000001) },
		                { 0x013, new CMwFieldInfo("ForcedRoll", 0x00000024) },
		                { 0x014, new CMwFieldInfo("StereoRadiusScaleEyeDist", 0x00000024) }
	                  })
	                },
	                { 0x06E, new CMwClassInfo("CGameControlCameraOrbital3d", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("UseAutoRadiusFromTargets", 0x00000001) },
		                { 0x001, new CMwFieldInfo("RotateSpeed", 0x00000031) },
		                { 0x002, new CMwFieldInfo("RadiusScale", 0x00000035) },
		                { 0x003, new CMwFieldInfo("OcclusionIsEnable", 0x00000001) },
		                { 0x004, new CMwFieldInfo("UsingBorderToRotate", 0x00000001) },
		                { 0x005, new CMwFieldInfo("IsMouseRotationPressed", 0x00000001) },
		                { 0x006, new CMwFieldInfo("IsMouseZoomPressed", 0x00000001) },
		                { 0x007, new CMwFieldInfo("IsMouseFovPressed", 0x00000001) },
		                { 0x008, new CMwFieldInfo("MouseBorderMoveSize", 0x00000024) },
		                { 0x009, new CMwFieldInfo("OcclusionTargetRadius", 0x00000024) },
		                { 0x00A, new CMwFieldInfo("OcclusionDistFromHit", 0x00000024) },
		                { 0x00B, new CMwFieldInfo("Radius", 0x00000024) },
		                { 0x00C, new CMwFieldInfo("WheelSensitivity", 0x00000024) },
		                { 0x00D, new CMwFieldInfo("FovKeySensitivity", 0x00000024) },
		                { 0x00E, new CMwFieldInfo("ZoomKeySensitivity", 0x00000024) },
		                { 0x00F, new CMwFieldInfo("Latitude", 0x00000024) },
		                { 0x010, new CMwFieldInfo("Longitude", 0x00000024) },
		                { 0x011, new CMwFieldInfo("DefaultRadius", 0x00000024) },
		                { 0x012, new CMwFieldInfo("RadiusMin", 0x00000024) },
		                { 0x013, new CMwFieldInfo("RadiusMax", 0x00000024) },
		                { 0x014, new CMwFieldInfo("LatitudeMin", 0x00000024) },
		                { 0x015, new CMwFieldInfo("LatitudeMax", 0x00000024) }
	                  })
	                },
	                { 0x06F, new CMwClassInfo("CGameControlCameraEffect", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwMethodInfo("Reset", 0x00000000, null, null) },
		                { 0x001, new CMwMethodInfo("Install", 0x00000000, null, null) },
		                { 0x002, new CMwMethodInfo("Uninstall", 0x00000000, null, null) },
		                { 0x003, new CMwFieldInfo("IsInstalled", 0x00000001) }
	                  })
	                },
	                { 0x070, new CMwClassInfo("CGameControlCameraEffectGroup", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("Effects", 0x00000007) }
	                  })
	                },
	                { 0x071, new CMwClassInfo("CGameControlCameraEffectShake", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("OffsetPos", 0x00000035) },
		                { 0x001, new CMwFieldInfo("Yaw", 0x00000024) },
		                { 0x002, new CMwFieldInfo("Pitch", 0x00000024) },
		                { 0x003, new CMwFieldInfo("Roll", 0x00000024) },
		                { 0x004, new CMwFieldInfo("Speed", 0x00000024) },
		                { 0x005, new CMwFieldInfo("Intensity", 0x00000024) },
		                { 0x006, new CMwFieldInfo("CurrentTime", 0x0000001F) }
	                  })
	                },
	                { 0x072, new CMwClassInfo("CGameControlCameraTarget", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("TargetGameMobilId", 0x0000001F) },
		                { 0x001, new CMwFieldInfo("CanUseRelativeTargetLocation", 0x00000001) },
		                { 0x002, new CMwFieldInfo("RelativeTargetLocation", 0x00000013) },
		                { 0x003, new CMwFieldInfo("IsUpLinked", 0x00000001) },
		                { 0x004, new CMwFieldInfo("Interpolate", 0x00000001) },
		                { 0x005, new CMwFieldInfo("LookAtFactor", 0x00000024) },
		                { 0x006, new CMwFieldInfo("Translation", 0x00000035) }
	                  })
	                },
	                { 0x073, new CMwClassInfo("CGameRace", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("InterfaceFid", 0x00000005) },
		                { 0x001, new CMwEnumInfo("SpectatorCameraType", new string[] { "|SpectatorCam|Replay", "|SpectatorCam|Follow", "|SpectatorCam|Free" }) },
		                { 0x002, new CMwEnumInfo("SpectatorCameraTarget", new string[] { "|SpectatorCam|Manual", "|SpectatorCam|Auto" }) },
		                { 0x003, new CMwFieldInfo("TargetPlayerInfo", 0x00000005) },
		                { 0x004, new CMwFieldInfo("SpectatorTargetName", 0x0000002D) },
		                { 0x005, new CMwFieldInfo("MediaClipPlayer", 0x00000005) },
		                { 0x006, new CMwFieldInfo("MediaClipPlayerGlobal", 0x00000005) },
		                { 0x007, new CMwFieldInfo("ReplayRecord", 0x00000005) },
		                { 0x008, new CMwFieldInfo("PrevReplayRecord", 0x00000005) },
		                { 0x009, new CMwMethodInfo("WriteDebugValidateStringToDisk", 0x00000000, null, null) }
	                  })
	                },
	                { 0x076, new CMwClassInfo("CGameLadderRanking", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("Name", 0x0000002D) },
		                { 0x001, new CMwFieldInfo("Path", 0x0000002D) },
		                { 0x002, new CMwFieldInfo("Login", 0x00000029) },
		                { 0x003, new CMwFieldInfo("ScoreStr", 0x00000029) },
		                { 0x004, new CMwFieldInfo("Ranking", 0x0000001F) },
		                { 0x005, new CMwFieldInfo("Ranking2", 0x0000001F) },
		                { 0x006, new CMwFieldInfo("Score", 0x0000001F) },
		                { 0x007, new CMwFieldInfo("Stars", 0x0000001F) },
		                { 0x008, new CMwFieldInfo("ChildsCount", 0x0000001F) },
		                { 0x009, new CMwFieldInfo("IsFolder", 0x00000001) }
	                  })
	                },
	                { 0x077, new CMwClassInfo("CGameCtnMediaBlock", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("IsInstalled", 0x00000001) },
		                { 0x001, new CMwFieldInfo("IsActive", 0x00000001) },
		                { 0x002, new CMwMethodInfo("SwitchOn", 0x00000000, null, null) },
		                { 0x003, new CMwMethodInfo("SwitchOff", 0x00000000, null, null) },
		                { 0x004, new CMwFieldInfo("Start", 0x00000024) },
		                { 0x005, new CMwFieldInfo("End", 0x00000024) }
	                  })
	                },
	                { 0x078, new CMwClassInfo("CGameCtnMediaTrack", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("Name", 0x0000002D) },
		                { 0x001, new CMwFieldInfo("IsReadOnly", 0x00000001) },
		                { 0x002, new CMwFieldInfo("IsKeepPlaying", 0x00000001) },
		                { 0x003, new CMwFieldInfo("Blocks", 0x00000007) }
	                  })
	                },
	                { 0x079, new CMwClassInfo("CGameCtnMediaClip", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("Name", 0x0000002D) },
		                { 0x001, new CMwFieldInfo("Tracks", 0x00000007) },
		                { 0x002, new CMwFieldInfo("Scene", 0x00000005) },
		                { 0x003, new CMwFieldInfo("LocalPlayerGhostId", 0x0000001F) }
	                  })
	                },
	                { 0x07A, new CMwClassInfo("CGameCtnMediaClipGroup", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("Clips", 0x00000007) }
	                  })
	                },
	                { 0x07C, new CMwClassInfo("CGameCtnMediaBlockCamera", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("IsInstalled", 0x00000001) },
		                { 0x001, new CMwFieldInfo("IsActive", 0x00000001) }
	                  })
	                },
	                { 0x07D, new CMwClassInfo("CGameCtnMediaBlockUi", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("UserInterface", 0x00000005) }
	                  })
	                },
	                { 0x07E, new CMwClassInfo("CGameCtnMediaBlockFx", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("FadeDuration", 0x00000024) },
		                { 0x001, new CMwFieldInfo("Scene3d", 0x00000005) },
		                { 0x002, new CMwFieldInfo("SceneFx", 0x00000005) }
	                  })
	                },
	                { 0x07F, new CMwClassInfo("CGameCtnMediaBlockFxBlur", new Dictionary<int, CMwMemberInfo>()
	                  {
	                  })
	                },
	                { 0x080, new CMwClassInfo("CGameCtnMediaBlockFxColors", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwMethodInfo("SwitchOn", 0x00000000, null, null) }
	                  })
	                },
	                { 0x081, new CMwClassInfo("CGameCtnMediaBlockFxBlurDepth", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwMethodInfo("SwitchOn", 0x00000000, null, null) }
	                  })
	                },
	                { 0x082, new CMwClassInfo("CGameCtnMediaBlockFxBlurMotion", new Dictionary<int, CMwMemberInfo>()
	                  {
	                  })
	                },
	                { 0x083, new CMwClassInfo("CGameCtnMediaBlockFxBloom", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwMethodInfo("SwitchOn", 0x00000000, null, null) }
	                  })
	                },
	                { 0x084, new CMwClassInfo("CGameCtnMediaBlockCameraGame", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("GameCamId", 0x0000001B) },
		                { 0x001, new CMwFieldInfo("GhostId", 0x0000001F) }
	                  })
	                },
	                { 0x085, new CMwClassInfo("CGameCtnMediaBlockTime", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwMethodInfo("SwitchOn", 0x00000000, null, null) }
	                  })
	                },
	                { 0x086, new CMwClassInfo("CGameCtnMediaClipPlayer", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("Clip", 0x00000005) },
		                { 0x001, new CMwFieldInfo("Scene2d", 0x00000005) },
		                { 0x002, new CMwFieldInfo("EdMediaTracks", 0x00000007) },
		                { 0x003, new CMwFieldInfo("LocalPlayerGameMobilId", 0x0000001F) },
		                { 0x004, new CMwFieldInfo("LocalPlayerGhost", 0x00000005) }
	                  })
	                },
	                { 0x087, new CMwClassInfo("CGameCtnMediaBlockEvent", new Dictionary<int, CMwMemberInfo>()
	                  {
	                  })
	                },
	                { 0x088, new CMwClassInfo("CGameRaceInterface", new Dictionary<int, CMwMemberInfo>()
	                  {
	                  })
	                },
	                { 0x089, new CMwClassInfo("CGameManiaNetResource", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("Url", 0x00000029) }
	                  })
	                },
	                { 0x08A, new CMwClassInfo("CGamePlayerInfo", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("WishSpectator", 0x00000001) },
		                { 0x001, new CMwFieldInfo("ForcedSpectator", 0x00000001) },
		                { 0x002, new CMwFieldInfo("Path", 0x0000002D) },
		                { 0x003, new CMwFieldInfo("StrOfficial", 0x00000029) },
		                { 0x004, new CMwFieldInfo("OfficialEnum", 0x0000001F) },
		                { 0x005, new CMwFieldInfo("StrLadderRanking", 0x0000002D) },
		                { 0x006, new CMwFieldInfo("StrLadderRankingSimple", 0x0000002D) },
		                { 0x007, new CMwFieldInfo("StrLadderScore", 0x00000029) },
		                { 0x008, new CMwFieldInfo("StrLadderLastPoints", 0x00000029) },
		                { 0x009, new CMwFieldInfo("StrLadderWins", 0x00000029) },
		                { 0x00A, new CMwFieldInfo("StrLadderDraws", 0x00000029) },
		                { 0x00B, new CMwFieldInfo("StrLadderLosses", 0x00000029) },
		                { 0x00C, new CMwFieldInfo("StrLadderTeamName", 0x00000029) },
		                { 0x00D, new CMwFieldInfo("StrLadderTeamRanking", 0x00000029) },
		                { 0x00E, new CMwFieldInfo("StrLadderTeamRankingSimple", 0x00000029) },
		                { 0x00F, new CMwFieldInfo("StrLadderNbrTeams", 0x00000029) },
		                { 0x010, new CMwFieldInfo("StrLadderScoreRounded", 0x00000029) }
	                  })
	                },
	                { 0x08B, new CMwClassInfo("CGamePlayerCameraSet", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("CamsMaster", 0x00000005) }
	                  })
	                },
	                { 0x08C, new CMwClassInfo("CGamePlayerProfile", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("ProfileName", 0x00000029) },
		                { 0x001, new CMwFieldInfo("DisplayProfileName", 0x0000002D) },
		                { 0x002, new CMwFieldInfo("NickName", 0x0000002D) },
		                { 0x003, new CMwFieldInfo("OnlinePath", 0x0000002D) },
		                { 0x004, new CMwFieldInfo("OnlineWishedPath", 0x0000002D) },
		                { 0x005, new CMwFieldInfo("OnlineLogin", 0x00000029) },
		                { 0x006, new CMwFieldInfo("OnlinePassword", 0x00000029) },
		                { 0x007, new CMwFieldInfo("OnlineValidationKey", 0x00000029) },
		                { 0x008, new CMwFieldInfo("League", 0x00000005) },
		                { 0x009, new CMwFieldInfo("LoginValidated", 0x00000001) },
		                { 0x00A, new CMwFieldInfo("RememberOnlinePassword", 0x00000001) },
		                { 0x00B, new CMwFieldInfo("ReceiveNews", 0x00000001) },
		                { 0x00C, new CMwFieldInfo("AutoConnect", 0x00000001) },
		                { 0x00D, new CMwFieldInfo("EnableCarSkinGeom", 0x00000001) },
		                { 0x00E, new CMwFieldInfo("EnableAvatars", 0x00000001) },
		                { 0x00F, new CMwFieldInfo("EnableChat", 0x00000001) },
		                { 0x010, new CMwFieldInfo("EnableUnlimitedHorns", 0x00000001) },
		                { 0x011, new CMwFieldInfo("IsShowPlayerGhost", 0x00000001) },
		                { 0x012, new CMwFieldInfo("Description", 0x0000002D) },
		                { 0x013, new CMwFieldInfo("CurrentSoloPlaylistName", 0x0000002D) },
		                { 0x014, new CMwFieldInfo("CurrentSoloPlaylistPath", 0x0000002D) },
		                { 0x015, new CMwFieldInfo("OnlineCoppers", 0x0000001F) },
		                { 0x016, new CMwFieldInfo("UnlockAllCheat", 0x00000001) },
		                { 0x017, new CMwFieldInfo("AskOpponents", 0x00000001) },
		                { 0x018, new CMwFieldInfo("LockHigherDifficulties", 0x00000001) }
	                  })
	                },
	                { 0x08D, new CMwClassInfo("CGamePlayerScore", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("PlayerName", 0x00000029) },
		                { 0x001, new CMwFieldInfo("NickName", 0x0000002D) },
		                { 0x002, new CMwMethodInfo("ComputeStats", 0x00000000, null, null) },
		                { 0x003, new CMwFieldInfo("ChallengesCount", 0x0000001F) },
		                { 0x004, new CMwFieldInfo("TotalPlayTime", 0x0000001F) },
		                { 0x005, new CMwFieldInfo("EditPlayTime", 0x0000001F) },
		                { 0x006, new CMwFieldInfo("RacePlayTime", 0x0000001F) },
		                { 0x007, new CMwFieldInfo("NetPlayTime", 0x0000001F) },
		                { 0x008, new CMwFieldInfo("TotalResetCount", 0x0000001F) },
		                { 0x009, new CMwFieldInfo("TotalFinishCount", 0x0000001F) },
		                { 0x00A, new CMwFieldInfo("MaxPlayTime", 0x0000001F) },
		                { 0x00B, new CMwFieldInfo("MaxEditPlayTime", 0x0000001F) },
		                { 0x00C, new CMwFieldInfo("MaxRacePlayTime", 0x0000001F) },
		                { 0x00D, new CMwFieldInfo("MaxNetPlayTime", 0x0000001F) },
		                { 0x00E, new CMwFieldInfo("MaxResetCount", 0x0000001F) },
		                { 0x00F, new CMwFieldInfo("MaxFinishCount", 0x0000001F) },
		                { 0x010, new CMwFieldInfo("AvgPlayTime", 0x0000001F) },
		                { 0x011, new CMwFieldInfo("AvgEditPlayTime", 0x0000001F) },
		                { 0x012, new CMwFieldInfo("AvgRacePlayTime", 0x0000001F) },
		                { 0x013, new CMwFieldInfo("AvgNetPlayTime", 0x0000001F) },
		                { 0x014, new CMwFieldInfo("AvgResetCount", 0x0000001F) },
		                { 0x015, new CMwFieldInfo("AvgFinishCount", 0x0000001F) },
		                { 0x016, new CMwFieldInfo("MostPlayed", 0x0000002D) },
		                { 0x017, new CMwFieldInfo("MostEdited", 0x0000002D) },
		                { 0x018, new CMwFieldInfo("MostRaced", 0x0000002D) },
		                { 0x019, new CMwFieldInfo("MostNetted", 0x0000002D) },
		                { 0x01A, new CMwFieldInfo("GetChallengeStats", 0x00000005) },
		                { 0x01B, new CMwFieldInfo("CurTotalPlayTime", 0x0000001F) },
		                { 0x01C, new CMwFieldInfo("CurEditPlayTime", 0x0000001F) },
		                { 0x01D, new CMwFieldInfo("CurRacePlayTime", 0x0000001F) },
		                { 0x01E, new CMwFieldInfo("CurNetPlayTime", 0x0000001F) },
		                { 0x01F, new CMwFieldInfo("CurResetCount", 0x0000001F) },
		                { 0x020, new CMwFieldInfo("CurFinishCount", 0x0000001F) },
		                { 0x021, new CMwFieldInfo("GlobalSkillPoints", 0x0000001F) }
	                  })
	                },
	                { 0x08E, new CMwClassInfo("CGameLeague", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("Path", 0x0000002D) },
		                { 0x001, new CMwFieldInfo("Name", 0x0000002D) },
		                { 0x002, new CMwFieldInfo("Description", 0x0000002D) },
		                { 0x003, new CMwFieldInfo("Login", 0x00000029) },
		                { 0x004, new CMwFieldInfo("OnlinePlayersCount", 0x0000001F) },
		                { 0x005, new CMwFieldInfo("OnlineHostsCount", 0x0000001F) },
		                { 0x006, new CMwEnumInfo("MaxServerLevel", new string[] { "Green", "Yellow", "Red" }) },
		                { 0x007, new CMwFieldInfo("IsGroup", 0x00000001) },
		                { 0x008, new CMwFieldInfo("Logo", 0x00000005) }
	                  })
	                },
	                { 0x08F, new CMwClassInfo("CGameCtnChallengeGroup", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("Name", 0x0000002D) },
		                { 0x001, new CMwEnumInfo("Medal", new string[] { "None", "Finished", "Bronze", "Silver", "Gold", "Nadeo" }) },
		                { 0x002, new CMwFieldInfo("ChallengeInfos", 0x00000007) },
		                { 0x003, new CMwFieldInfo("Unlocked", 0x00000001) },
		                { 0x004, new CMwMethodInfo("UnlockChallenges", 0x00000000, null, null) },
		                { 0x005, new CMwMethodInfo("CleanChallenges", 0x00000000, null, null) },
		                { 0x006, new CMwMethodInfo("EmptyChallenges", 0x00000000, null, null) }
	                  })
	                },
	                { 0x090, new CMwClassInfo("CGameCtnCampaign", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("Id", 0x00000029) },
		                { 0x001, new CMwFieldInfo("CollectionId", 0x00000029) },
		                { 0x002, new CMwFieldInfo("IconId", 0x00000029) },
		                { 0x003, new CMwFieldInfo("Name", 0x0000002D) },
		                { 0x004, new CMwEnumInfo("Type", new string[] { "None", "Race", "Puzzle", "Survival", "Platform", "Stunts", "Training" }) },
		                { 0x005, new CMwFieldInfo("Index", 0x0000001F) },
		                { 0x006, new CMwFieldInfo("UnlockType", 0x0000001F) },
		                { 0x007, new CMwFieldInfo("IsInternal", 0x00000001) },
		                { 0x008, new CMwFieldInfo("UnlockedByCampaign", 0x0000002D) },
		                { 0x009, new CMwFieldInfo("NbMedals", 0x0000001F) },
		                { 0x00A, new CMwFieldInfo("NbBronzeMedals", 0x0000001F) },
		                { 0x00B, new CMwFieldInfo("NbSilverMedals", 0x0000001F) },
		                { 0x00C, new CMwFieldInfo("NbGoldMedals", 0x0000001F) },
		                { 0x00D, new CMwFieldInfo("NbAuthorMedals", 0x0000001F) },
		                { 0x00E, new CMwEnumInfo("Medal", new string[] { "None", "Finished", "Bronze", "Silver", "Gold", "Nadeo" }) },
		                { 0x00F, new CMwFieldInfo("ChallengeGroups", 0x00000007) },
		                { 0x010, new CMwMethodInfo("AddChallengeGroup", 0x00000000, null, null) }
	                  })
	                },
	                { 0x091, new CMwClassInfo("CGameCtnGhostInfo", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("Time", 0x0000001F) },
		                { 0x001, new CMwFieldInfo("StuntsScore", 0x0000001F) },
		                { 0x002, new CMwFieldInfo("Login", 0x00000029) },
		                { 0x003, new CMwFieldInfo("Nickname", 0x0000002D) },
		                { 0x004, new CMwFieldInfo("ReplayRecordIndex", 0x0000001F) }
	                  })
	                },
	                { 0x092, new CMwClassInfo("CGameCtnGhost", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwMethodInfo("StartRecord", 0x00000000, null, null) },
		                { 0x001, new CMwMethodInfo("StopRecord", 0x00000000, null, null) },
		                { 0x002, new CMwMethodInfo("StartReplay", 0x00000000, null, null) },
		                { 0x003, new CMwMethodInfo("StopReplay", 0x00000000, null, null) },
		                { 0x004, new CMwFieldInfo("EventsDuration", 0x0000001F) },
		                { 0x005, new CMwFieldInfo("SkinPackDescs", 0x00000007) },
		                { 0x006, new CMwFieldInfo("GhostLogin", 0x00000029) },
		                { 0x007, new CMwFieldInfo("GhostNickname", 0x0000002D) },
		                { 0x008, new CMwFieldInfo("GhostAvatarName", 0x0000002D) },
		                { 0x009, new CMwFieldInfo("PlayerMobilId", 0x0000001B) },
		                { 0x00A, new CMwFieldInfo("RaceTime", 0x0000001F) },
		                { 0x00B, new CMwFieldInfo("NbRespawns", 0x0000001F) },
		                { 0x00C, new CMwFieldInfo("StuntsScore", 0x0000001F) },
		                { 0x00D, new CMwFieldInfo("LightTrailColor", 0x00000009) },
		                { 0x00E, new CMwFieldInfo("Validate_Version", 0x00000029) },
		                { 0x00F, new CMwFieldInfo("Validate_RaceSettings", 0x00000029) },
		                { 0x010, new CMwFieldInfo("Validate_ExeChecksum", 0x0000001F) },
		                { 0x011, new CMwFieldInfo("Validate_OsKind", 0x0000001F) },
		                { 0x012, new CMwFieldInfo("Validate_CpuKind", 0x0000001F) }
	                  })
	                },
	                { 0x093, new CMwClassInfo("CGameCtnReplayRecord", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("Ghosts", 0x00000007) },
		                { 0x001, new CMwFieldInfo("Challenge", 0x00000005) },
		                { 0x002, new CMwFieldInfo("Duration", 0x0000001F) },
		                { 0x003, new CMwMethodInfo("StopRecord", 0x00000000, null, null) },
		                { 0x004, new CMwFieldInfo("HumanTimeToGameTimeFunc", 0x00000005) }
	                  })
	                },
	                { 0x094, new CMwClassInfo("CGameCtnReplayRecordInfo", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("ChallengeId", 0x00000029) },
		                { 0x001, new CMwFieldInfo("BestTime", 0x0000001F) },
		                { 0x002, new CMwFieldInfo("PlayerNickname", 0x0000002D) },
		                { 0x003, new CMwFieldInfo("PlayerLogin", 0x00000029) }
	                  })
	                },
	                { 0x095, new CMwClassInfo("CGamePlayerOfficialScores", new Dictionary<int, CMwMemberInfo>()
	                  {
	                  })
	                },
	                { 0x096, new CMwClassInfo("CGameLadderRankingLeague", new Dictionary<int, CMwMemberInfo>()
	                  {
	                  })
	                },
	                { 0x097, new CMwClassInfo("CGameLadderRankingPlayer", new Dictionary<int, CMwMemberInfo>()
	                  {
	                  })
	                },
	                { 0x099, new CMwClassInfo("CGameLadderRankingSkill", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("DynamicLeagueName", 0x0000002D) },
		                { 0x001, new CMwFieldInfo("DynamicLeagueLogo", 0x00000005) }
	                  })
	                },
	                { 0x09A, new CMwClassInfo("CGameControlCard", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("DisplayedNod", 0x00000005) },
		                { 0x001, new CMwMethodInfo("ForceReconfig", 0x00000000, null, null) },
		                { 0x002, new CMwMethodInfo("ConnectChilds", 0x00000000, null, null) },
		                { 0x003, new CMwMethodInfo("DisconnectChilds", 0x00000000, null, null) },
		                { 0x004, new CMwFieldInfo("UseDelays", 0x00000001) },
		                { 0x005, new CMwFieldInfo("UseOwnData", 0x00000001) },
		                { 0x006, new CMwFieldInfo("SelectionEnabled", 0x00000001) },
		                { 0x007, new CMwFieldInfo("CardFocused", 0x00000001) },
		                { 0x008, new CMwFieldInfo("CardSelected", 0x00000001) }
	                  })
	                },
	                { 0x09B, new CMwClassInfo("CGameControlCardManager", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("DataTypes", 0x00000007) }
	                  })
	                },
	                { 0x09C, new CMwClassInfo("CGameControlDataType", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("Name", 0x00000029) },
		                { 0x001, new CMwFieldInfo("CardTemplate", 0x00000005) }
	                  })
	                },
	                { 0x09F, new CMwClassInfo("CGameCtnMediaBlockCameraSimple", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("KeyCache", 0x0000001F) }
	                  })
	                },
	                { 0x0A0, new CMwClassInfo("CGameCtnMediaBlockCameraOrbital", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("BaseLocation", 0x00000013) }
	                  })
	                },
	                { 0x0A1, new CMwClassInfo("CGameCtnMediaBlockCameraPath", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("IsPathDirty", 0x00000001) },
		                { 0x001, new CMwFieldInfo("Path", 0x00000005) },
		                { 0x002, new CMwFieldInfo("Lengths", 0x00000025) },
		                { 0x003, new CMwFieldInfo("CurveLength", 0x00000024) },
		                { 0x004, new CMwFieldInfo("TotalWeight", 0x00000024) }
	                  })
	                },
	                { 0x0A2, new CMwClassInfo("CGameCtnMediaBlockCameraCustom", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("KeyCache", 0x0000001F) }
	                  })
	                },
	                { 0x0A3, new CMwClassInfo("CGameCtnMediaBlockCameraEffect", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("Effect", 0x00000005) }
	                  })
	                },
	                { 0x0A4, new CMwClassInfo("CGameCtnMediaBlockCameraEffectShake", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("KeyCache", 0x0000001F) }
	                  })
	                },
	                { 0x0A5, new CMwClassInfo("CGameCtnMediaBlockImage", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("Bitmap", 0x00000005) },
		                { 0x001, new CMwFieldInfo("Effect", 0x00000005) },
		                { 0x002, new CMwFieldInfo("Mobil", 0x00000005) }
	                  })
	                },
	                { 0x0A6, new CMwClassInfo("CGameCtnMediaBlockMusicEffect", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("Dummy", 0x0000001F) }
	                  })
	                },
	                { 0x0A7, new CMwClassInfo("CGameCtnMediaBlockSound", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("Sound", 0x00000005) },
		                { 0x001, new CMwFieldInfo("AudioSound", 0x00000005) },
		                { 0x002, new CMwFieldInfo("IsBlockPlaying", 0x00000001) },
		                { 0x003, new CMwFieldInfo("IsLooping", 0x00000001) },
		                { 0x004, new CMwFieldInfo("IsMusic", 0x00000001) },
		                { 0x005, new CMwFieldInfo("PlayCount", 0x0000001F) },
		                { 0x006, new CMwMethodInfo("OnParamsModified", 0x00000000, null, null) }
	                  })
	                },
	                { 0x0A8, new CMwClassInfo("CGameCtnMediaBlockText", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("Text", 0x0000002D) },
		                { 0x001, new CMwFieldInfo("Mobil", 0x00000005) },
		                { 0x002, new CMwFieldInfo("Effect", 0x00000005) }
	                  })
	                },
	                { 0x0A9, new CMwClassInfo("CGameCtnMediaBlockTrails", new Dictionary<int, CMwMemberInfo>()
	                  {
	                  })
	                },
	                { 0x0AA, new CMwClassInfo("CGameCtnMediaBlockTransition", new Dictionary<int, CMwMemberInfo>()
	                  {
	                  })
	                },
	                { 0x0AB, new CMwClassInfo("CGameCtnMediaBlockTransitionFade", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("Mobil", 0x00000005) },
		                { 0x001, new CMwFieldInfo("Color", 0x00000009) }
	                  })
	                },
	                { 0x0AC, new CMwClassInfo("CGameCtnMediaBlockUiSimpleEvtsDisplay", new Dictionary<int, CMwMemberInfo>()
	                  {
	                  })
	                },
	                { 0x0AD, new CMwClassInfo("CGameCtnMediaClipViewer", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("IsInput", 0x00000001) },
		                { 0x001, new CMwEnumInfo("ViewerStatus", new string[] { "Running", "Exit", "Next" }) },
		                { 0x002, new CMwFieldInfo("Clip", 0x00000005) },
		                { 0x003, new CMwFieldInfo("ClipPlayer", 0x00000005) },
		                { 0x004, new CMwFieldInfo("PlaySpeed", 0x00000024) },
		                { 0x005, new CMwFieldInfo("ClipGroup", 0x00000005) },
		                { 0x006, new CMwFieldInfo("ClipGroupClip", 0x00000005) },
		                { 0x007, new CMwFieldInfo("ClipGroupPlayer", 0x00000005) },
		                { 0x008, new CMwFieldInfo("ClipGroupIndex", 0x0000001F) },
		                { 0x009, new CMwFieldInfo("ClipGroupStartTime", 0x00000024) }
	                  })
	                },
	                { 0x0AE, new CMwClassInfo("CGameCtnCursor", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("CoordX", 0x0000001F) },
		                { 0x001, new CMwFieldInfo("CoordY", 0x0000001F) },
		                { 0x002, new CMwFieldInfo("CoordZ", 0x0000001F) },
		                { 0x003, new CMwFieldInfo("CursorMobil", 0x00000005) },
		                { 0x004, new CMwFieldInfo("BlockMobil", 0x00000005) },
		                { 0x005, new CMwFieldInfo("BlockHelperMobil", 0x00000005) },
		                { 0x006, new CMwFieldInfo("CanPlaceColor", 0x00000009) },
		                { 0x007, new CMwFieldInfo("CanJoinColor", 0x00000009) },
		                { 0x008, new CMwFieldInfo("CannotPlaceNorJoinColor", 0x00000009) },
		                { 0x009, new CMwFieldInfo("NothingToDoColor", 0x00000009) }
	                  })
	                },
	                { 0x0AF, new CMwClassInfo("CGameCtnEditor", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("MouseMoveDist", 0x00000024) }
	                  })
	                },
	                { 0x0B1, new CMwClassInfo("CGameCtnMediaTracker", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwEnumInfo("EditionMode", new string[] { "|MediaTrack|Camera", "|MediaTrack|ReplayFx" }) },
		                { 0x001, new CMwFieldInfo("MediaClipPlayer", 0x00000005) },
		                { 0x002, new CMwFieldInfo("SafeFrame", 0x00000005) },
		                { 0x003, new CMwFieldInfo("IsDisplayPlayerNames", 0x00000001) },
		                { 0x004, new CMwFieldInfo("ControlCamFree", 0x00000005) },
		                { 0x005, new CMwFieldInfo("BlockEditor", 0x00000005) },
		                { 0x006, new CMwFieldInfo("TriggerMobil", 0x00000005) },
		                { 0x007, new CMwMethodInfo("OnListTrackPagePrev", 0x00000000, null, null) },
		                { 0x008, new CMwMethodInfo("OnListTrackPageNext", 0x00000000, null, null) },
		                { 0x009, new CMwMethodInfo("ButShootVideo", 0x00000000, null, null) },
		                { 0x00A, new CMwMethodInfo("ButShootScreen", 0x00000000, null, null) },
		                { 0x00B, new CMwMethodInfo("ButPreview", 0x00000000, null, null) },
		                { 0x00C, new CMwMethodInfo("But3dPreview", 0x00000000, null, null) },
		                { 0x00D, new CMwMethodInfo("ButSave", 0x00000000, null, null) },
		                { 0x00E, new CMwMethodInfo("ButExportClip", 0x00000000, null, null) },
		                { 0x00F, new CMwMethodInfo("ButExportClip_OnOk", 0x00000000, null, null) },
		                { 0x010, new CMwMethodInfo("ButImportClip", 0x00000000, null, null) },
		                { 0x011, new CMwMethodInfo("ButImportClip_OnOk", 0x00000000, null, null) },
		                { 0x012, new CMwMethodInfo("ButImportGhosts", 0x00000000, null, null) },
		                { 0x013, new CMwMethodInfo("ButImportGhosts_OnOk", 0x00000000, null, null) },
		                { 0x014, new CMwFieldInfo("OrientWeight", 0x00000024) },
		                { 0x015, new CMwFieldInfo("OrientWeightDist", 0x00000024) },
		                { 0x016, new CMwFieldInfo("ReplayTime", 0x0000001F) },
		                { 0x017, new CMwFieldInfo("BlockStart", 0x0000001F) },
		                { 0x018, new CMwFieldInfo("BlockEnd", 0x0000001F) },
		                { 0x019, new CMwMethodInfo("ButPrevFrame", 0x00000000, null, null) },
		                { 0x01A, new CMwMethodInfo("ButNextFrame", 0x00000000, null, null) },
		                { 0x01B, new CMwMethodInfo("ButAddClip", 0x00000000, null, null) },
		                { 0x01C, new CMwMethodInfo("ButRemoveClip", 0x00000000, null, null) },
		                { 0x01D, new CMwMethodInfo("ButRemoveClip_Ok", 0x00000000, null, null) },
		                { 0x01E, new CMwMethodInfo("ButTriggerMode", 0x00000000, null, null) },
		                { 0x01F, new CMwMethodInfo("ButPreviewClipGroup", 0x00000000, null, null) },
		                { 0x020, new CMwFieldInfo("MediaTrackerGhostRefIndex", 0x0000001F) },
		                { 0x021, new CMwFieldInfo("MediaTrackerGhosts", 0x0000001F) },
		                { 0x022, new CMwMethodInfo("ButClipCam", 0x00000000, null, null) },
		                { 0x023, new CMwMethodInfo("ButFreeCam", 0x00000000, null, null) },
		                { 0x024, new CMwFieldInfo("ButIsLooping", 0x00000001) },
		                { 0x025, new CMwFieldInfo("ClipKeepPlaying", 0x00000001) },
		                { 0x026, new CMwEnumInfo("ClipTriggerCond", new string[] { "None", "RaceTime <", "RaceTime >", "Already Triggered", "Speed <", "Speed >", "Not Already Triggered" }) },
		                { 0x027, new CMwFieldInfo("ClipTriggerCondValue", 0x00000024) },
		                { 0x028, new CMwFieldInfo("TrackKeepPlaying", 0x00000001) },
		                { 0x029, new CMwMethodInfo("ButAddTrack", 0x00000000, null, null) },
		                { 0x02A, new CMwMethodInfo("ButRemoveTrack", 0x00000000, null, null) },
		                { 0x02B, new CMwMethodInfo("ButRemoveTrack_Ok", 0x00000000, null, null) },
		                { 0x02C, new CMwMethodInfo("ButFirstFrame", 0x00000000, null, null) },
		                { 0x02D, new CMwMethodInfo("ButStop", 0x00000000, null, null) },
		                { 0x02E, new CMwMethodInfo("ButSlowForward", 0x00000000, null, null) },
		                { 0x02F, new CMwMethodInfo("ButNormalForward", 0x00000000, null, null) },
		                { 0x030, new CMwMethodInfo("ButPlay", 0x00000000, null, null) },
		                { 0x031, new CMwMethodInfo("ButLastFrame", 0x00000000, null, null) },
		                { 0x032, new CMwMethodInfo("ButShowFullTimeLine", 0x00000000, null, null) },
		                { 0x033, new CMwMethodInfo("ButInsertKey", 0x00000000, null, null) },
		                { 0x034, new CMwMethodInfo("ButRemoveKey", 0x00000000, null, null) },
		                { 0x035, new CMwMethodInfo("ButRemoveAllBlockKeys", 0x00000000, null, null) },
		                { 0x036, new CMwMethodInfo("ButSmoothSpeed", 0x00000000, null, null) },
		                { 0x037, new CMwMethodInfo("ButInsertBlock", 0x00000000, null, null) },
		                { 0x038, new CMwMethodInfo("ButRemoveBlock", 0x00000000, null, null) },
		                { 0x039, new CMwMethodInfo("ButRemoveBlock_Ok", 0x00000000, null, null) },
		                { 0x03A, new CMwFieldInfo("EntryPlaySpeed", 0x00000024) },
		                { 0x03B, new CMwMethodInfo("ButCutBlock", 0x00000000, null, null) },
		                { 0x03C, new CMwMethodInfo("ButResetRoll", 0x00000000, null, null) },
		                { 0x03D, new CMwMethodInfo("ButResetDir", 0x00000000, null, null) },
		                { 0x03E, new CMwMethodInfo("ButMoveFaster", 0x00000000, null, null) },
		                { 0x03F, new CMwMethodInfo("ButMoveSlower", 0x00000000, null, null) },
		                { 0x040, new CMwMethodInfo("ButCamCustomAdvancedParams", 0x00000000, null, null) },
		                { 0x041, new CMwMethodInfo("ButCamPathAdvancedParams", 0x00000000, null, null) },
		                { 0x042, new CMwMethodInfo("ButExit", 0x00000000, null, null) },
		                { 0x043, new CMwMethodInfo("ButSwitchInterface", 0x00000000, null, null) },
		                { 0x044, new CMwFieldInfo("ControlSimi2", 0x00000005) },
		                { 0x045, new CMwFieldInfo("Clips", 0x00000007) },
		                { 0x046, new CMwFieldInfo("Tracks", 0x00000007) },
		                { 0x047, new CMwFieldInfo("SelTrack", 0x00000005) },
		                { 0x048, new CMwFieldInfo("TrackName", 0x0000002D) },
		                { 0x049, new CMwFieldInfo("TrackText", 0x0000002D) },
		                { 0x04A, new CMwFieldInfo("TextPosX", 0x00000024) },
		                { 0x04B, new CMwFieldInfo("TextPosY", 0x00000024) },
		                { 0x04C, new CMwFieldInfo("TextDepth", 0x00000028) },
		                { 0x04D, new CMwFieldInfo("TextRot", 0x00000024) },
		                { 0x04E, new CMwFieldInfo("TextScaleX", 0x00000024) },
		                { 0x04F, new CMwFieldInfo("TextScaleY", 0x00000024) },
		                { 0x050, new CMwFieldInfo("TextOpacity", 0x00000028) },
		                { 0x051, new CMwFieldInfo("TrackImage", 0x0000002D) },
		                { 0x052, new CMwFieldInfo("ImagePosX", 0x00000024) },
		                { 0x053, new CMwFieldInfo("ImagePosY", 0x00000024) },
		                { 0x054, new CMwFieldInfo("ImageDepth", 0x00000028) },
		                { 0x055, new CMwFieldInfo("ImageRot", 0x00000024) },
		                { 0x056, new CMwFieldInfo("ImageScaleX", 0x00000024) },
		                { 0x057, new CMwFieldInfo("ImageScaleY", 0x00000024) },
		                { 0x058, new CMwFieldInfo("ImageOpacity", 0x00000028) },
		                { 0x059, new CMwMethodInfo("ButChooseImage", 0x00000000, null, null) },
		                { 0x05A, new CMwMethodInfo("ButChooseSound", 0x00000000, null, null) },
		                { 0x05B, new CMwFieldInfo("TransFadeOpacity", 0x00000024) },
		                { 0x05C, new CMwFieldInfo("CamPosXCamCustom", 0x00000024) },
		                { 0x05D, new CMwFieldInfo("CamPosYCamCustom", 0x00000024) },
		                { 0x05E, new CMwFieldInfo("CamPosZCamCustom", 0x00000024) },
		                { 0x05F, new CMwFieldInfo("CamPitchCamCustom", 0x00000024) },
		                { 0x060, new CMwFieldInfo("CamYawCamCustom", 0x00000024) },
		                { 0x061, new CMwFieldInfo("CamRollCamCustom", 0x00000024) },
		                { 0x062, new CMwFieldInfo("CamFovCamCustom", 0x00000028) },
		                { 0x063, new CMwFieldInfo("CamAnchorCamCustom", 0x0000001F) },
		                { 0x064, new CMwFieldInfo("CamAnchorNameCamCustom", 0x0000002D) },
		                { 0x065, new CMwFieldInfo("CamIsAnchorVisibleCamCustom", 0x00000001) },
		                { 0x066, new CMwFieldInfo("CamUseAnchorOrientationCamCustom", 0x00000001) },
		                { 0x067, new CMwFieldInfo("CamTargetCamCustom", 0x0000001F) },
		                { 0x068, new CMwFieldInfo("CamTargetNameCamCustom", 0x0000002D) },
		                { 0x069, new CMwFieldInfo("CamTargetPosXCamCustom", 0x00000024) },
		                { 0x06A, new CMwFieldInfo("CamTargetPosYCamCustom", 0x00000024) },
		                { 0x06B, new CMwFieldInfo("CamTargetPosZCamCustom", 0x00000024) },
		                { 0x06C, new CMwEnumInfo("CamInterpCamCustom", new string[] { "|CameraInterp|None", "|CameraInterp|Hermite", "|CameraInterp|Linear", "|CameraInterp|FixedTangent" }) },
		                { 0x06D, new CMwFieldInfo("CamLeftTangentXCamCustom", 0x00000024) },
		                { 0x06E, new CMwFieldInfo("CamLeftTangentYCamCustom", 0x00000024) },
		                { 0x06F, new CMwFieldInfo("CamLeftTangentZCamCustom", 0x00000024) },
		                { 0x070, new CMwFieldInfo("CamRightTangentXCamCustom", 0x00000024) },
		                { 0x071, new CMwFieldInfo("CamRightTangentYCamCustom", 0x00000024) },
		                { 0x072, new CMwFieldInfo("CamRightTangentZCamCustom", 0x00000024) },
		                { 0x073, new CMwFieldInfo("CamPosXCamPath", 0x00000024) },
		                { 0x074, new CMwFieldInfo("CamPosYCamPath", 0x00000024) },
		                { 0x075, new CMwFieldInfo("CamPosZCamPath", 0x00000024) },
		                { 0x076, new CMwFieldInfo("CamPitchCamPath", 0x00000024) },
		                { 0x077, new CMwFieldInfo("CamYawCamPath", 0x00000024) },
		                { 0x078, new CMwFieldInfo("CamRollCamPath", 0x00000024) },
		                { 0x079, new CMwFieldInfo("CamFovCamPath", 0x00000028) },
		                { 0x07A, new CMwFieldInfo("CamAnchorCamPath", 0x0000001F) },
		                { 0x07B, new CMwFieldInfo("CamAnchorNameCamPath", 0x0000002D) },
		                { 0x07C, new CMwFieldInfo("CamWeightCamPath", 0x00000024) },
		                { 0x07D, new CMwFieldInfo("CamIsAnchorVisibleCamPath", 0x00000001) },
		                { 0x07E, new CMwFieldInfo("CamUseAnchorOrientationCamPath", 0x00000001) },
		                { 0x07F, new CMwFieldInfo("CamTargetCamPath", 0x0000001F) },
		                { 0x080, new CMwFieldInfo("CamTargetNameCamPath", 0x0000002D) },
		                { 0x081, new CMwFieldInfo("CamTargetPosXCamPath", 0x00000024) },
		                { 0x082, new CMwFieldInfo("CamTargetPosYCamPath", 0x00000024) },
		                { 0x083, new CMwFieldInfo("CamTargetPosZCamPath", 0x00000024) },
		                { 0x084, new CMwMethodInfo("ButCamAnchorPrev", 0x00000000, null, null) },
		                { 0x085, new CMwMethodInfo("ButCamAnchorNext", 0x00000000, null, null) },
		                { 0x086, new CMwMethodInfo("ButCamTargetPrev", 0x00000000, null, null) },
		                { 0x087, new CMwMethodInfo("ButCamTargetNext", 0x00000000, null, null) },
		                { 0x088, new CMwFieldInfo("CamGameCur", 0x0000001B) },
		                { 0x089, new CMwMethodInfo("ButCamGamePrev", 0x00000000, null, null) },
		                { 0x08A, new CMwMethodInfo("ButCamGameNext", 0x00000000, null, null) },
		                { 0x08B, new CMwFieldInfo("CamGameTarget", 0x0000001F) },
		                { 0x08C, new CMwFieldInfo("CamGameTargetName", 0x0000002D) },
		                { 0x08D, new CMwMethodInfo("ButCamGameTargetPrev", 0x00000000, null, null) },
		                { 0x08E, new CMwMethodInfo("ButCamGameTargetNext", 0x00000000, null, null) },
		                { 0x08F, new CMwFieldInfo("TimeValue", 0x00000024) },
		                { 0x090, new CMwFieldInfo("TimeTangent", 0x00000024) },
		                { 0x091, new CMwFieldInfo("FxColorsFxIntensity", 0x00000028) },
		                { 0x092, new CMwEnumInfo("FxColorsZMode", new string[] { "|FxColors|Near", "|FxColors|Far" }) },
		                { 0x093, new CMwFieldInfo("FxColorsZ_Near", 0x00000024) },
		                { 0x094, new CMwFieldInfo("FxColorsInverseRGB", 0x00000028) },
		                { 0x095, new CMwFieldInfo("FxColorsHue", 0x00000028) },
		                { 0x096, new CMwFieldInfo("FxColorsSaturation", 0x00000028) },
		                { 0x097, new CMwFieldInfo("FxColorsBrightness", 0x00000028) },
		                { 0x098, new CMwFieldInfo("FxColorsContrast", 0x00000028) },
		                { 0x099, new CMwFieldInfo("FxColorsModulateR", 0x00000028) },
		                { 0x09A, new CMwFieldInfo("FxColorsModulateG", 0x00000028) },
		                { 0x09B, new CMwFieldInfo("FxColorsModulateB", 0x00000028) },
		                { 0x09C, new CMwFieldInfo("FxColorsIntensityF", 0x00000028) },
		                { 0x09D, new CMwFieldInfo("FxColorsZ_Far", 0x00000024) },
		                { 0x09E, new CMwFieldInfo("FxColorsInverseRGBF", 0x00000028) },
		                { 0x09F, new CMwFieldInfo("FxColorsHueF", 0x00000028) },
		                { 0x0A0, new CMwFieldInfo("FxColorsSaturationF", 0x00000028) },
		                { 0x0A1, new CMwFieldInfo("FxColorsBrightnessF", 0x00000028) },
		                { 0x0A2, new CMwFieldInfo("FxColorsContrastF", 0x00000028) },
		                { 0x0A3, new CMwFieldInfo("FxColorsModulateRF", 0x00000028) },
		                { 0x0A4, new CMwFieldInfo("FxColorsModulateGF", 0x00000028) },
		                { 0x0A5, new CMwFieldInfo("FxColorsModulateBF", 0x00000028) },
		                { 0x0A6, new CMwFieldInfo("FxBlurDepthLensSize", 0x00000024) },
		                { 0x0A7, new CMwFieldInfo("FxBlurDepthForceZ", 0x00000001) },
		                { 0x0A8, new CMwFieldInfo("FxBlurDepthFocusZ", 0x00000024) },
		                { 0x0A9, new CMwFieldInfo("FxBloomFxIntensity", 0x00000024) },
		                { 0x0AA, new CMwFieldInfo("FxBloomSensitivity", 0x00000024) },
		                { 0x0AB, new CMwFieldInfo("CamFxShakeIntensity", 0x00000024) },
		                { 0x0AC, new CMwFieldInfo("CamFxShakeSpeed", 0x00000024) },
		                { 0x0AD, new CMwFieldInfo("StereoSeparation", 0x00000024) },
		                { 0x0AE, new CMwFieldInfo("StereoScreenDist", 0x00000024) },
		                { 0x0AF, new CMwFieldInfo("TrackSound", 0x0000002D) },
		                { 0x0B0, new CMwFieldInfo("SoundIsMusic", 0x00000001) },
		                { 0x0B1, new CMwFieldInfo("SoundVolume", 0x00000028) },
		                { 0x0B2, new CMwFieldInfo("SoundPan", 0x00000028) },
		                { 0x0B3, new CMwFieldInfo("SoundPos", 0x00000035) },
		                { 0x0B4, new CMwFieldInfo("SoundLooping", 0x00000001) },
		                { 0x0B5, new CMwFieldInfo("SoundPlayCount", 0x0000001F) },
		                { 0x0B6, new CMwFieldInfo("MusicVolume", 0x00000028) },
		                { 0x0B7, new CMwFieldInfo("GlobalSoundVolume", 0x00000028) },
		                { 0x0B8, new CMwFieldInfo("GhostName", 0x0000002D) },
		                { 0x0B9, new CMwFieldInfo("GhostSkin", 0x0000002D) },
		                { 0x0BA, new CMwFieldInfo("GhostRaceTime", 0x0000001F) },
		                { 0x0BB, new CMwFieldInfo("GhostStartOffset", 0x00000024) },
		                { 0x0BC, new CMwMethodInfo("ButGhostSkinSelect", 0x00000000, null, null) },
		                { 0x0BD, new CMwFieldInfo("RenderHq", 0x00000001) },
		                { 0x0BE, new CMwFieldInfo("RenderHqSoftShadows", 0x00000001) },
		                { 0x0BF, new CMwFieldInfo("RenderHqCountSS", 0x0000001F) },
		                { 0x0C0, new CMwMethodInfo("ButFrameKeyAdvanced", 0x00000000, null, null) }
	                  })
	                },
	                { 0x0B2, new CMwClassInfo("CGamePopUp", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwMethodInfo("OnSelect", 0x00000000, null, null) },
		                { 0x001, new CMwMethodInfo("OnCancel", 0x00000000, null, null) }
	                  })
	                },
	                { 0x0B3, new CMwClassInfo("CGameCtnEdControlCam", new Dictionary<int, CMwMemberInfo>()
	                  {
	                  })
	                },
	                { 0x0B4, new CMwClassInfo("CGameCtnEdControlCamCustom", new Dictionary<int, CMwMemberInfo>()
	                  {
	                  })
	                },
	                { 0x0B5, new CMwClassInfo("CGameCtnEdControlCamPath", new Dictionary<int, CMwMemberInfo>()
	                  {
	                  })
	                },
	                { 0x0B6, new CMwClassInfo("CGameSafeFrame", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("SafeFrameSizeX", 0x0000001F) },
		                { 0x001, new CMwFieldInfo("SafeFrameSizeY", 0x0000001F) },
		                { 0x002, new CMwFieldInfo("Zoom", 0x00000024) },
		                { 0x003, new CMwFieldInfo("Position", 0x00000031) },
		                { 0x004, new CMwFieldInfo("AutoZoom", 0x00000001) }
	                  })
	                },
	                { 0x0B7, new CMwClassInfo("CGameSafeFrameConfig", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("Color", 0x00000039) },
		                { 0x001, new CMwFieldInfo("LinesColor", 0x00000039) }
	                  })
	                },
	                { 0x0B8, new CMwClassInfo("CGameCtnPainter", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwEnumInfo("PainterMode", new string[] { "Fill", "Brush", "Sticker", "Layer" }) },
		                { 0x001, new CMwFieldInfo("VehicleMobil", 0x00000005) },
		                { 0x002, new CMwFieldInfo("ControlCameraOrbital3d", 0x00000005) },
		                { 0x003, new CMwFieldInfo("PaintColor", 0x00000009) },
		                { 0x004, new CMwFieldInfo("PaintSolidShader", 0x00000005) },
		                { 0x005, new CMwFieldInfo("PaintSolid", 0x00000005) },
		                { 0x006, new CMwFieldInfo("IconSkinBitmap", 0x00000005) },
		                { 0x007, new CMwFieldInfo("Scale", 0x00000028) },
		                { 0x008, new CMwFieldInfo("Angle", 0x00000028) },
		                { 0x009, new CMwFieldInfo("Shininess", 0x00000028) },
		                { 0x00A, new CMwFieldInfo("Transparency", 0x00000028) },
		                { 0x00B, new CMwFieldInfo("ButtonFillModeEnabled", 0x00000001) },
		                { 0x00C, new CMwFieldInfo("ButtonBrushModeEnabled", 0x00000001) },
		                { 0x00D, new CMwFieldInfo("ButtonStickerModeEnabled", 0x00000001) },
		                { 0x00E, new CMwFieldInfo("ButtonLayerModeEnabled", 0x00000001) },
		                { 0x00F, new CMwFieldInfo("ButtonStickerModeSelected", 0x00000001) },
		                { 0x010, new CMwFieldInfo("ButtonInvertXAxisSelected", 0x00000001) },
		                { 0x011, new CMwFieldInfo("ButtonSubObjectModeSelected", 0x00000001) },
		                { 0x012, new CMwFieldInfo("ButtonPaintSymModeSelected", 0x00000001) },
		                { 0x013, new CMwFieldInfo("ButtonColorPickerModeSelected", 0x00000001) },
		                { 0x014, new CMwMethodInfo("OnButtonSelectedInGrid", 0x00000000, null, null) },
		                { 0x015, new CMwMethodInfo("OnOkInHelpFrame", 0x00000000, null, null) },
		                { 0x016, new CMwMethodInfo("OnButtonCurrentColorClicked", 0x00000000, null, null) },
		                { 0x017, new CMwMethodInfo("OnButtonCustomColor0Clicked", 0x00000000, null, null) },
		                { 0x018, new CMwMethodInfo("OnButtonCustomColor1Clicked", 0x00000000, null, null) },
		                { 0x019, new CMwMethodInfo("OnButtonCustomColor2Clicked", 0x00000000, null, null) },
		                { 0x01A, new CMwMethodInfo("OnButtonCustomColor3Clicked", 0x00000000, null, null) },
		                { 0x01B, new CMwMethodInfo("OnButtonCustomColor4Clicked", 0x00000000, null, null) },
		                { 0x01C, new CMwMethodInfo("DrawLine", 0x00000000, null, null) },
		                { 0x01D, new CMwMethodInfo("HasToAlignSticker", 0x00000000, null, null) },
		                { 0x01E, new CMwMethodInfo("ImageQuarterRotLeft", 0x00000000, null, null) },
		                { 0x01F, new CMwMethodInfo("ImageQuarterRotRight", 0x00000000, null, null) },
		                { 0x020, new CMwMethodInfo("EngageColorChooserMode", 0x00000000, null, null) },
		                { 0x021, new CMwMethodInfo("StepImagesOnLeft", 0x00000000, null, null) },
		                { 0x022, new CMwMethodInfo("StepImagesOnRight", 0x00000000, null, null) },
		                { 0x023, new CMwMethodInfo("FillWithPattern", 0x00000000, null, null) },
		                { 0x024, new CMwMethodInfo("FillWithColor", 0x00000000, null, null) },
		                { 0x025, new CMwMethodInfo("ApplyLayer", 0x00000000, null, null) },
		                { 0x026, new CMwMethodInfo("Undo", 0x00000000, null, null) },
		                { 0x027, new CMwMethodInfo("Redo", 0x00000000, null, null) },
		                { 0x028, new CMwMethodInfo("SaveSkin", 0x00000000, null, null) },
		                { 0x029, new CMwMethodInfo("SaveSkinAs", 0x00000000, null, null) },
		                { 0x02A, new CMwMethodInfo("SaveSkinAs_OnOk", 0x00000000, null, null) },
		                { 0x02B, new CMwMethodInfo("ReloadSkin", 0x00000000, null, null) },
		                { 0x02C, new CMwMethodInfo("WantHelpDialog", 0x00000000, null, null) },
		                { 0x02D, new CMwFieldInfo("ButtonPaintWithTextEnabled", 0x00000001) },
		                { 0x02E, new CMwFieldInfo("TextToCreateBitmap", 0x0000002D) },
		                { 0x02F, new CMwFieldInfo("BitmapText", 0x00000005) }
	                  })
	                },
	                { 0x0B9, new CMwClassInfo("CGameControlGrid", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("MaxPerColumn", 0x0000001F) },
		                { 0x001, new CMwFieldInfo("MaxPerRow", 0x0000001F) },
		                { 0x002, new CMwFieldInfo("FastNextPageCount", 0x0000001F) },
		                { 0x003, new CMwFieldInfo("FastPreviousPageCount", 0x0000001F) },
		                { 0x004, new CMwFieldInfo("ForcedPageCount", 0x0000001F) },
		                { 0x005, new CMwFieldInfo("ForceHideArrows", 0x00000001) },
		                { 0x006, new CMwFieldInfo("ForceHidePageCounter", 0x00000001) },
		                { 0x007, new CMwFieldInfo("HaveLocalData", 0x00000001) },
		                { 0x008, new CMwFieldInfo("HideLocalDataIfNone", 0x00000001) },
		                { 0x009, new CMwFieldInfo("CurrentPage", 0x0000001F) },
		                { 0x00A, new CMwFieldInfo("PageCount", 0x0000001F) },
		                { 0x00B, new CMwMethodInfo("UpdatePageCount", 0x00000000, null, null) },
		                { 0x00C, new CMwMethodInfo("UpdatePageCounter", 0x00000000, null, null) },
		                { 0x00D, new CMwMethodInfo("UpdateNavigationButtons", 0x00000000, null, null) },
		                { 0x00E, new CMwMethodInfo("UpdateLocalData", 0x00000000, null, null) },
		                { 0x00F, new CMwMethodInfo("OnCurrentPageChanged", 0x00000000, null, null) },
		                { 0x010, new CMwFieldInfo("ButtonFirstPage", 0x00000005) },
		                { 0x011, new CMwFieldInfo("ButtonFastPreviousPage", 0x00000005) },
		                { 0x012, new CMwFieldInfo("ButtonPreviousPage", 0x00000005) },
		                { 0x013, new CMwFieldInfo("EntryPageCounter", 0x00000005) },
		                { 0x014, new CMwFieldInfo("ButtonNextPage", 0x00000005) },
		                { 0x015, new CMwFieldInfo("ButtonFastNextPage", 0x00000005) },
		                { 0x016, new CMwFieldInfo("ButtonLastPage", 0x00000005) },
		                { 0x017, new CMwFieldInfo("BaseLocalData", 0x00000005) },
		                { 0x018, new CMwMethodInfo("OnFirstPage", 0x00000000, null, null) },
		                { 0x019, new CMwMethodInfo("OnFastPreviousPage", 0x00000000, null, null) },
		                { 0x01A, new CMwMethodInfo("OnPreviousPage", 0x00000000, null, null) },
		                { 0x01B, new CMwMethodInfo("OnNextPage", 0x00000000, null, null) },
		                { 0x01C, new CMwMethodInfo("OnFastNextPage", 0x00000000, null, null) },
		                { 0x01D, new CMwMethodInfo("OnLastPage", 0x00000000, null, null) },
		                { 0x01E, new CMwFieldInfo("StrPageCounter", 0x00000029) },
		                { 0x01F, new CMwFieldInfo("Remote_TotalCount", 0x0000001F) },
		                { 0x020, new CMwFieldInfo("Remote_SpecificOverTotalCount", 0x00000029) },
		                { 0x021, new CMwFieldInfo("Remote_Pool", 0x00000005) }
	                  })
	                },
	                { 0x0BA, new CMwClassInfo("CGameControlGridCard", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("CardManager", 0x00000005) },
		                { 0x001, new CMwFieldInfo("PushByColumns", 0x00000001) },
		                { 0x002, new CMwFieldInfo("CacheAtCreation", 0x00000001) },
		                { 0x003, new CMwFieldInfo("UseCustomSelection", 0x00000001) },
		                { 0x004, new CMwFieldInfo("FillWithDefault", 0x00000001) },
		                { 0x005, new CMwFieldInfo("DefaultCardName", 0x00000029) },
		                { 0x006, new CMwFieldInfo("NodsToDisplay", 0x00000007) },
		                { 0x007, new CMwFieldInfo("NodCards", 0x00000007) },
		                { 0x008, new CMwMethodInfo("UpdateFromDatas", 0x00000000, null, null) },
		                { 0x009, new CMwMethodInfo("UpdateOnlyCards", 0x00000000, null, null) },
		                { 0x00A, new CMwMethodInfo("CleanCaches", 0x00000000, null, null) }
	                  })
	                },
	                { 0x0BB, new CMwClassInfo("CGameCtnNetServerInfo", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("ServerName", 0x0000002D) },
		                { 0x001, new CMwFieldInfo("HostName", 0x0000002D) },
		                { 0x002, new CMwFieldInfo("PlayerName", 0x0000002D) },
		                { 0x003, new CMwFieldInfo("PlayerCount", 0x0000001F) },
		                { 0x004, new CMwFieldInfo("MaxPlayerCount", 0x0000001F) },
		                { 0x005, new CMwFieldInfo("PlayerCountOverMax", 0x00000029) },
		                { 0x006, new CMwFieldInfo("PlayersLevelMin", 0x00000024) },
		                { 0x007, new CMwFieldInfo("PlayersLevelAvg", 0x00000024) },
		                { 0x008, new CMwFieldInfo("PlayersLevelMax", 0x00000024) },
		                { 0x009, new CMwFieldInfo("ServerLevel", 0x0000001F) },
		                { 0x00A, new CMwFieldInfo("HasBuddies", 0x00000001) },
		                { 0x00B, new CMwFieldInfo("BuddiesCount", 0x00000029) },
		                { 0x00C, new CMwFieldInfo("SpectatorCount", 0x0000001F) },
		                { 0x00D, new CMwFieldInfo("MaxSpectatorCount", 0x0000001F) },
		                { 0x00E, new CMwFieldInfo("NextMaxPlayerCount", 0x0000001F) },
		                { 0x00F, new CMwFieldInfo("NextMaxSpectatorCount", 0x0000001F) },
		                { 0x010, new CMwFieldInfo("ListName", 0x0000002D) },
		                { 0x011, new CMwFieldInfo("IsFavourite", 0x00000001) },
		                { 0x012, new CMwFieldInfo("IsBuddy", 0x00000001) },
		                { 0x013, new CMwFieldInfo("AllowDownload", 0x00000001) },
		                { 0x014, new CMwFieldInfo("Comment", 0x0000002D) },
		                { 0x015, new CMwFieldInfo("HideServer", 0x0000001F) },
		                { 0x016, new CMwFieldInfo("RoundNumber", 0x0000001F) },
		                { 0x017, new CMwEnumInfo("LadderMode", new string[] { "No Ladder", "Forced" }) },
		                { 0x018, new CMwEnumInfo("NextLadderMode", new string[] { "No Ladder", "Forced" }) },
		                { 0x019, new CMwEnumInfo("NextVehicleNetQuality", new string[] { "Low", "High" }) },
		                { 0x01A, new CMwEnumInfo("ValidationMode", new string[] { "|RefereeMode|Top3", "|RefereeMode|All" }) },
		                { 0x01B, new CMwEnumInfo("ValidationAction_Invalid", new string[] { "None", "Log", "Reset score", "Ban" }) },
		                { 0x01C, new CMwEnumInfo("ValidationAction_NA", new string[] { "None", "Log", "Reset score", "Ban" }) },
		                { 0x01D, new CMwFieldInfo("IsWarmUp", 0x00000001) },
		                { 0x01E, new CMwFieldInfo("LadderServerLimitMax", 0x00000024) },
		                { 0x01F, new CMwFieldInfo("LadderServerLimitMin", 0x00000024) }
	                  })
	                },
	                { 0x0BC, new CMwClassInfo("CGameControlCardCtnChallengeInfo", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("Groups", 0x00000007) },
		                { 0x001, new CMwFieldInfo("LeagueNameMaxCharsCount", 0x0000001F) },
		                { 0x002, new CMwFieldInfo("StrFullLeagueName", 0x0000002D) },
		                { 0x003, new CMwFieldInfo("StrPlayerRanking", 0x0000002D) },
		                { 0x004, new CMwFieldInfo("StrPlayerLeagueRanking", 0x0000002D) },
		                { 0x005, new CMwFieldInfo("StrChallengeName", 0x0000002D) },
		                { 0x006, new CMwFieldInfo("StrChallengeAuthor", 0x0000002D) },
		                { 0x007, new CMwFieldInfo("StrChallengeComments", 0x0000002D) },
		                { 0x008, new CMwFieldInfo("StrCollectionName", 0x0000002D) },
		                { 0x009, new CMwFieldInfo("StrCopperPrice", 0x0000002D) },
		                { 0x00A, new CMwFieldInfo("StrPlayerName", 0x0000002D) },
		                { 0x00B, new CMwFieldInfo("StrBronzeScore", 0x0000002D) },
		                { 0x00C, new CMwFieldInfo("StrSilverScore", 0x0000002D) },
		                { 0x00D, new CMwFieldInfo("StrGoldScore", 0x0000002D) },
		                { 0x00E, new CMwFieldInfo("StrAuthorScore", 0x0000002D) },
		                { 0x00F, new CMwFieldInfo("StrPlayerScore", 0x0000002D) },
		                { 0x010, new CMwFieldInfo("StrPlayerOfficialRecord", 0x0000002D) },
		                { 0x011, new CMwFieldInfo("StrCopperString", 0x00000029) },
		                { 0x012, new CMwFieldInfo("StrLeagueName", 0x0000002D) },
		                { 0x013, new CMwFieldInfo("Medals", 0x0000001F) },
		                { 0x014, new CMwFieldInfo("TrainingMedal", 0x0000001F) },
		                { 0x015, new CMwFieldInfo("OfficialMedal", 0x0000001F) },
		                { 0x016, new CMwFieldInfo("Difficulty", 0x0000001F) },
		                { 0x017, new CMwFieldInfo("NextMedalTime", 0x0000001F) },
		                { 0x018, new CMwFieldInfo("PlayerLeagueRanking", 0x0000001F) },
		                { 0x019, new CMwFieldInfo("PlayerSkillScore", 0x0000001F) },
		                { 0x01A, new CMwFieldInfo("BmpLeagueLogo", 0x00000005) },
		                { 0x01B, new CMwFieldInfo("BmpMood", 0x00000005) },
		                { 0x01C, new CMwFieldInfo("BmpMod", 0x00000005) },
		                { 0x01D, new CMwFieldInfo("BmpBannerGrey", 0x00000005) },
		                { 0x01E, new CMwFieldInfo("BmpBanner", 0x00000005) },
		                { 0x01F, new CMwMethodInfo("OnRemoveChallenge", 0x00000000, null, null) }
	                  })
	                },
	                { 0x0BD, new CMwClassInfo("CGameControlCardGeneric", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("Type", 0x0000001F) },
		                { 0x001, new CMwFieldInfo("Str1", 0x00000029) },
		                { 0x002, new CMwFieldInfo("Str2", 0x00000029) },
		                { 0x003, new CMwFieldInfo("Str3", 0x00000029) },
		                { 0x004, new CMwFieldInfo("StrInt1", 0x0000002D) },
		                { 0x005, new CMwFieldInfo("StrInt2", 0x0000002D) },
		                { 0x006, new CMwFieldInfo("StrInt3", 0x0000002D) },
		                { 0x007, new CMwFieldInfo("StrInt4", 0x0000002D) },
		                { 0x008, new CMwFieldInfo("StrInt5", 0x0000002D) },
		                { 0x009, new CMwFieldInfo("StrInt6", 0x0000002D) },
		                { 0x00A, new CMwFieldInfo("StrInt7", 0x0000002D) },
		                { 0x00B, new CMwFieldInfo("Nat1", 0x0000001F) },
		                { 0x00C, new CMwFieldInfo("Nat2", 0x0000001F) },
		                { 0x00D, new CMwFieldInfo("Nat3", 0x0000001F) },
		                { 0x00E, new CMwFieldInfo("Real1", 0x00000024) },
		                { 0x00F, new CMwFieldInfo("Real2", 0x00000024) },
		                { 0x010, new CMwFieldInfo("Real3", 0x00000024) },
		                { 0x011, new CMwFieldInfo("Nod1", 0x00000005) },
		                { 0x012, new CMwFieldInfo("Nod2", 0x00000005) },
		                { 0x013, new CMwFieldInfo("Nod3", 0x00000005) }
	                  })
	                },
	                { 0x0BE, new CMwClassInfo("CGameControlCardLeague", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("StrPath", 0x0000002D) },
		                { 0x001, new CMwFieldInfo("StrName", 0x0000002D) },
		                { 0x002, new CMwFieldInfo("StrDescription", 0x0000002D) },
		                { 0x003, new CMwFieldInfo("StrOnlinePlayersCount", 0x00000029) },
		                { 0x004, new CMwFieldInfo("StrOnlineHostsCount", 0x00000029) },
		                { 0x005, new CMwFieldInfo("StrLogoUrl", 0x00000029) },
		                { 0x006, new CMwFieldInfo("Logo", 0x00000005) },
		                { 0x007, new CMwFieldInfo("MaxLevel", 0x0000001F) }
	                  })
	                },
	                { 0x0BF, new CMwClassInfo("CGameControlCardCtnNetServerInfo", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("StrPath", 0x0000002D) },
		                { 0x001, new CMwFieldInfo("StrName", 0x0000002D) },
		                { 0x002, new CMwFieldInfo("StrPureName", 0x0000002D) },
		                { 0x003, new CMwFieldInfo("StrPlayerName", 0x0000002D) },
		                { 0x004, new CMwFieldInfo("StrServerName", 0x0000002D) },
		                { 0x005, new CMwFieldInfo("StrHostName", 0x0000002D) },
		                { 0x006, new CMwFieldInfo("StrPureServerName", 0x0000002D) },
		                { 0x007, new CMwFieldInfo("StrComment", 0x0000002D) },
		                { 0x008, new CMwFieldInfo("StrPlayersCount", 0x00000029) },
		                { 0x009, new CMwFieldInfo("StrPlayersCountMax", 0x00000029) },
		                { 0x00A, new CMwFieldInfo("StrSpectatorsCount", 0x00000029) },
		                { 0x00B, new CMwFieldInfo("StrSpectatorsCountMax", 0x00000029) },
		                { 0x00C, new CMwFieldInfo("StrBuddiesCount", 0x00000029) },
		                { 0x00D, new CMwFieldInfo("StrValue", 0x00000029) },
		                { 0x00E, new CMwFieldInfo("StrLadderServerLimitMin", 0x0000002D) },
		                { 0x00F, new CMwFieldInfo("StrLadderServerLimitMax", 0x0000002D) },
		                { 0x010, new CMwFieldInfo("LadderMode", 0x0000001F) },
		                { 0x011, new CMwFieldInfo("VehicleQuality", 0x0000001F) },
		                { 0x012, new CMwFieldInfo("RaceType", 0x0000001F) },
		                { 0x013, new CMwFieldInfo("HaveBuddies", 0x00000001) },
		                { 0x014, new CMwFieldInfo("IsBuddy", 0x00000001) },
		                { 0x015, new CMwFieldInfo("IsFull", 0x00000001) },
		                { 0x016, new CMwFieldInfo("IsFullSpectator", 0x00000001) },
		                { 0x017, new CMwFieldInfo("IsAllowingDownload", 0x00000001) },
		                { 0x018, new CMwFieldInfo("IsPrivate", 0x00000001) },
		                { 0x019, new CMwFieldInfo("IsPrivateForSpectator", 0x00000001) },
		                { 0x01A, new CMwFieldInfo("Logo", 0x00000005) },
		                { 0x01B, new CMwFieldInfo("BmpBannerEnv", 0x00000005) },
		                { 0x01C, new CMwFieldInfo("PlayersCountRatio", 0x00000024) },
		                { 0x01D, new CMwFieldInfo("SpectatorsCountRatio", 0x00000024) },
		                { 0x01E, new CMwFieldInfo("ManiaStarsRatio", 0x00000024) },
		                { 0x01F, new CMwFieldInfo("PlayerStars", 0x0000001F) },
		                { 0x020, new CMwFieldInfo("Level", 0x0000001F) },
		                { 0x021, new CMwFieldInfo("State", 0x0000001F) },
		                { 0x022, new CMwFieldInfo("LevelAndLadderMode", 0x0000001F) },
		                { 0x023, new CMwMethodInfo("OnChangeBuddyState", 0x00000000, null, null) },
		                { 0x024, new CMwMethodInfo("OnChangeBuddyState_SetBuddy", 0x00000000, null, null) },
		                { 0x025, new CMwMethodInfo("OnChangeBuddyState_SetNotBuddy", 0x00000000, null, null) },
		                { 0x026, new CMwMethodInfo("OnChangeFavouriteState", 0x00000000, null, null) },
		                { 0x027, new CMwMethodInfo("OnChangeFavouriteState_SetFavourite", 0x00000000, null, null) },
		                { 0x028, new CMwMethodInfo("OnChangeFavouriteState_SetNotFavourite", 0x00000000, null, null) }
	                  })
	                },
	                { 0x0C0, new CMwClassInfo("CGameControlCardNetOnlineNews", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("StrRepliesTotalCount", 0x00000029) },
		                { 0x001, new CMwFieldInfo("StrStartDate", 0x00000029) },
		                { 0x002, new CMwFieldInfo("StrIconUrl", 0x00000029) },
		                { 0x003, new CMwFieldInfo("StrUrlToReply", 0x00000029) },
		                { 0x004, new CMwFieldInfo("StrPath", 0x0000002D) },
		                { 0x005, new CMwFieldInfo("StrTitle", 0x0000002D) },
		                { 0x006, new CMwFieldInfo("StrContent", 0x0000002D) },
		                { 0x007, new CMwFieldInfo("Icon", 0x00000005) },
		                { 0x008, new CMwMethodInfo("OnViewReplies", 0x00000000, null, null) },
		                { 0x009, new CMwMethodInfo("OnEditReply", 0x00000000, null, null) },
		                { 0x00A, new CMwMethodInfo("OnUseManiaCode", 0x00000000, null, null) }
	                  })
	                },
	                { 0x0C1, new CMwClassInfo("CGameControlCardLadderRanking", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("ChildsCount", 0x0000001F) },
		                { 0x001, new CMwFieldInfo("Ranking", 0x0000001F) },
		                { 0x002, new CMwFieldInfo("Ranking2", 0x0000001F) },
		                { 0x003, new CMwFieldInfo("Medals", 0x0000001F) },
		                { 0x004, new CMwFieldInfo("Medals2", 0x0000001F) },
		                { 0x005, new CMwFieldInfo("Score2", 0x0000001F) },
		                { 0x006, new CMwFieldInfo("StrName", 0x0000002D) },
		                { 0x007, new CMwFieldInfo("StrPath", 0x0000002D) },
		                { 0x008, new CMwFieldInfo("StrScore", 0x0000002D) },
		                { 0x009, new CMwFieldInfo("ManiaStarsRatio", 0x00000024) },
		                { 0x00A, new CMwFieldInfo("StrLogin", 0x00000029) },
		                { 0x00B, new CMwFieldInfo("StrLogoUrl", 0x00000029) },
		                { 0x00C, new CMwFieldInfo("StrSubGroupLogoUrl", 0x00000029) },
		                { 0x00D, new CMwFieldInfo("Logo", 0x00000005) },
		                { 0x00E, new CMwFieldInfo("SubGroupLogo", 0x00000005) },
		                { 0x00F, new CMwFieldInfo("DoSelectionOnChildsCount", 0x00000001) },
		                { 0x010, new CMwFieldInfo("UseTop3Medals", 0x00000001) }
	                  })
	                },
	                { 0x0C2, new CMwClassInfo("CGameControlCardMessage", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("StrSender", 0x00000029) },
		                { 0x001, new CMwFieldInfo("StrReceiver", 0x00000029) },
		                { 0x002, new CMwFieldInfo("StrSendDate", 0x00000029) },
		                { 0x003, new CMwFieldInfo("StrSubject", 0x0000002D) },
		                { 0x004, new CMwFieldInfo("StrMessage", 0x0000002D) },
		                { 0x005, new CMwFieldInfo("Donation", 0x0000001F) },
		                { 0x006, new CMwMethodInfo("OnCheckLogin", 0x00000000, null, null) }
	                  })
	                },
	                { 0x0C3, new CMwClassInfo("CGameCalendar", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("Events", 0x00000007) }
	                  })
	                },
	                { 0x0C4, new CMwClassInfo("CGameCalendarEvent", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("Date", 0x00000029) },
		                { 0x001, new CMwFieldInfo("Description", 0x0000002D) },
		                { 0x002, new CMwFieldInfo("Data", 0x00000005) }
	                  })
	                },
	                { 0x0C5, new CMwClassInfo("CGameControlCardCalendar", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("StrToday", 0x0000002D) },
		                { 0x001, new CMwFieldInfo("StrCurrentDate", 0x0000002D) },
		                { 0x002, new CMwFieldInfo("TodayTextModifier", 0x00000029) }
	                  })
	                },
	                { 0x0C6, new CMwClassInfo("CGameControlCardCalendarEvent", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("StrDate", 0x00000029) },
		                { 0x001, new CMwFieldInfo("StrDescription", 0x0000002D) }
	                  })
	                },
	                { 0x0C7, new CMwClassInfo("CGameControlCardProfile", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("StrLogin", 0x0000002D) },
		                { 0x001, new CMwFieldInfo("StrNickName", 0x0000002D) },
		                { 0x002, new CMwFieldInfo("Avatar", 0x00000005) },
		                { 0x003, new CMwMethodInfo("OnChooseProfile", 0x00000000, null, null) },
		                { 0x004, new CMwMethodInfo("OnRemoveProfile", 0x00000000, null, null) },
		                { 0x005, new CMwMethodInfo("OnConnectProfile", 0x00000000, null, null) }
	                  })
	                },
	                { 0x0C8, new CMwClassInfo("CGameControlCardCtnReplayRecordInfo", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("StrName", 0x0000002D) },
		                { 0x001, new CMwFieldInfo("Time", 0x0000001F) },
		                { 0x002, new CMwFieldInfo("ShortName", 0x00000001) }
	                  })
	                },
	                { 0x0C9, new CMwClassInfo("CGameCtnMenus", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("LastString", 0x0000002D) },
		                { 0x001, new CMwFieldInfo("LastString2", 0x0000002D) },
		                { 0x002, new CMwFieldInfo("LastString3", 0x00000029) },
		                { 0x003, new CMwFieldInfo("PasswordString", 0x00000029) },
		                { 0x004, new CMwFieldInfo("PasswordString2", 0x00000029) },
		                { 0x005, new CMwFieldInfo("PasswordString3", 0x00000029) },
		                { 0x006, new CMwFieldInfo("SelectedName", 0x00000029) },
		                { 0x007, new CMwFieldInfo("SelectedNickname", 0x0000002D) },
		                { 0x008, new CMwFieldInfo("SelectedAvatarName", 0x0000002D) },
		                { 0x009, new CMwMethodInfo("ResetMenuBackgrounds", 0x00000000, null, null) },
		                { 0x00A, new CMwFieldInfo("DisplayProfiles", 0x00000007) },
		                { 0x00B, new CMwMethodInfo("MenuMain_ViewManialink", 0x00000000, null, null) },
		                { 0x00C, new CMwMethodInfo("DialogMessenger_Init", 0x00000000, null, null) },
		                { 0x00D, new CMwMethodInfo("DialogMessenger_Update", 0x00000000, null, null) },
		                { 0x00E, new CMwMethodInfo("DialogMessenger_UpdateButtons", 0x00000000, null, null) },
		                { 0x00F, new CMwMethodInfo("DialogMessenger_Clean", 0x00000000, null, null) },
		                { 0x010, new CMwMethodInfo("DialogMessenger_NotifyNewMessages", 0x00000000, null, null) },
		                { 0x011, new CMwMethodInfo("DialogMessenger_OnNewMessage", 0x00000000, null, null) },
		                { 0x012, new CMwMethodInfo("DialogMessenger_OnShowInbox", 0x00000000, null, null) },
		                { 0x013, new CMwMethodInfo("DialogMessenger_OnShowOutbox", 0x00000000, null, null) },
		                { 0x014, new CMwMethodInfo("DialogMessenger_OnSave", 0x00000000, null, null) },
		                { 0x015, new CMwMethodInfo("DialogMessenger_OnSend", 0x00000000, null, null) },
		                { 0x016, new CMwMethodInfo("DialogMessenger_OnReply", 0x00000000, null, null) },
		                { 0x017, new CMwMethodInfo("DialogMessenger_OnSendAll", 0x00000000, null, null) },
		                { 0x018, new CMwMethodInfo("DialogMessenger_OnRemove", 0x00000000, null, null) },
		                { 0x019, new CMwMethodInfo("DialogMessenger_OnRemoveAll", 0x00000000, null, null) },
		                { 0x01A, new CMwMethodInfo("DialogMessenger_OnQuit", 0x00000000, null, null) },
		                { 0x01B, new CMwMethodInfo("Dialog_OnValidate", 0x00000000, null, null) },
		                { 0x01C, new CMwMethodInfo("Dialog_OnCancel", 0x00000000, null, null) },
		                { 0x01D, new CMwMethodInfo("Dialog_OnJoinAsSpectator", 0x00000000, null, null) },
		                { 0x01E, new CMwMethodInfo("DialogList_OnOk", 0x00000000, null, null) },
		                { 0x01F, new CMwFieldInfo("DialogList_Nods", 0x00000007) },
		                { 0x020, new CMwMethodInfo("DialogCardGrid_OnOk", 0x00000000, null, null) },
		                { 0x021, new CMwMethodInfo("DialogCardGrid_OnCancel", 0x00000000, null, null) },
		                { 0x022, new CMwMethodInfo("DialogCardGrid_OnRefresh", 0x00000000, null, null) },
		                { 0x023, new CMwMethodInfo("DialogRefereeStatus_OnCancel", 0x00000000, null, null) },
		                { 0x024, new CMwFieldInfo("DialogRefereeStatus_Message", 0x0000002E) },
		                { 0x025, new CMwFieldInfo("DialogChooseSkin_Name", 0x0000002D) },
		                { 0x026, new CMwFieldInfo("DialogChooseSkin_SelectIndex", 0x0000001F) },
		                { 0x027, new CMwFieldInfo("DialogChooseSkin_Article", 0x00000005) },
		                { 0x028, new CMwFieldInfo("DialogChooseSkin_SkinsItem", 0x00000007) },
		                { 0x029, new CMwFieldInfo("TempLogin", 0x00000029) },
		                { 0x02A, new CMwFieldInfo("TempPassword", 0x00000029) },
		                { 0x02B, new CMwMethodInfo("DialogConnect_OnRememberPassword", 0x00000000, null, null) },
		                { 0x02C, new CMwMethodInfo("DialogConnect_OnForgotPassword", 0x00000000, null, null) },
		                { 0x02D, new CMwMethodInfo("DialogConnect_OnConnect", 0x00000000, null, null) },
		                { 0x02E, new CMwMethodInfo("DialogConnect_OnCancel", 0x00000000, null, null) },
		                { 0x02F, new CMwMethodInfo("DialogEventNonBlocking_OnSelect", 0x00000000, null, null) },
		                { 0x030, new CMwMethodInfo("DialogEventNonBlocking_OnNextPage", 0x00000000, null, null) },
		                { 0x031, new CMwMethodInfo("DialogEventNonBlocking_OnPrevPage", 0x00000000, null, null) },
		                { 0x032, new CMwMethodInfo("DialogEventNonBlocking_OnOk", 0x00000000, null, null) },
		                { 0x033, new CMwMethodInfo("DialogEventNonBlocking_OnCancel", 0x00000000, null, null) },
		                { 0x034, new CMwMethodInfo("DialogEventNonBlocking_OnEdit", 0x00000000, null, null) },
		                { 0x035, new CMwMethodInfo("DialogEventNonBlocking_OnRemove", 0x00000000, null, null) },
		                { 0x036, new CMwMethodInfo("DialogEventNonBlocking_OnAdd", 0x00000000, null, null) },
		                { 0x037, new CMwMethodInfo("DialogChoosePackDesc_OnNextPage", 0x00000000, null, null) },
		                { 0x038, new CMwMethodInfo("DialogChoosePackDesc_OnPrevPage", 0x00000000, null, null) },
		                { 0x039, new CMwMethodInfo("DialogChoosePackDesc_DrawCurPage", 0x00000000, null, null) },
		                { 0x03A, new CMwMethodInfo("DialogChoosePackDesc_OnAddItem", 0x00000000, null, null) },
		                { 0x03B, new CMwMethodInfo("DialogConfirmOfficial_OnOk", 0x00000000, null, null) },
		                { 0x03C, new CMwMethodInfo("DialogConfirmOfficial_OnCancel", 0x00000000, null, null) },
		                { 0x03D, new CMwMethodInfo("DialogInputSettings_OnQuit", 0x00000000, null, null) },
		                { 0x03E, new CMwMethodInfo("DialogInputSettings_OnApplyAll", 0x00000000, null, null) },
		                { 0x03F, new CMwFieldInfo("DialogInputSettings_AnalogDeadZone", 0x00000028) },
		                { 0x040, new CMwFieldInfo("DialogInputSettings_AnalogSensitivity", 0x00000028) },
		                { 0x041, new CMwFieldInfo("DialogInputSettings_RumbleIntensity", 0x00000028) },
		                { 0x042, new CMwMethodInfo("DialogStereoscopySettings_OnQuit", 0x00000000, null, null) },
		                { 0x043, new CMwMethodInfo("DialogStereoscopySettings_Enable", 0x00000000, null, null) },
		                { 0x044, new CMwMethodInfo("DialogStereoscopySettings_Disable", 0x00000000, null, null) },
		                { 0x045, new CMwMethodInfo("DialogStereoscopySettings_DefaultValues", 0x00000000, null, null) },
		                { 0x046, new CMwFieldInfo("DialogStereoscopySettings_Strength", 0x00000028) },
		                { 0x047, new CMwFieldInfo("DialogStereoscopySettings_Separation", 0x00000028) },
		                { 0x048, new CMwFieldInfo("DialogStereoscopySettings_ScreenDist", 0x00000028) },
		                { 0x049, new CMwFieldInfo("DialogStereoscopySettings_ColorFactor", 0x00000028) },
		                { 0x04A, new CMwFieldInfo("DialogStereoscopySettings_HeadTrack", 0x00000001) },
		                { 0x04B, new CMwEnumInfo("DialogStereoscopySettings_Mode", new string[] { "Anaglyph", "Left/Right", "Left/Right'", "LineEven/Odd", "LineOdd/Even" }) },
		                { 0x04C, new CMwMethodInfo("MenuChooseChallenge_OnSelect", 0x00000000, null, null) },
		                { 0x04D, new CMwMethodInfo("MenuChooseChallenge_OnBack", 0x00000000, null, null) },
		                { 0x04E, new CMwMethodInfo("MenuChooseChallenge_OnRefresh", 0x00000000, null, null) },
		                { 0x04F, new CMwMethodInfo("MenuChooseChallenge_OnOk", 0x00000000, null, null) },
		                { 0x050, new CMwMethodInfo("MenuChooseChallenge_OnHierarchyUp", 0x00000000, null, null) },
		                { 0x051, new CMwMethodInfo("MenuChooseChallenge_OnChangeLeague", 0x00000000, null, null) },
		                { 0x052, new CMwMethodInfo("MenuChooseChallenge_OnChallengeRemoved", 0x00000000, null, null) },
		                { 0x053, new CMwMethodInfo("MenuChooseChallenge_OnChallengeRemovedConfirmed", 0x00000000, null, null) },
		                { 0x054, new CMwMethodInfo("MenuChooseChallenge_SelectOrUnselectAll", 0x00000000, null, null) },
		                { 0x055, new CMwFieldInfo("MenuChooseChallenge_ChallengesCount", 0x0000001F) },
		                { 0x056, new CMwFieldInfo("MenuChooseChallenge_Flatten", 0x00000001) },
		                { 0x057, new CMwFieldInfo("SelectAll", 0x00000001) },
		                { 0x058, new CMwFieldInfo("HierarchyPath", 0x0000002D) },
		                { 0x059, new CMwFieldInfo("MenuChooseChallenge_FilterString", 0x0000002D) },
		                { 0x05A, new CMwMethodInfo("MenuMultiLocal", 0x00000000, null, null) },
		                { 0x05B, new CMwMethodInfo("MenuMultiLocal_OnBack", 0x00000000, null, null) },
		                { 0x05C, new CMwMethodInfo("MenuMultiLocal_OnOk", 0x00000000, null, null) },
		                { 0x05D, new CMwMethodInfo("MenuMultiLocal_OnConfigureInputs", 0x00000000, null, null) },
		                { 0x05E, new CMwFieldInfo("MenuMultiLocal_SetUsedPlayersCount", 0x00000003) },
		                { 0x05F, new CMwMethodInfo("MenuVehicleProfile_OnBack", 0x00000000, null, null) },
		                { 0x060, new CMwMethodInfo("MenuVehicleProfile_OnApplyAll", 0x00000000, null, null) },
		                { 0x061, new CMwFieldInfo("MenuVehicleProfile_CurVehicleUrl", 0x00000029) },
		                { 0x062, new CMwMethodInfo("MenuVehicleProfile_OnPaint", 0x00000000, null, null) },
		                { 0x063, new CMwMethodInfo("MenuProfileAdvanced", 0x00000000, null, null) },
		                { 0x064, new CMwMethodInfo("MenuProfile", 0x00000000, null, null) },
		                { 0x065, new CMwMethodInfo("MenuProfile_Launch", 0x00000000, null, null) },
		                { 0x066, new CMwMethodInfo("MenuProfile_OnChooseAvatar", 0x00000000, null, null) },
		                { 0x067, new CMwMethodInfo("MenuProfile_OnChangeHorn", 0x00000000, null, null) },
		                { 0x068, new CMwMethodInfo("MenuProfile_OnResetHorn", 0x00000000, null, null) },
		                { 0x069, new CMwMethodInfo("MenuProfile_OnChooseHorn", 0x00000000, null, null) },
		                { 0x06A, new CMwMethodInfo("MenuProfile_OnReceiveNews", 0x00000000, null, null) },
		                { 0x06B, new CMwMethodInfo("MenuProfile_ChooseHorn_OnOk", 0x00000000, null, null) },
		                { 0x06C, new CMwMethodInfo("MenuProfile_ChooseHorn_OnSelect", 0x00000000, null, null) },
		                { 0x06D, new CMwMethodInfo("MenuProfile_OnAddGroup", 0x00000000, null, null) },
		                { 0x06E, new CMwMethodInfo("MenuProfile_RemoveGroup", 0x00000000, null, null) },
		                { 0x06F, new CMwMethodInfo("MenuProfile_OnChangeZone", 0x00000000, null, null) },
		                { 0x070, new CMwMethodInfo("MenuProfile_OnConvertAccount", 0x00000000, null, null) },
		                { 0x071, new CMwMethodInfo("MenuProfile_OnDisconnectAccount", 0x00000000, null, null) },
		                { 0x072, new CMwMethodInfo("MenuProfile_OnDisconnectAccountConfirmed", 0x00000000, null, null) },
		                { 0x073, new CMwMethodInfo("MenuProfile_OnCheckModifications", 0x00000000, null, null) },
		                { 0x074, new CMwMethodInfo("MenuProfile_OnChangePass", 0x00000000, null, null) },
		                { 0x075, new CMwMethodInfo("MenuProfile_OnBack", 0x00000000, null, null) },
		                { 0x076, new CMwFieldInfo("MenuProfile_Avatar", 0x00000005) },
		                { 0x077, new CMwFieldInfo("Menu_GroupToRemove", 0x00000005) },
		                { 0x078, new CMwFieldInfo("MenuProfile_GroupsCount", 0x0000001F) },
		                { 0x079, new CMwFieldInfo("MenuProfile_HornName", 0x0000002D) },
		                { 0x07A, new CMwMethodInfo("MenuPlayerPage_OnBack", 0x00000000, null, null) },
		                { 0x07B, new CMwMethodInfo("DialogManageGroup_OnCreate", 0x00000000, null, null) },
		                { 0x07C, new CMwFieldInfo("DialogManageGroup_Name", 0x0000002D) },
		                { 0x07D, new CMwFieldInfo("DialogManageGroup_Login", 0x00000029) },
		                { 0x07E, new CMwFieldInfo("DialogManageGroup_Pass", 0x00000029) },
		                { 0x07F, new CMwFieldInfo("DialogManageGroup_PassConfirm", 0x00000029) },
		                { 0x080, new CMwMethodInfo("MenuPaintVehicle", 0x00000000, null, null) },
		                { 0x081, new CMwMethodInfo("MenuPaintVehicle_OnBackConfirm", 0x00000000, null, null) },
		                { 0x082, new CMwMethodInfo("MenuPaintVehicle_OnBack", 0x00000000, null, null) },
		                { 0x083, new CMwFieldInfo("Painter", 0x00000005) },
		                { 0x084, new CMwMethodInfo("DialogRegisterAccountChoice_CreateNewAccount", 0x00000000, null, null) },
		                { 0x085, new CMwMethodInfo("DialogRegisterAccountChoice_UseExistingAccount", 0x00000000, null, null) },
		                { 0x086, new CMwMethodInfo("DialogRegisterAccountChoice_Offline", 0x00000000, null, null) },
		                { 0x087, new CMwMethodInfo("DialogOnlineAccount_OnCancel", 0x00000000, null, null) },
		                { 0x088, new CMwMethodInfo("DialogOnlineAccount_OnOk", 0x00000000, null, null) },
		                { 0x089, new CMwMethodInfo("DialogOnlineAccount_OnCheckLogin", 0x00000000, null, null) },
		                { 0x08A, new CMwMethodInfo("DialogOnlineAccountInfo_OnCopyToClipboard", 0x00000000, null, null) },
		                { 0x08B, new CMwMethodInfo("DialogOnlineAccountError_OnOk", 0x00000000, null, null) },
		                { 0x08C, new CMwMethodInfo("DialogOnlineAccountError_OnCancel", 0x00000000, null, null) },
		                { 0x08D, new CMwMethodInfo("DialogOnlineAccountError_OnMailAccount", 0x00000000, null, null) },
		                { 0x08E, new CMwMethodInfo("DialogOnlineAccount_OnRememberPassword", 0x00000000, null, null) },
		                { 0x08F, new CMwMethodInfo("DialogOnlineAccountPersonnal_OnReceiveNews", 0x00000000, null, null) },
		                { 0x090, new CMwFieldInfo("DialogOnlineAccount_ChooseLogin1", 0x00000001) },
		                { 0x091, new CMwFieldInfo("DialogOnlineAccount_ChooseLogin2", 0x00000001) },
		                { 0x092, new CMwFieldInfo("DialogOnlineAccount_ChooseLogin3", 0x00000001) },
		                { 0x093, new CMwFieldInfo("DialogOnlineAccount_SubscriptionLeague", 0x00000005) },
		                { 0x094, new CMwMethodInfo("DialogOnlineAccountPersonnal_OnCancel", 0x00000000, null, null) },
		                { 0x095, new CMwMethodInfo("DialogOnlineAccountPersonnal_OnOk", 0x00000000, null, null) },
		                { 0x096, new CMwMethodInfo("DialogImportAccount_OnImport", 0x00000000, null, null) },
		                { 0x097, new CMwMethodInfo("DialogImportAccount_OnCancel", 0x00000000, null, null) },
		                { 0x098, new CMwFieldInfo("ImportAccount_UseFreeAccountLogin", 0x00000001) },
		                { 0x099, new CMwFieldInfo("ImportAccount_Login", 0x00000029) },
		                { 0x09A, new CMwFieldInfo("ImportAccount_Pass", 0x00000029) },
		                { 0x09B, new CMwMethodInfo("DialogConvertAccount_OnConvert", 0x00000000, null, null) },
		                { 0x09C, new CMwMethodInfo("DialogConvertAccount_OnCancel", 0x00000000, null, null) },
		                { 0x09D, new CMwFieldInfo("ConvertAccount_Key", 0x00000029) },
		                { 0x09E, new CMwFieldInfo("OnlineAccount_Key", 0x00000029) },
		                { 0x09F, new CMwFieldInfo("OnlineAccount_Login", 0x00000029) },
		                { 0x0A0, new CMwFieldInfo("OnlineAccount_Pass", 0x00000029) },
		                { 0x0A1, new CMwFieldInfo("OnlineAccount_PassConfirm", 0x00000029) },
		                { 0x0A2, new CMwFieldInfo("OnlineAccount_PassNew", 0x00000029) },
		                { 0x0A3, new CMwFieldInfo("OnlineAccount_Age", 0x0000001F) },
		                { 0x0A4, new CMwFieldInfo("OnlineAccount_EMail", 0x00000029) },
		                { 0x0A5, new CMwFieldInfo("OnlineAccount_NickName", 0x0000002D) },
		                { 0x0A6, new CMwFieldInfo("OnlineAccount_Path", 0x0000002D) },
		                { 0x0A7, new CMwFieldInfo("OnlineAccount_WishedPath", 0x0000002D) },
		                { 0x0A8, new CMwFieldInfo("OnlineAccount_NewLeague", 0x00000005) },
		                { 0x0A9, new CMwFieldInfo("OnlineAccount_AcceptNews", 0x00000001) },
		                { 0x0AA, new CMwFieldInfo("DialogPlayers", 0x00000007) },
		                { 0x0AB, new CMwFieldInfo("ConfirmFiles", 0x00000007) },
		                { 0x0AC, new CMwFieldInfo("ReplayInfos", 0x00000007) },
		                { 0x0AD, new CMwMethodInfo("MenuMultiPlayerNetworkLan_OnRefresh", 0x00000000, null, null) },
		                { 0x0AE, new CMwMethodInfo("MenuMultiPlayerNetworkLan_OnSel", 0x00000000, null, null) },
		                { 0x0AF, new CMwMethodInfo("MenuMultiPlayerNetworkLan_OnCreate", 0x00000000, null, null) },
		                { 0x0B0, new CMwMethodInfo("MenuMultiPlayerNetworkLan_OnBack", 0x00000000, null, null) },
		                { 0x0B1, new CMwFieldInfo("MenuMultiPlayerNetworkLan_ServersCount", 0x0000001F) },
		                { 0x0B2, new CMwFieldInfo("MenuMultiPlayerNetworkLan_PlayersCount", 0x0000001F) },
		                { 0x0B3, new CMwFieldInfo("DialogRemoteBrowser_ToFind", 0x0000002D) },
		                { 0x0B4, new CMwMethodInfo("DialogRemoteBrowser_OnClose", 0x00000000, null, null) },
		                { 0x0B5, new CMwMethodInfo("DialogRemoteBrowser_OnFind", 0x00000000, null, null) },
		                { 0x0B6, new CMwMethodInfo("MenuInternetLeague_OnAll", 0x00000000, null, null) },
		                { 0x0B7, new CMwMethodInfo("MenuInternetLeague_OnSuggested", 0x00000000, null, null) },
		                { 0x0B8, new CMwMethodInfo("MenuInternetLeague_OnFavorites", 0x00000000, null, null) },
		                { 0x0B9, new CMwMethodInfo("MenuInternetLeague_OnRankings", 0x00000000, null, null) },
		                { 0x0BA, new CMwMethodInfo("MenuInternetLeague_OnCreate", 0x00000000, null, null) },
		                { 0x0BB, new CMwMethodInfo("MenuInternetLeague_OnBack", 0x00000000, null, null) },
		                { 0x0BC, new CMwMethodInfo("MenuInternetLeague_OnRefresh", 0x00000000, null, null) },
		                { 0x0BD, new CMwMethodInfo("MenuInternetLeague_OnRefreshSimple", 0x00000000, null, null) },
		                { 0x0BE, new CMwMethodInfo("MenuInternetLeague_OnHierarchyUp", 0x00000000, null, null) },
		                { 0x0BF, new CMwMethodInfo("MenuInternetLeague_OnHierarchyItemSelected", 0x00000000, null, null) },
		                { 0x0C0, new CMwMethodInfo("MenuInternetLeague_OnServerSelected", 0x00000000, null, null) },
		                { 0x0C1, new CMwMethodInfo("MenuInternetLeague_OnPlayerSelected", 0x00000000, null, null) },
		                { 0x0C2, new CMwMethodInfo("MenuInternetLeague_OnFilterGameMode", 0x00000000, null, null) },
		                { 0x0C3, new CMwMethodInfo("MenuInternetLeague_BrowseServers", 0x00000000, null, null) },
		                { 0x0C4, new CMwMethodInfo("MenuInternetLeague_BrowsePlayers", 0x00000000, null, null) },
		                { 0x0C5, new CMwMethodInfo("MenuInternetLeague_OnChangeBuddyState", 0x00000000, null, null) },
		                { 0x0C6, new CMwMethodInfo("MenuInternetLeague_OnLadderHierarchyUp", 0x00000000, null, null) },
		                { 0x0C7, new CMwMethodInfo("MenuInternetLeague_SwitchServersPlayers", 0x00000000, null, null) },
		                { 0x0C8, new CMwMethodInfo("MenuInternetLeague_SwitchLeaguesPlayersLadder", 0x00000000, null, null) },
		                { 0x0C9, new CMwFieldInfo("MenuInternetLeague_Today", 0x0000002D) },
		                { 0x0CA, new CMwFieldInfo("MenuInternet_Path", 0x0000002D) },
		                { 0x0CB, new CMwFieldInfo("MenuInternet_LadderCurrentLeagueName", 0x0000002D) },
		                { 0x0CC, new CMwFieldInfo("MenuInternetLeague_CurrentLeagueName", 0x0000002D) },
		                { 0x0CD, new CMwFieldInfo("MenuInternetLeague_CurrentLeagueDescription", 0x0000002D) },
		                { 0x0CE, new CMwFieldInfo("MenuInternet_OnlyPaying", 0x00000001) },
		                { 0x0CF, new CMwMethodInfo("DialogChooseGameModeFilter_OnGameModeChoosen", 0x00000000, null, null) },
		                { 0x0D0, new CMwMethodInfo("MenuReplay_OnRefresh", 0x00000000, null, null) },
		                { 0x0D1, new CMwMethodInfo("MenuReplay_OnSelectDir", 0x00000000, null, null) },
		                { 0x0D2, new CMwMethodInfo("MenuReplay_OnSelectReplay", 0x00000000, null, null) },
		                { 0x0D3, new CMwMethodInfo("MenuReplay_OnOk", 0x00000000, null, null) },
		                { 0x0D4, new CMwMethodInfo("MenuReplay_OnSelectAll", 0x00000000, null, null) },
		                { 0x0D5, new CMwMethodInfo("MenuReplay_OnRollingDemo", 0x00000000, null, null) },
		                { 0x0D6, new CMwMethodInfo("MenuReplay_FilterAndRedraw", 0x00000000, null, null) },
		                { 0x0D7, new CMwMethodInfo("MenuReplay_OnPathUp", 0x00000000, null, null) },
		                { 0x0D8, new CMwFieldInfo("MenuReplay_Flatten", 0x00000001) },
		                { 0x0D9, new CMwFieldInfo("MenuReplay_CurPath", 0x0000002D) },
		                { 0x0DA, new CMwFieldInfo("MenuReplay_ReplaysCount", 0x0000001F) },
		                { 0x0DB, new CMwFieldInfo("ReplayList", 0x00000007) },
		                { 0x0DC, new CMwFieldInfo("ReplayDirsList", 0x00000007) },
		                { 0x0DD, new CMwMethodInfo("MenuConfigureInputs", 0x00000000, null, null) },
		                { 0x0DE, new CMwMethodInfo("MenuConfigureInputs_OnSel", 0x00000000, null, null) },
		                { 0x0DF, new CMwMethodInfo("MenuConfigureInputs_OnBack", 0x00000000, null, null) },
		                { 0x0E0, new CMwFieldInfo("MenuConfigureInputs_Actions", 0x00000007) },
		                { 0x0E1, new CMwMethodInfo("MenuConfigureInputs_SetDefaults", 0x00000000, null, null) },
		                { 0x0E2, new CMwMethodInfo("MenuConfigureInputs_SetDefaults_OnYes", 0x00000000, null, null) },
		                { 0x0E3, new CMwMethodInfo("DialogChooseProfile_OnAdd", 0x00000000, null, null) },
		                { 0x0E4, new CMwMethodInfo("DialogChooseProfile_OnSelect", 0x00000000, null, null) },
		                { 0x0E5, new CMwMethodInfo("DialogChooseProfile_OnCancel", 0x00000000, null, null) },
		                { 0x0E6, new CMwMethodInfo("DialogInGameMenu_OnAdvanced", 0x00000000, null, null) },
		                { 0x0E7, new CMwMethodInfo("DialogInGameMenu_OnCancel", 0x00000000, null, null) },
		                { 0x0E8, new CMwMethodInfo("DialogInGameMenu_OnResume", 0x00000000, null, null) },
		                { 0x0E9, new CMwMethodInfo("DialogInGameMenu_Restart", 0x00000000, null, null) },
		                { 0x0EA, new CMwMethodInfo("DialogInGameMenu_Next", 0x00000000, null, null) },
		                { 0x0EB, new CMwMethodInfo("DialogInGameMenu_Spectator", 0x00000000, null, null) },
		                { 0x0EC, new CMwMethodInfo("DialogInGameMenu_OfficialMode", 0x00000000, null, null) },
		                { 0x0ED, new CMwMethodInfo("DialogInGameMenu_Buddy", 0x00000000, null, null) },
		                { 0x0EE, new CMwMethodInfo("DialogInGameMenu_Abuse", 0x00000000, null, null) },
		                { 0x0EF, new CMwMethodInfo("DialogInGameMenu_Kick", 0x00000000, null, null) },
		                { 0x0F0, new CMwMethodInfo("DialogInGameMenu_Ban", 0x00000000, null, null) },
		                { 0x0F1, new CMwMethodInfo("DialogInGameMenu_OnBecomeReferee", 0x00000000, null, null) },
		                { 0x0F2, new CMwMethodInfo("DialogInGameMenu_OnSaveReplay", 0x00000000, null, null) },
		                { 0x0F3, new CMwMethodInfo("DialogInGameMenu_OnSavePrevReplay", 0x00000000, null, null) },
		                { 0x0F4, new CMwMethodInfo("DialogInGameMenu_OnSaveChallenge", 0x00000000, null, null) },
		                { 0x0F5, new CMwMethodInfo("DialogInGameMenu_OnQuit", 0x00000000, null, null) },
		                { 0x0F6, new CMwMethodInfo("DialogInGameMenu_OnPlayerProfile", 0x00000000, null, null) },
		                { 0x0F7, new CMwMethodInfo("DialogInGameMenu_OnLadderRankings", 0x00000000, null, null) },
		                { 0x0F8, new CMwMethodInfo("DialogInGameMenu_OnVote", 0x00000000, null, null) },
		                { 0x0F9, new CMwMethodInfo("DialogPlayerBuddy_OnOk", 0x00000000, null, null) },
		                { 0x0FA, new CMwMethodInfo("DialogPlayerKick_OnCancel", 0x00000000, null, null) },
		                { 0x0FB, new CMwMethodInfo("DialogPlayerProfile_OnOk", 0x00000000, null, null) },
		                { 0x0FC, new CMwMethodInfo("DialogPlayerProfile_OnPrevPlayer", 0x00000000, null, null) },
		                { 0x0FD, new CMwMethodInfo("DialogPlayerProfile_OnNextPlayer", 0x00000000, null, null) },
		                { 0x0FE, new CMwMethodInfo("DialogPlayerProfile_OnHorn", 0x00000000, null, null) },
		                { 0x0FF, new CMwMethodInfo("DialogPlayerProfile_Buddy", 0x00000000, null, null) },
		                { 0x100, new CMwMethodInfo("DialogPlayerProfile_Kick", 0x00000000, null, null) },
		                { 0x101, new CMwMethodInfo("DialogPlayerProfile_Ban", 0x00000000, null, null) },
		                { 0x102, new CMwMethodInfo("DialogPlayerProfile_Abuse", 0x00000000, null, null) },
		                { 0x103, new CMwMethodInfo("DialogPlayerProfile_Validate", 0x00000000, null, null) },
		                { 0x104, new CMwMethodInfo("DialogLadderRankingsOld_OnOk", 0x00000000, null, null) },
		                { 0x105, new CMwMethodInfo("DialogLadderRankingsOld_OnBack", 0x00000000, null, null) },
		                { 0x106, new CMwMethodInfo("DialogLadderRankingsOld_OnShowCurrentPlayersRankings", 0x00000000, null, null) },
		                { 0x107, new CMwMethodInfo("DialogLadderRankingsOld_OnSelectItem", 0x00000000, null, null) },
		                { 0x108, new CMwMethodInfo("DialogLadderRankings_OnOk", 0x00000000, null, null) },
		                { 0x109, new CMwMethodInfo("DialogLadderRankings_OnBack", 0x00000000, null, null) },
		                { 0x10A, new CMwMethodInfo("DialogLadderRankings_OnShowCurrentPlayersRankings", 0x00000000, null, null) },
		                { 0x10B, new CMwMethodInfo("DialogLadderRankings_OnRankingSelected", 0x00000000, null, null) },
		                { 0x10C, new CMwMethodInfo("DialogVote_OnVoteYes", 0x00000000, null, null) },
		                { 0x10D, new CMwMethodInfo("DialogVote_OnVoteNo", 0x00000000, null, null) },
		                { 0x10E, new CMwMethodInfo("DialogVote_OnCancel", 0x00000000, null, null) },
		                { 0x10F, new CMwFieldInfo("DialogPlayerProfileCameraControl", 0x00000005) },
		                { 0x110, new CMwFieldInfo("DialogPlayerProfileVehicleOverlayScene", 0x00000005) },
		                { 0x111, new CMwFieldInfo("DialogPlayerProfile_Long", 0x00000024) },
		                { 0x112, new CMwFieldInfo("DialogPlayerProfile_Lat", 0x00000024) },
		                { 0x113, new CMwFieldInfo("DialogPlayerProfile_Radius", 0x00000024) },
		                { 0x114, new CMwFieldInfo("DialogPlayerProfile_Speed", 0x00000024) },
		                { 0x115, new CMwMethodInfo("DialogAskIncreaseCacheSize_OnYes", 0x00000000, null, null) },
		                { 0x116, new CMwMethodInfo("DialogAskIncreaseCacheSize_OnNo", 0x00000000, null, null) },
		                { 0x117, new CMwMethodInfo("DialogAskIncreaseCacheSize_OnNever", 0x00000000, null, null) },
		                { 0x118, new CMwMethodInfo("DialogChooseLeague", 0x00000000, null, null) },
		                { 0x119, new CMwMethodInfo("DialogChooseLeague_Clean", 0x00000000, null, null) },
		                { 0x11A, new CMwMethodInfo("DialogChooseLeague_UpdateThisLevel", 0x00000000, null, null) },
		                { 0x11B, new CMwMethodInfo("DialogChooseLeague_OnSelect", 0x00000000, null, null) },
		                { 0x11C, new CMwMethodInfo("DialogChooseLeague_OnOk", 0x00000000, null, null) },
		                { 0x11D, new CMwMethodInfo("DialogChooseLeague_OnCancel", 0x00000000, null, null) },
		                { 0x11E, new CMwFieldInfo("DialogChooseLeague_CurrentPath", 0x0000002D) },
		                { 0x11F, new CMwFieldInfo("DialogChooseLeague_DisplayableCurrentPath", 0x0000002D) },
		                { 0x120, new CMwMethodInfo("DialogCreateProfile_OnOk", 0x00000000, null, null) },
		                { 0x121, new CMwMethodInfo("DialogCreateProfile_OnCancel", 0x00000000, null, null) },
		                { 0x122, new CMwMethodInfo("DialogCreateProfile_OnAvatar", 0x00000000, null, null) },
		                { 0x123, new CMwMethodInfo("DialogChooseAvatar_OnCancel", 0x00000000, null, null) },
		                { 0x124, new CMwMethodInfo("DialogChooseAvatar_OnAddAvatar", 0x00000000, null, null) },
		                { 0x125, new CMwFieldInfo("NeverAskAgain", 0x00000001) },
		                { 0x126, new CMwFieldInfo("DataInfos", 0x00000007) },
		                { 0x127, new CMwFieldInfo("SceneProfile", 0x00000005) },
		                { 0x128, new CMwMethodInfo("DialogCredits_NonBlocking", 0x00000000, null, null) },
		                { 0x129, new CMwFieldInfo("DialogConnect_RememberOnlinePassword", 0x00000001) },
		                { 0x12A, new CMwFieldInfo("DialogOnlineAccount_RememberOnlinePassword", 0x00000001) },
		                { 0x12B, new CMwFieldInfo("DialogOnlineAccountPersonnal_ReceiveNews", 0x00000001) },
		                { 0x12C, new CMwMethodInfo("DialogAddBuddy_OnAdd", 0x00000000, null, null) },
		                { 0x12D, new CMwMethodInfo("DialogMailBuddy_OnMail", 0x00000000, null, null) },
		                { 0x12E, new CMwMethodInfo("DialogAddOrInviteBuddy_OnCancel", 0x00000000, null, null) },
		                { 0x12F, new CMwFieldInfo("DialogAddBuddy_Login", 0x00000029) },
		                { 0x130, new CMwFieldInfo("DialogMailBuddy_EMail", 0x00000029) },
		                { 0x131, new CMwMethodInfo("DialogGraphicSettings_OnApply", 0x00000000, null, null) },
		                { 0x132, new CMwMethodInfo("DialogGraphicSettings_OnCancel", 0x00000000, null, null) },
		                { 0x133, new CMwFieldInfo("MenuProfile_TagsAdmin_CurTag", 0x0000001F) },
		                { 0x134, new CMwFieldInfo("MenuProfile_TagsAdmin_TagCount", 0x0000001F) },
		                { 0x135, new CMwFieldInfo("MenuProfile_TagsAdmin_CurTagIsAvailableForConsultation", 0x00000001) }
	                  })
	                },
	                { 0x0CA, new CMwClassInfo("CGameLadderRankingCtnChallengeAchievement", new Dictionary<int, CMwMemberInfo>()
	                  {
	                  })
	                },
	                { 0x0CB, new CMwClassInfo("CGameCtnNetForm", new Dictionary<int, CMwMemberInfo>()
	                  {
	                  })
	                },
	                { 0x0CC, new CMwClassInfo("CGameRemoteBufferDataInfoFinds", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("RefreshPlayerDuration", 0x0000001F) },
		                { 0x001, new CMwFieldInfo("RefreshServerDuration", 0x0000001F) },
		                { 0x002, new CMwFieldInfo("CountPlayerPerPage", 0x0000001F) },
		                { 0x003, new CMwFieldInfo("CountServerPerPage", 0x0000001F) }
	                  })
	                },
	                { 0x0CD, new CMwClassInfo("CGameRemoteBufferDataInfoRankings", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("RefreshLeagueRankingDuration", 0x0000001F) },
		                { 0x001, new CMwFieldInfo("RefreshPlayerRankingDuration", 0x0000001F) },
		                { 0x002, new CMwFieldInfo("RefreshTeamRankingDuration", 0x0000001F) },
		                { 0x003, new CMwFieldInfo("RefreshSkillRankingDuration", 0x0000001F) },
		                { 0x004, new CMwFieldInfo("RefreshAchievementRankingDuration", 0x0000001F) },
		                { 0x005, new CMwFieldInfo("CountLeagueRankingPerPage", 0x0000001F) },
		                { 0x006, new CMwFieldInfo("CountPlayerRankingPerPage", 0x0000001F) },
		                { 0x007, new CMwFieldInfo("CountTeamRankingPerPage", 0x0000001F) },
		                { 0x008, new CMwFieldInfo("CountSkillRankingPerPage", 0x0000001F) },
		                { 0x009, new CMwFieldInfo("CountAchievementRankingPerPage", 0x0000001F) }
	                  })
	                },
	                { 0x0CE, new CMwClassInfo("CGameRemoteBufferDataInfoSearchs", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("RefreshLeaguesDuration", 0x0000001F) },
		                { 0x001, new CMwFieldInfo("RefreshServersDuration", 0x0000001F) },
		                { 0x002, new CMwFieldInfo("RefreshServersSuggestedDuration", 0x0000001F) },
		                { 0x003, new CMwFieldInfo("RefreshServersFavouritesDuration", 0x0000001F) },
		                { 0x004, new CMwFieldInfo("RefreshPlayersDuration", 0x0000001F) },
		                { 0x005, new CMwFieldInfo("LeaguesPerPageCount", 0x0000001F) },
		                { 0x006, new CMwFieldInfo("ServersPerPageCount", 0x0000001F) },
		                { 0x007, new CMwFieldInfo("ServersSuggestedPerPageCount", 0x0000001F) },
		                { 0x008, new CMwFieldInfo("ServersFavouritesPerPageCount", 0x0000001F) },
		                { 0x009, new CMwFieldInfo("PlayersPerPageCount", 0x0000001F) }
	                  })
	                },
	                { 0x0CF, new CMwClassInfo("CGameTournament", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("Path", 0x0000002D) },
		                { 0x001, new CMwFieldInfo("Name", 0x0000002D) },
		                { 0x002, new CMwFieldInfo("Description", 0x0000002D) },
		                { 0x003, new CMwFieldInfo("ContestantsInCount", 0x0000001F) },
		                { 0x004, new CMwFieldInfo("ContestantsOutCount", 0x0000001F) },
		                { 0x005, new CMwFieldInfo("RoundsCount", 0x0000001F) },
		                { 0x006, new CMwFieldInfo("Logo", 0x00000005) }
	                  })
	                },
	                { 0x0D0, new CMwClassInfo("CGameChampionship", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("Path", 0x0000002D) },
		                { 0x001, new CMwFieldInfo("Name", 0x0000002D) },
		                { 0x002, new CMwFieldInfo("Description", 0x0000002D) },
		                { 0x003, new CMwFieldInfo("CanSubscribe", 0x00000001) },
		                { 0x004, new CMwFieldInfo("IsPrivate", 0x00000001) },
		                { 0x005, new CMwFieldInfo("Logo", 0x00000005) },
		                { 0x006, new CMwFieldInfo("Contestants", 0x00000007) },
		                { 0x007, new CMwFieldInfo("Tournaments", 0x00000007) },
		                { 0x008, new CMwFieldInfo("GeneralCalendar", 0x00000005) }
	                  })
	                },
	                { 0x0D1, new CMwClassInfo("CGameCtnMasterServer", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("IsTransfering", 0x00000001) },
		                { 0x001, new CMwFieldInfo("ManiaZones_Disable", 0x00000001) },
		                { 0x002, new CMwFieldInfo("ManiaZones_DisableSubTitle", 0x00000001) },
		                { 0x003, new CMwFieldInfo("ManiaZones_Title", 0x0000002D) },
		                { 0x004, new CMwFieldInfo("ManiaZones_SubTitle", 0x0000002D) },
		                { 0x005, new CMwFieldInfo("TeamsInfos", 0x00000007) }
	                  })
	                },
	                { 0x0D2, new CMwClassInfo("CGameCtnNetwork", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("NextChallengeIndex", 0x0000001F) },
		                { 0x001, new CMwFieldInfo("Spectator", 0x00000001) },
		                { 0x002, new CMwFieldInfo("OfficialMode", 0x00000001) },
		                { 0x003, new CMwMethodInfo("GetFilesToSubmit", 0x00000000, null, null) },
		                { 0x004, new CMwFieldInfo("InGetReplaysMode", 0x00000001) },
		                { 0x005, new CMwFieldInfo("IsInRefereeMode", 0x00000001) },
		                { 0x006, new CMwEnumInfo("ValidationMode", new string[] { "|RefereeMode|Top3", "|RefereeMode|All" }) }
	                  })
	                },
	                { 0x0D3, new CMwClassInfo("CGameCtnApp", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("Interface", 0x00000005) },
		                { 0x001, new CMwFieldInfo("Advertising", 0x00000005) },
		                { 0x002, new CMwFieldInfo("OfficialCampaigns", 0x00000007) },
		                { 0x003, new CMwFieldInfo("DynamicCampaigns", 0x00000007) },
		                { 0x004, new CMwFieldInfo("CollectionFids", 0x00000007) },
		                { 0x005, new CMwFieldInfo("AdditionalCollectorsFids", 0x00000007) },
		                { 0x006, new CMwFieldInfo("AdditionalSkinsFids", 0x00000007) },
		                { 0x007, new CMwFieldInfo("ChallengeInfos", 0x00000007) },
		                { 0x008, new CMwFieldInfo("ReplayRecordInfos", 0x00000007) },
		                { 0x009, new CMwFieldInfo("VertexCount", 0x0000001F) },
		                { 0x00A, new CMwFieldInfo("MenuBackground_MustLoopIntro", 0x00000001) },
		                { 0x00B, new CMwFieldInfo("MessagesCount", 0x0000001F) },
		                { 0x00C, new CMwFieldInfo("GameCamera", 0x00000005) },
		                { 0x00D, new CMwFieldInfo("GameScene", 0x00000005) },
		                { 0x00E, new CMwFieldInfo("Challenge", 0x00000005) },
		                { 0x00F, new CMwFieldInfo("CurrentPlayground", 0x00000005) },
		                { 0x010, new CMwFieldInfo("Money", 0x00000029) },
		                { 0x011, new CMwFieldInfo("PlayerProfiles", 0x00000007) },
		                { 0x012, new CMwFieldInfo("Scores", 0x00000007) },
		                { 0x013, new CMwFieldInfo("CurrentProfile", 0x00000005) },
		                { 0x014, new CMwFieldInfo("CurrentScores", 0x00000005) },
		                { 0x015, new CMwFieldInfo("GlobalCatalog", 0x00000005) },
		                { 0x016, new CMwFieldInfo("Network", 0x00000005) },
		                { 0x017, new CMwFieldInfo("CanModifyWithoutInvalidate", 0x00000001) },
		                { 0x018, new CMwFieldInfo("DefaultCollectorVehicle", 0x00000005) },
		                { 0x019, new CMwFieldInfo("NcVehicleId", 0x00000029) },
		                { 0x01A, new CMwFieldInfo("NcCollectorV", 0x00000005) },
		                { 0x01B, new CMwFieldInfo("StereoscopyEnable", 0x00000001) }
	                  })
	                },
	                { 0x0D5, new CMwClassInfo("CGameControlCardCtnArticle", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("Icon", 0x00000005) },
		                { 0x001, new CMwFieldInfo("StrName", 0x0000002D) }
	                  })
	                },
	                { 0x0D6, new CMwClassInfo("CGameControlCardCtnCampaign", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("StrName", 0x0000002D) },
		                { 0x001, new CMwFieldInfo("StrRaceType", 0x0000002D) },
		                { 0x002, new CMwFieldInfo("StrEnvironment", 0x0000002D) },
		                { 0x003, new CMwFieldInfo("StrLeagueName", 0x0000002D) },
		                { 0x004, new CMwFieldInfo("StrSkillRank", 0x0000002D) },
		                { 0x005, new CMwFieldInfo("StrSkillScore", 0x0000002D) },
		                { 0x006, new CMwFieldInfo("IconId", 0x0000001B) },
		                { 0x007, new CMwFieldInfo("NbMedals", 0x0000001F) },
		                { 0x008, new CMwFieldInfo("Name", 0x0000002D) }
	                  })
	                },
	                { 0x0D7, new CMwClassInfo("CGameControlCardChampionship", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("StrPath", 0x0000002D) },
		                { 0x001, new CMwFieldInfo("StrName", 0x0000002D) },
		                { 0x002, new CMwFieldInfo("StrDescription", 0x0000002D) },
		                { 0x003, new CMwFieldInfo("StrLogoUrl", 0x00000029) },
		                { 0x004, new CMwFieldInfo("Logo", 0x00000005) },
		                { 0x005, new CMwFieldInfo("CanSubscribe", 0x00000001) },
		                { 0x006, new CMwFieldInfo("IsPrivate", 0x00000001) }
	                  })
	                },
	                { 0x0D8, new CMwClassInfo("CGameControlCardCtnChapter", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("Icon", 0x00000005) },
		                { 0x001, new CMwFieldInfo("BannerChallenge", 0x00000005) },
		                { 0x002, new CMwFieldInfo("StrName", 0x0000002D) }
	                  })
	                },
	                { 0x0D9, new CMwClassInfo("CGameControlCardCtnGhost", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("StrName", 0x0000002D) },
		                { 0x001, new CMwFieldInfo("Time", 0x0000001F) }
	                  })
	                },
	                { 0x0DA, new CMwClassInfo("CGameControlCardCtnGhostInfo", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("StrName", 0x0000002D) },
		                { 0x001, new CMwFieldInfo("Time", 0x0000001F) }
	                  })
	                },
	                { 0x0DB, new CMwClassInfo("CGameControlCardNetOnlineEvent", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("Id", 0x0000001F) },
		                { 0x001, new CMwFieldInfo("StrStartDate", 0x00000029) },
		                { 0x002, new CMwFieldInfo("StrEndDate", 0x00000029) },
		                { 0x003, new CMwFieldInfo("StrSenderName", 0x0000002D) },
		                { 0x004, new CMwFieldInfo("StrTitle", 0x0000002D) },
		                { 0x005, new CMwFieldInfo("StrContent", 0x0000002D) },
		                { 0x006, new CMwMethodInfo("OnUseManiaCode", 0x00000000, null, null) }
	                  })
	                },
	                { 0x0DC, new CMwClassInfo("CGameControlCardNetTeamInfo", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("StrLogoUrl", 0x00000029) },
		                { 0x001, new CMwFieldInfo("StrHomeUrl", 0x00000029) },
		                { 0x002, new CMwFieldInfo("StrPath", 0x0000002D) },
		                { 0x003, new CMwFieldInfo("StrName", 0x0000002D) },
		                { 0x004, new CMwFieldInfo("StrDescription", 0x0000002D) },
		                { 0x005, new CMwFieldInfo("MembersCount", 0x0000001F) },
		                { 0x006, new CMwFieldInfo("Wins", 0x0000001F) },
		                { 0x007, new CMwFieldInfo("Losses", 0x0000001F) },
		                { 0x008, new CMwFieldInfo("Draws", 0x0000001F) },
		                { 0x009, new CMwFieldInfo("Ranking", 0x0000001F) },
		                { 0x00A, new CMwFieldInfo("Logo", 0x00000005) },
		                { 0x00B, new CMwFieldInfo("NodsMembers", 0x00000007) }
	                  })
	                },
	                { 0x0DD, new CMwClassInfo("CGameControlCardCtnVehicle", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("Icon", 0x00000005) },
		                { 0x001, new CMwFieldInfo("StrName", 0x0000002D) }
	                  })
	                },
	                { 0x0DE, new CMwClassInfo("CGameControlGridCtnCampaign", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("Campaign", 0x00000005) },
		                { 0x001, new CMwFieldInfo("CampaignNameLabel", 0x00000005) },
		                { 0x002, new CMwFieldInfo("ChallengeGroupNamesGrid", 0x00000005) },
		                { 0x003, new CMwEnumInfo("ChallengeGroupAlignment", new string[] { "Left", "Center", "Right" }) },
		                { 0x004, new CMwFieldInfo("ChallengeCardTemplate", 0x00000005) }
	                  })
	                },
	                { 0x0DF, new CMwClassInfo("CGameControlGridCtnChallengeGroup", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("ChallengeGroup", 0x00000005) },
		                { 0x001, new CMwFieldInfo("ChallengeGroupNameLabel", 0x00000005) },
		                { 0x002, new CMwEnumInfo("ChallengeAlignment", new string[] { "Top", "Center", "Bottom" }) }
	                  })
	                },
	                { 0x0E0, new CMwClassInfo("CGameCtnEditorScenePocLink", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("RotationSpeed", 0x00000024) },
		                { 0x001, new CMwFieldInfo("EditorCameraDistance", 0x00000024) },
		                { 0x002, new CMwFieldInfo("EditorCameraAngle", 0x00000024) },
		                { 0x003, new CMwFieldInfo("CurrentHAngle", 0x00000024) },
		                { 0x004, new CMwFieldInfo("TargetHAngle", 0x00000024) }
	                  })
	                },
	                { 0x0E1, new CMwClassInfo("CGameAnalyzer", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("Scene", 0x00000005) },
		                { 0x001, new CMwFieldInfo("ContainerLeft", 0x00000005) },
		                { 0x002, new CMwFieldInfo("ContainerRight", 0x00000005) },
		                { 0x003, new CMwFieldInfo("Mode", 0x0000000E) },
		                { 0x004, new CMwFieldInfo("FrameRate", 0x00000005) }
	                  })
	                },
	                { 0x0E2, new CMwClassInfo("CGamePlaygroundInterface", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("InterfaceRoot", 0x00000005) }
	                  })
	                },
	                { 0x0E3, new CMwClassInfo("CGamePlayerAttributesLiving", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("Health", 0x00000024) }
	                  })
	                },
	                { 0x0E4, new CMwClassInfo("CGamePlayerScoresShooter", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("TotalFrags", 0x0000001F) },
		                { 0x001, new CMwFieldInfo("TotalScore", 0x00000024) }
	                  })
	                },
	                { 0x0E5, new CMwClassInfo("CGameCtnMediaBlockGhost", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("Ghost", 0x00000005) },
		                { 0x001, new CMwFieldInfo("StartOffset", 0x00000024) }
	                  })
	                },
	                { 0x0E6, new CMwClassInfo("CGameEnvironmentManager", new Dictionary<int, CMwMemberInfo>()
	                  {
	                  })
	                },
	                { 0x0E7, new CMwClassInfo("CGameDialogShootVideo", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("VideoFps", 0x0000001F) },
		                { 0x001, new CMwFieldInfo("VideoWidth", 0x0000001F) },
		                { 0x002, new CMwFieldInfo("VideoHeight", 0x0000001F) },
		                { 0x003, new CMwFieldInfo("VideoHq", 0x00000001) },
		                { 0x004, new CMwEnumInfo("VideoHqSampleSize", new string[] { "1x", "4x", "9x", "16x", "25x" }) },
		                { 0x005, new CMwFieldInfo("VideoHqMB", 0x00000001) },
		                { 0x006, new CMwEnumInfo("VideoStereo3d", new string[] { "None", "Red-Cyan", "Left-Right" }) },
		                { 0x007, new CMwFieldInfo("IsAudioStream", 0x00000001) },
		                { 0x008, new CMwMethodInfo("OnOk", 0x00000000, null, null) },
		                { 0x009, new CMwMethodInfo("OnCancel", 0x00000000, null, null) }
	                  })
	                },
	                { 0x0E8, new CMwClassInfo("CGameManialinkFileEntry", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwMethodInfo("OnFileChoosen", 0x00000000, null, null) }
	                  })
	                },
	                { 0x0E9, new CMwClassInfo("CGameNetDataDownload", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("ReturnedError", 0x0000001F) },
		                { 0x001, new CMwFieldInfo("CheckUpToDate", 0x00000001) },
		                { 0x002, new CMwFieldInfo("IsPaused", 0x00000001) },
		                { 0x003, new CMwFieldInfo("IsFinished", 0x00000001) }
	                  })
	                },
	                { 0x0EA, new CMwClassInfo("CGameCampaignPlayerScores", new Dictionary<int, CMwMemberInfo>()
	                  {
	                  })
	                },
	                { 0x0EB, new CMwClassInfo("CGameLoadProgress", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("Overlay", 0x00000005) }
	                  })
	                },
	                { 0x0EC, new CMwClassInfo("CGameNetFormBuddy", new Dictionary<int, CMwMemberInfo>()
	                  {
	                  })
	                },
	                { 0x0ED, new CMwClassInfo("CGameLadderScoresComputer", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("PlayersCount", 0x0000001F) },
		                { 0x001, new CMwFieldInfo("MatchDuration", 0x0000001F) },
		                { 0x002, new CMwFieldInfo("RefereesCount", 0x0000001F) },
		                { 0x003, new CMwFieldInfo("EnvString", 0x00000029) },
		                { 0x004, new CMwFieldInfo("Player1Ranking", 0x0000001F) },
		                { 0x005, new CMwFieldInfo("Player1Points", 0x00000024) },
		                { 0x006, new CMwFieldInfo("Player2Ranking", 0x0000001F) },
		                { 0x007, new CMwFieldInfo("Player2Points", 0x00000024) },
		                { 0x008, new CMwFieldInfo("Player1WonPoints", 0x00000024) },
		                { 0x009, new CMwFieldInfo("Player2WonPoints", 0x00000024) },
		                { 0x00A, new CMwFieldInfo("Draw", 0x00000001) }
	                  })
	                },
	                { 0x0EE, new CMwClassInfo("CGameLadderScores", new Dictionary<int, CMwMemberInfo>()
	                  {
	                  })
	                }
                  })
                },
                { 0x04, new CMwEngineInfo("Graphic", new Dictionary<int, CMwClassInfo>()
                  {
	                { 0x001, new CMwClassInfo("GxLight", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("Color", 0x00000009) },
		                { 0x001, new CMwFieldInfo("Intensity", 0x00000028) },
		                { 0x002, new CMwFieldInfo("DiffuseIntensity", 0x00000028) },
		                { 0x003, new CMwFieldInfo("SpecularIntens", 0x00000024) },
		                { 0x004, new CMwFieldInfo("SpecularPower", 0x00000024) },
		                { 0x005, new CMwFieldInfo("ShadowIntensity", 0x00000028) },
		                { 0x006, new CMwFieldInfo("ShadowRGB", 0x00000009) },
		                { 0x007, new CMwFieldInfo("DoLighting", 0x00000001) },
		                { 0x008, new CMwFieldInfo("LightMapOnly", 0x00000001) },
		                { 0x009, new CMwFieldInfo("IsInversed", 0x00000001) },
		                { 0x00A, new CMwFieldInfo("IsShadowGen", 0x00000001) },
		                { 0x00B, new CMwFieldInfo("DoSpecular", 0x00000001) },
		                { 0x00C, new CMwFieldInfo("HasLensFlare", 0x00000001) },
		                { 0x00D, new CMwFieldInfo("FlareIntensity", 0x00000028) },
		                { 0x00E, new CMwFieldInfo("HasSprite", 0x00000001) },
		                { 0x00F, new CMwFieldInfo("EnableGroup0", 0x00000001) },
		                { 0x010, new CMwFieldInfo("EnableGroup1", 0x00000001) },
		                { 0x011, new CMwFieldInfo("EnableGroup2", 0x00000001) },
		                { 0x012, new CMwFieldInfo("EnableGroup3", 0x00000001) }
	                  })
	                },
	                { 0x002, new CMwClassInfo("GxLightBall", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("AmbientRGB", 0x00000009) },
		                { 0x001, new CMwFieldInfo("Radius", 0x00000028) },
		                { 0x002, new CMwFieldInfo("CustomRadiusSpecular", 0x00000001) },
		                { 0x003, new CMwFieldInfo("RadiusSpecular", 0x00000028) },
		                { 0x004, new CMwFieldInfo("CustomRadiusShadow", 0x00000001) },
		                { 0x005, new CMwFieldInfo("RadiusShadow", 0x00000028) },
		                { 0x006, new CMwFieldInfo("CustomRadiusFlare", 0x00000001) },
		                { 0x007, new CMwFieldInfo("RadiusFlare", 0x00000028) },
		                { 0x008, new CMwFieldInfo("EmittingRadius", 0x00000028) },
		                { 0x009, new CMwEnumInfo("AttenuationType", new string[] { "Hyperbolic", "1-(D/R)^2" }) },
		                { 0x00A, new CMwFieldInfo("Attenuation1", 0x00000028) },
		                { 0x00B, new CMwFieldInfo("Attenuation2", 0x00000028) }
	                  })
	                },
	                { 0x003, new CMwClassInfo("GxLightPoint", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("FlareSize", 0x00000024) },
		                { 0x001, new CMwFieldInfo("FlareBiasZ", 0x00000024) }
	                  })
	                },
	                { 0x004, new CMwClassInfo("GxFog", new Dictionary<int, CMwMemberInfo>()
	                  {
	                  })
	                },
	                { 0x005, new CMwClassInfo("GxLightAmbient", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("ShadeMinY", 0x00000024) },
		                { 0x001, new CMwFieldInfo("ShadeMaxY", 0x00000024) }
	                  })
	                },
	                { 0x006, new CMwClassInfo("GxLightNotAmbient", new Dictionary<int, CMwMemberInfo>()
	                  {
	                  })
	                },
	                { 0x007, new CMwClassInfo("GxLightDirectional", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("DblSidedRGB", 0x00000009) },
		                { 0x001, new CMwFieldInfo("ReverseRGB", 0x00000009) },
		                { 0x002, new CMwFieldInfo("ReverseIntens", 0x00000028) },
		                { 0x003, new CMwFieldInfo("EmittAngularSize", 0x00000024) },
		                { 0x004, new CMwFieldInfo("FlareAngularSize", 0x00000024) },
		                { 0x005, new CMwFieldInfo("FlareIntensPower", 0x00000028) },
		                { 0x006, new CMwFieldInfo("UseBoundaryHint", 0x00000001) },
		                { 0x007, new CMwFieldInfo("BoundaryHintPos", 0x00000035) },
		                { 0x008, new CMwFieldInfo("DazzleAngleMax", 0x00000024) },
		                { 0x009, new CMwFieldInfo("DazzleIntensity", 0x00000028) }
	                  })
	                },
	                { 0x008, new CMwClassInfo("GxFogBlender", new Dictionary<int, CMwMemberInfo>()
	                  {
	                  })
	                },
	                { 0x00A, new CMwClassInfo("GxLightFrustum", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("IsOrtho", 0x00000001) },
		                { 0x001, new CMwFieldInfo("NearZ", 0x00000024) },
		                { 0x002, new CMwFieldInfo("FarZ", 0x00000024) },
		                { 0x003, new CMwFieldInfo("FovY", 0x00000024) },
		                { 0x004, new CMwFieldInfo("RatioXY", 0x00000024) },
		                { 0x005, new CMwFieldInfo("SizeX", 0x00000024) },
		                { 0x006, new CMwFieldInfo("SizeY", 0x00000024) },
		                { 0x007, new CMwFieldInfo("DoAttenuation", 0x00000001) },
		                { 0x008, new CMwEnumInfo("Apply", new string[] { "ModulateAdd", "Modulate", "Add", "ModulateX2" }) },
		                { 0x009, new CMwEnumInfo("Technique", new string[] { "RenderCube", "2dMaskNoClipZ", "2dBallLight", "GenShadowMask" }) },
		                { 0x00A, new CMwFieldInfo("iShadowGroup", 0x0000001F) },
		                { 0x00B, new CMwFieldInfo("DoFadeZ", 0x00000001) },
		                { 0x00C, new CMwFieldInfo("RatioFadeZ", 0x00000028) },
		                { 0x00D, new CMwFieldInfo("UseFacePosX", 0x00000001) },
		                { 0x00E, new CMwFieldInfo("UseFaceNegX", 0x00000001) },
		                { 0x00F, new CMwFieldInfo("UseFacePosY", 0x00000001) },
		                { 0x010, new CMwFieldInfo("UseFaceNegY", 0x00000001) },
		                { 0x011, new CMwFieldInfo("UseFacePosZ", 0x00000001) },
		                { 0x012, new CMwFieldInfo("UseFaceNegZ", 0x00000001) }
	                  })
	                },
	                { 0x00B, new CMwClassInfo("GxLightSpot", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("AngleInner", 0x00000028) },
		                { 0x001, new CMwFieldInfo("AngleOuter", 0x00000028) },
		                { 0x002, new CMwFieldInfo("CustomAngleFlare", 0x00000001) },
		                { 0x003, new CMwFieldInfo("AngleFlare", 0x00000028) },
		                { 0x004, new CMwFieldInfo("CustomAngleShadow", 0x00000001) },
		                { 0x005, new CMwFieldInfo("AngleInnerShadow", 0x00000028) },
		                { 0x006, new CMwFieldInfo("AngleOuterShadow", 0x00000028) },
		                { 0x007, new CMwFieldInfo("FalloffExponent", 0x00000024) }
	                  })
	                }
                  })
                },
                { 0x05, new CMwEngineInfo("Function", new Dictionary<int, CMwClassInfo>()
                  {
	                { 0x002, new CMwClassInfo("CFuncKeys", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("Xs", 0x00000025) },
		                { 0x001, new CMwMethodInfo("Reset", 0x00000000, null, null) }
	                  })
	                },
	                { 0x003, new CMwClassInfo("CFuncKeysTrans", new Dictionary<int, CMwMemberInfo>()
	                  {
	                  })
	                },
	                { 0x004, new CMwClassInfo("CFuncKeysTransQuat", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwMethodInfo("AddTransQuatKey", 0x00000041,
			                new uint[] { 0x00000024, 0x00000035, 0x00000035, 0x00000035 },
			                new string[] { "X", "Pos", "Dir", "Up" })
		                }
	                  })
	                },
	                { 0x005, new CMwClassInfo("CFuncSkel", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("BonesCount", 0x0000001F) }
	                  })
	                },
	                { 0x006, new CMwClassInfo("CFuncKeysSkel", new Dictionary<int, CMwMemberInfo>()
	                  {
	                  })
	                },
	                { 0x007, new CMwClassInfo("CFuncSkelValues", new Dictionary<int, CMwMemberInfo>()
	                  {
	                  })
	                },
	                { 0x008, new CMwClassInfo("CFuncKeysCmd", new Dictionary<int, CMwMemberInfo>()
	                  {
	                  })
	                },
	                { 0x00A, new CMwClassInfo("CFuncKeysPath", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwEnumInfo("DrawMode", new string[] { "Line", "BSpline", "BSplineTension", "BSplineBias", "BezierSpline", "BetaSpline", "Hermite" }) },
		                { 0x001, new CMwFieldInfo("BSplineTension", 0x00000028) },
		                { 0x002, new CMwFieldInfo("BSplineBias", 0x00000028) },
		                { 0x003, new CMwFieldInfo("BetaSplineTension", 0x00000028) },
		                { 0x004, new CMwFieldInfo("BetaSplineSkew", 0x00000028) },
		                { 0x005, new CMwFieldInfo("UseTangentOrientation", 0x00000001) }
	                  })
	                },
	                { 0x00B, new CMwClassInfo("CFuncPlug", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("Period", 0x00000024) },
		                { 0x001, new CMwFieldInfo("Phase", 0x00000024) },
		                { 0x002, new CMwFieldInfo("AutoCreateMotion", 0x00000001) },
		                { 0x003, new CMwFieldInfo("RandomizePhase", 0x00000001) },
		                { 0x004, new CMwFieldInfo("InputVal", 0x0000001B) }
	                  })
	                },
	                { 0x00C, new CMwClassInfo("CFuncLightIntensity", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("Intensity0", 0x00000024) },
		                { 0x001, new CMwFieldInfo("Intensity1", 0x00000024) },
		                { 0x002, new CMwFieldInfo("FlickerDuration", 0x00000024) }
	                  })
	                },
	                { 0x00D, new CMwClassInfo("CFuncTreeTranslate", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("StartPoint", 0x00000035) },
		                { 0x001, new CMwFieldInfo("EndPoint", 0x00000035) },
		                { 0x002, new CMwFieldInfo("IsPingPong", 0x00000001) },
		                { 0x003, new CMwFieldInfo("IsSmooth", 0x00000001) }
	                  })
	                },
	                { 0x00E, new CMwClassInfo("CFuncEnum", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("Values", 0x00000006) },
		                { 0x001, new CMwFieldInfo("WantedCount", 0x0000001F) },
		                { 0x002, new CMwFieldInfo("AtlasX", 0x0000001F) },
		                { 0x003, new CMwFieldInfo("AtlasY", 0x0000001F) },
		                { 0x004, new CMwFieldInfo("ValuesAtlasYs", 0x00000020) },
		                { 0x005, new CMwFieldInfo("MinTexCoord", 0x00000031) },
		                { 0x006, new CMwFieldInfo("MaxTexCoord", 0x00000031) },
		                { 0x007, new CMwFieldInfo("TexSize", 0x00000031) },
		                { 0x008, new CMwFieldInfo("IconIndexs", 0x00000005) }
	                  })
	                },
	                { 0x00F, new CMwClassInfo("CFuncSin", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("In", 0x00000024) },
		                { 0x001, new CMwFieldInfo("Out", 0x00000024) }
	                  })
	                },
	                { 0x010, new CMwClassInfo("CFunc", new Dictionary<int, CMwMemberInfo>()
	                  {
	                  })
	                },
	                { 0x011, new CMwClassInfo("CFuncShader", new Dictionary<int, CMwMemberInfo>()
	                  {
	                  })
	                },
	                { 0x012, new CMwClassInfo("CFuncKeysVisual", new Dictionary<int, CMwMemberInfo>()
	                  {
	                  })
	                },
	                { 0x013, new CMwClassInfo("CFuncKeysSound", new Dictionary<int, CMwMemberInfo>()
	                  {
	                  })
	                },
	                { 0x014, new CMwClassInfo("CFuncShaders", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("FuncShaders", 0x00000006) }
	                  })
	                },
	                { 0x015, new CMwClassInfo("CFuncShaderLayerUV", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("LayerName", 0x00000029) },
		                { 0x001, new CMwEnumInfo("LayerEnum", new string[] {  }) },
		                { 0x002, new CMwEnumInfo("SignalType", new string[] { "TransLinear", "TransCircular", "Rotate", "TransSubTexture", "TransLinearScale", "CopySubTexture", "Scale", "Reset", "TransSubTexture2", "=>Comp" }) },
		                { 0x003, new CMwFieldInfo("Base", 0x00000031) },
		                { 0x004, new CMwFieldInfo("Amplitude", 0x00000031) },
		                { 0x005, new CMwFieldInfo("Offset", 0x00000031) },
		                { 0x006, new CMwFieldInfo("Scale", 0x00000031) },
		                { 0x007, new CMwFieldInfo("AngleStart", 0x00000028) },
		                { 0x008, new CMwFieldInfo("AngleEnd", 0x00000028) },
		                { 0x009, new CMwFieldInfo("NbSubTexture", 0x0000001F) },
		                { 0x00A, new CMwFieldInfo("NbSubTexturePerLine", 0x0000001F) },
		                { 0x00B, new CMwFieldInfo("NbSubTexturePerColumn", 0x0000001F) },
		                { 0x00C, new CMwFieldInfo("TopToBottom", 0x00000001) },
		                { 0x00D, new CMwFieldInfo("WriteX", 0x00000001) },
		                { 0x00E, new CMwFieldInfo("WriteY", 0x00000001) },
		                { 0x00F, new CMwFieldInfo("WriteZ", 0x00000001) },
		                { 0x010, new CMwFieldInfo("WriteW", 0x00000001) },
		                { 0x011, new CMwFieldInfo("EnablePingPong", 0x00000001) },
		                { 0x012, new CMwFieldInfo("EnableSmooth", 0x00000001) },
		                { 0x013, new CMwFieldInfo("EnableMipMapping", 0x00000001) },
		                { 0x014, new CMwFieldInfo("EnableBlending", 0x00000001) },
		                { 0x015, new CMwFieldInfo("EnableSmoothBlend", 0x00000001) },
		                { 0x016, new CMwFieldInfo("BitmapCopy", 0x00000005) }
	                  })
	                },
	                { 0x016, new CMwClassInfo("CFuncShaderFxFactor", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwEnumInfo("MapFx", new string[] { "SelfIllumLuminance", "LightMapIntensity" }) },
		                { 0x001, new CMwEnumInfo("Type", new string[] { "Linear", "Smooth", "Refresh glow" }) },
		                { 0x002, new CMwFieldInfo("Base", 0x00000024) },
		                { 0x003, new CMwFieldInfo("Amplitude", 0x00000024) },
		                { 0x004, new CMwFieldInfo("Offset", 0x00000024) }
	                  })
	                },
	                { 0x017, new CMwClassInfo("CFuncColor", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("Blend", 0x00000024) },
		                { 0x001, new CMwFieldInfo("Color0", 0x00000009) },
		                { 0x002, new CMwFieldInfo("Color1", 0x00000009) },
		                { 0x003, new CMwFieldInfo("OutColor", 0x00000009) }
	                  })
	                },
	                { 0x018, new CMwClassInfo("CFuncLight", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwEnumInfo("FctType", new string[] { "Sinus", "Flick" }) },
		                { 0x001, new CMwFieldInfo("FlickPeriod", 0x00000024) },
		                { 0x002, new CMwFieldInfo("FlickCount", 0x0000001F) }
	                  })
	                },
	                { 0x019, new CMwClassInfo("CFuncLightColor", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("Color0", 0x00000009) },
		                { 0x001, new CMwFieldInfo("Color1", 0x00000009) },
		                { 0x002, new CMwFieldInfo("Image", 0x00000005) }
	                  })
	                },
	                { 0x01A, new CMwClassInfo("CFuncKeysReal", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwEnumInfo("RealInterp", new string[] { "Linear", "None" }) },
		                { 0x001, new CMwFieldInfo("Ys", 0x00000025) }
	                  })
	                },
	                { 0x01B, new CMwClassInfo("CFuncVisual", new Dictionary<int, CMwMemberInfo>()
	                  {
	                  })
	                },
	                { 0x01C, new CMwClassInfo("CFuncTree", new Dictionary<int, CMwMemberInfo>()
	                  {
	                  })
	                },
	                { 0x01D, new CMwClassInfo("CFuncVisualShiver", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("Amplitude", 0x00000024) },
		                { 0x001, new CMwFieldInfo("OffsetPlane", 0x00000035) },
		                { 0x002, new CMwEnumInfo("ShiverType", new string[] { "Normal", "Axis" }) },
		                { 0x003, new CMwFieldInfo("Axis", 0x00000035) },
		                { 0x004, new CMwFieldInfo("UsePlane", 0x00000001) },
		                { 0x005, new CMwFieldInfo("PlanePoint", 0x00000035) },
		                { 0x006, new CMwFieldInfo("PlaneNormal", 0x00000035) },
		                { 0x007, new CMwFieldInfo("PlaneMinDist", 0x00000024) },
		                { 0x008, new CMwFieldInfo("UsePlane2", 0x00000001) },
		                { 0x009, new CMwFieldInfo("Plane2Point", 0x00000035) },
		                { 0x00A, new CMwFieldInfo("Plane2Normal", 0x00000035) },
		                { 0x00B, new CMwFieldInfo("Plane2MinDist", 0x00000024) }
	                  })
	                },
	                { 0x01E, new CMwClassInfo("CFuncTreeRotate", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("AngleMin", 0x00000024) },
		                { 0x001, new CMwFieldInfo("AngleMax", 0x00000024) }
	                  })
	                },
	                { 0x01F, new CMwClassInfo("CFuncTreeBend", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("Amplitude", 0x00000024) }
	                  })
	                },
	                { 0x020, new CMwClassInfo("CFuncPathMesh", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("Visual", 0x00000005) },
		                { 0x001, new CMwFieldInfo("OffsetVisualPos", 0x00000035) },
		                { 0x002, new CMwFieldInfo("Locations", 0x00000006) }
	                  })
	                },
	                { 0x021, new CMwClassInfo("CFuncPathMeshLocation", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("Name", 0x00000029) },
		                { 0x001, new CMwFieldInfo("Location", 0x00000013) }
	                  })
	                },
	                { 0x02A, new CMwClassInfo("CFuncKeysReals", new Dictionary<int, CMwMemberInfo>()
	                  {
	                  })
	                },
	                { 0x02B, new CMwClassInfo("CFuncVisualBlendShapeSequence", new Dictionary<int, CMwMemberInfo>()
	                  {
	                  })
	                },
	                { 0x02C, new CMwClassInfo("CFuncManagerCharacter", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("Groups", 0x00000007) }
	                  })
	                },
	                { 0x02D, new CMwClassInfo("CFuncManagerCharacterAdv", new Dictionary<int, CMwMemberInfo>()
	                  {
	                  })
	                },
	                { 0x02E, new CMwClassInfo("CFuncGroup", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("IdName", 0x00000029) },
		                { 0x001, new CMwFieldInfo("Elems", 0x00000007) }
	                  })
	                },
	                { 0x02F, new CMwClassInfo("CFuncGroupElem", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("IdName", 0x00000029) }
	                  })
	                },
	                { 0x030, new CMwClassInfo("CFuncKeysNatural", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("Naturals", 0x00000020) }
	                  })
	                },
	                { 0x031, new CMwClassInfo("CFuncTreeSubVisualSequence", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("SubKeys", 0x00000005) },
		                { 0x001, new CMwFieldInfo("SimpleModeIsLooping", 0x00000001) },
		                { 0x002, new CMwFieldInfo("SimpleModeStartIndex", 0x0000001F) },
		                { 0x003, new CMwFieldInfo("SimpleModeEndIndex", 0x0000001F) }
	                  })
	                },
	                { 0x032, new CMwClassInfo("CFuncTreeElevator", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("LevelCount", 0x0000001F) },
		                { 0x001, new CMwFieldInfo("LevelHeight", 0x00000024) },
		                { 0x002, new CMwFieldInfo("MoveStepDuration", 0x00000028) },
		                { 0x003, new CMwFieldInfo("MoveAxis", 0x00000035) }
	                  })
	                },
	                { 0x033, new CMwClassInfo("CFuncShaderTweakKeysTranss", new Dictionary<int, CMwMemberInfo>()
	                  {
	                  })
	                },
	                { 0x034, new CMwClassInfo("CFuncWeather", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("FuncClouds", 0x00000005) },
		                { 0x001, new CMwFieldInfo("MaterialSky_Night", 0x00000005) },
		                { 0x002, new CMwFieldInfo("MaterialSky_SunRise", 0x00000005) },
		                { 0x003, new CMwFieldInfo("MaterialSky_Day", 0x00000005) },
		                { 0x004, new CMwFieldInfo("MaterialSky_SunFall", 0x00000005) },
		                { 0x005, new CMwFieldInfo("MaterialSea0", 0x00000005) },
		                { 0x006, new CMwFieldInfo("MaterialSea1", 0x00000005) },
		                { 0x007, new CMwFieldInfo("ImageLightAmb", 0x00000005) },
		                { 0x008, new CMwFieldInfo("ImageLightDirSun", 0x00000005) },
		                { 0x009, new CMwFieldInfo("ImageLightDirMoon", 0x00000005) },
		                { 0x00A, new CMwFieldInfo("ImageLightDirDblSided", 0x00000005) },
		                { 0x00B, new CMwFieldInfo("BitmapFlareSun", 0x00000005) },
		                { 0x00C, new CMwFieldInfo("FlareAngularSizeSun", 0x00000024) },
		                { 0x00D, new CMwFieldInfo("BitmapFlareMoon", 0x00000005) },
		                { 0x00E, new CMwFieldInfo("FlareAngularSizeMoon", 0x00000024) },
		                { 0x00F, new CMwFieldInfo("BitmapSkyGradV", 0x00000005) },
		                { 0x010, new CMwFieldInfo("ImageFogColor", 0x00000005) },
		                { 0x011, new CMwFieldInfo("ImageSeaColor", 0x00000005) },
		                { 0x012, new CMwFieldInfo("BitmapRainFid", 0x00000005) },
		                { 0x013, new CMwFieldInfo("SceneFxFid", 0x00000005) },
		                { 0x014, new CMwFieldInfo("CameraFarZ", 0x00000024) },
		                { 0x015, new CMwFieldInfo("FogByVertex", 0x00000001) },
		                { 0x016, new CMwFieldInfo("FogRGB", 0x00000009) },
		                { 0x017, new CMwEnumInfo("FogFormula", new string[] { "None", "Exp", "Exp2", "Linear" }) },
		                { 0x018, new CMwEnumInfo("FogSpace", new string[] { "CameraFarZ", "World" }) },
		                { 0x019, new CMwFieldInfo("FogLinearStart", 0x00000024) },
		                { 0x01A, new CMwFieldInfo("FogLinearEnd", 0x00000024) },
		                { 0x01B, new CMwFieldInfo("FogExpDensity", 0x00000024) },
		                { 0x01C, new CMwFieldInfo("DayFogColor", 0x00000009) },
		                { 0x01D, new CMwFieldInfo("DayFogStart", 0x00000024) },
		                { 0x01E, new CMwFieldInfo("DayFogEnd", 0x00000024) },
		                { 0x01F, new CMwFieldInfo("DayFogDensity", 0x00000024) },
		                { 0x020, new CMwFieldInfo("FogBlender", 0x00000005) },
		                { 0x021, new CMwMethodInfo("SeaTwkResetToShaderDefaults", 0x00000000, null, null) },
		                { 0x022, new CMwFieldInfo("LDirSpecIntens", 0x00000031) },
		                { 0x023, new CMwFieldInfo("LDirSpecPower", 0x00000031) },
		                { 0x024, new CMwFieldInfo("SeaTwkReflecIntensNight", 0x00000028) },
		                { 0x025, new CMwFieldInfo("SeaTwkReflecIntensDay", 0x00000028) },
		                { 0x026, new CMwFieldInfo("SeaTwkReflecIntensMidNight", 0x00000028) },
		                { 0x027, new CMwFieldInfo("SeaTwkReflecIntensMidDay", 0x00000028) },
		                { 0x028, new CMwFieldInfo("SeaTwkReflecIntensTMNight", 0x00000028) },
		                { 0x029, new CMwFieldInfo("SeaTwkReflecIntensTMDay", 0x00000028) },
		                { 0x02A, new CMwFieldInfo("SeaTwkReflecIntensTMmidNight", 0x00000028) },
		                { 0x02B, new CMwFieldInfo("SeaTwkReflecIntensTMmidDay", 0x00000028) },
		                { 0x02C, new CMwFieldInfo("SeaTwkWaterColor_Night", 0x00000009) },
		                { 0x02D, new CMwFieldInfo("SeaTwkWaterColor_Day", 0x00000009) }
	                  })
	                },
	                { 0x035, new CMwClassInfo("CFuncPuffLull", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("TileSizeInWorld", 0x00000024) },
		                { 0x001, new CMwFieldInfo("PuffWDMax", 0x00000028) },
		                { 0x002, new CMwFieldInfo("LullWDMax", 0x00000028) },
		                { 0x003, new CMwFieldInfo("GenCount", 0x0000001F) },
		                { 0x004, new CMwFieldInfo("GenPuffRatio", 0x00000028) },
		                { 0x005, new CMwFieldInfo("GenSize", 0x00000031) },
		                { 0x006, new CMwFieldInfo("GenPuffWDMin", 0x00000028) },
		                { 0x007, new CMwFieldInfo("GenPuffWDMax", 0x00000028) },
		                { 0x008, new CMwFieldInfo("GenLullWDMin", 0x00000028) },
		                { 0x009, new CMwFieldInfo("GenLullWDMax", 0x00000028) },
		                { 0x00A, new CMwFieldInfo("GenLifeTimeMin", 0x00000029) },
		                { 0x00B, new CMwFieldInfo("GenLifeTimeMax", 0x00000029) },
		                { 0x00C, new CMwFieldInfo("BlendPuff", 0x00000028) },
		                { 0x00D, new CMwFieldInfo("BlendLull", 0x00000028) },
		                { 0x00E, new CMwFieldInfo("Combine2nd8th", 0x00000028) },
		                { 0x00F, new CMwFieldInfo("MaterialPuff", 0x00000005) },
		                { 0x010, new CMwFieldInfo("MaterialLull", 0x00000005) }
	                  })
	                },
	                { 0x036, new CMwClassInfo("CFuncEnvelope", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("KeyFrameValue0", 0x00000024) },
		                { 0x001, new CMwFieldInfo("KeyFrameValue1", 0x00000024) },
		                { 0x002, new CMwFieldInfo("KeyFrameValue2", 0x00000024) },
		                { 0x003, new CMwFieldInfo("KeyFrameValue3", 0x00000024) },
		                { 0x004, new CMwFieldInfo("KeyFramePos1", 0x00000024) },
		                { 0x005, new CMwFieldInfo("KeyFramePos2", 0x00000024) },
		                { 0x006, new CMwFieldInfo("Frequency", 0x00000024) },
		                { 0x007, new CMwFieldInfo("Amplitude", 0x00000024) },
		                { 0x008, new CMwEnumInfo("ModFunc", new string[] { "Cos", "Sin" }) }
	                  })
	                },
	                { 0x037, new CMwClassInfo("CFuncSegment", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("KeyCount", 0x0000001F) }
	                  })
	                },
	                { 0x038, new CMwClassInfo("CFuncColorGradient", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("KeyFrameValue0", 0x00000009) },
		                { 0x001, new CMwFieldInfo("KeyFrameValue1", 0x00000009) },
		                { 0x002, new CMwFieldInfo("KeyFrameValue2", 0x00000009) },
		                { 0x003, new CMwFieldInfo("KeyFrameValue3", 0x00000009) },
		                { 0x004, new CMwFieldInfo("KeyFramePos1", 0x00000024) },
		                { 0x005, new CMwFieldInfo("KeyFramePos2", 0x00000024) }
	                  })
	                },
	                { 0x039, new CMwClassInfo("CFuncFullColorGradient", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("KeyFrameValue0", 0x00000009) },
		                { 0x001, new CMwFieldInfo("KeyFrameValue1", 0x00000009) },
		                { 0x002, new CMwFieldInfo("KeyFrameValue2", 0x00000009) },
		                { 0x003, new CMwFieldInfo("KeyFrameValue3", 0x00000009) },
		                { 0x004, new CMwFieldInfo("KeyFramePos1", 0x00000024) },
		                { 0x005, new CMwFieldInfo("KeyFramePos2", 0x00000024) }
	                  })
	                },
	                { 0x03A, new CMwClassInfo("CFuncClouds", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("SolidFids", 0x00000007) },
		                { 0x001, new CMwEnumInfo("HeightCenter", new string[] { "  Camera", "  WorldXZ" }) },
		                { 0x002, new CMwFieldInfo("HeightCenterXZ", 0x00000031) },
		                { 0x003, new CMwFieldInfo("BottomNearZ", 0x00000024) },
		                { 0x004, new CMwFieldInfo("PointDists", 0x00000026) },
		                { 0x005, new CMwFieldInfo("PointHeights", 0x00000026) },
		                { 0x006, new CMwFieldInfo("BottomFarZ", 0x00000024) },
		                { 0x007, new CMwFieldInfo("SpeedScale", 0x00000024) },
		                { 0x008, new CMwEnumInfo("Lighting", new string[] { "per object", "per sprite" }) },
		                { 0x009, new CMwFieldInfo("ImageColorMin", 0x00000005) },
		                { 0x00A, new CMwFieldInfo("ImageColorMax", 0x00000005) }
	                  })
	                },
	                { 0x03C, new CMwClassInfo("CCurveInterface", new Dictionary<int, CMwMemberInfo>()
	                  {
	                  })
	                },
	                { 0x042, new CMwClassInfo("CFuncCurvesReal", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("Id", 0x0000001B) },
		                { 0x001, new CMwFieldInfo("Xs", 0x00000026) },
		                { 0x002, new CMwFieldInfo("Curves", 0x00000007) }
	                  })
	                },
	                { 0x043, new CMwClassInfo("CFuncCurves2Real", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("Id", 0x0000001B) },
		                { 0x001, new CMwFieldInfo("Xs", 0x00000026) },
		                { 0x002, new CMwFieldInfo("Curves2", 0x00000007) }
	                  })
	                },
	                { 0x044, new CMwClassInfo("CFuncNoise", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("ScaleX", 0x00000024) },
		                { 0x001, new CMwFieldInfo("ScaleY", 0x00000024) },
		                { 0x002, new CMwFieldInfo("ScaleZ", 0x00000024) },
		                { 0x003, new CMwFieldInfo("ScaleT", 0x00000024) },
		                { 0x004, new CMwFieldInfo("PeriodX", 0x00000024) },
		                { 0x005, new CMwFieldInfo("PeriodY", 0x00000024) },
		                { 0x006, new CMwFieldInfo("PeriodZ", 0x00000024) },
		                { 0x007, new CMwFieldInfo("PeriodT", 0x00000024) },
		                { 0x008, new CMwFieldInfo("ValMin", 0x00000024) },
		                { 0x009, new CMwFieldInfo("ValMax", 0x00000024) }
	                  })
	                }
                  })
                },
                { 0x06, new CMwEngineInfo("Hms", new Dictionary<int, CMwClassInfo>()
                  {
	                { 0x001, new CMwClassInfo("CHmsCamera", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("PickEnable", 0x00000001) },
		                { 0x001, new CMwFieldInfo("UseViewDependantRendering", 0x00000001) },
		                { 0x002, new CMwEnumInfo("ViewportRatio", new string[] { "None", "FovY", "FovX" }) },
		                { 0x003, new CMwEnumInfo("iPrecalcRender", new string[] {  }) },
		                { 0x004, new CMwFieldInfo("NextLocation", 0x00000013) },
		                { 0x005, new CMwMethodInfo("ResetLocation", 0x00000000, null, null) },
		                { 0x006, new CMwFieldInfo("ClearColorEnable", 0x00000001) },
		                { 0x007, new CMwFieldInfo("ClearColor", 0x00000009) },
		                { 0x008, new CMwFieldInfo("UseZBuffer", 0x00000001) },
		                { 0x009, new CMwFieldInfo("ScissorEnable", 0x00000001) },
		                { 0x00A, new CMwFieldInfo("FovRectEnable", 0x00000001) },
		                { 0x00B, new CMwFieldInfo("ClearZBuffer", 0x00000001) },
		                { 0x00C, new CMwFieldInfo("DrawRectMin", 0x00000031) },
		                { 0x00D, new CMwFieldInfo("DrawRectMax", 0x00000031) },
		                { 0x00E, new CMwFieldInfo("ScissorRectMin", 0x00000031) },
		                { 0x00F, new CMwFieldInfo("ScissorRectMax", 0x00000031) },
		                { 0x010, new CMwFieldInfo("FovRectMin", 0x00000031) },
		                { 0x011, new CMwFieldInfo("FovRectMax", 0x00000031) },
		                { 0x012, new CMwFieldInfo("NearZ", 0x00000024) },
		                { 0x013, new CMwFieldInfo("FarZ", 0x00000024) },
		                { 0x014, new CMwFieldInfo("Fov", 0x00000024) },
		                { 0x015, new CMwFieldInfo("RatioXY", 0x00000024) },
		                { 0x016, new CMwFieldInfo("Picker", 0x00000005) },
		                { 0x017, new CMwFieldInfo("FocusZ", 0x00000024) },
		                { 0x018, new CMwEnumInfo("LensMode", new string[] { "Focal", "Size" }) },
		                { 0x019, new CMwFieldInfo("LensFocal", 0x00000024) },
		                { 0x01A, new CMwFieldInfo("LensSize", 0x00000024) },
		                { 0x01B, new CMwFieldInfo("AsyncPrevDeltaT", 0x00000024) },
		                { 0x01C, new CMwFieldInfo("AsyncPrevDeltaRotationScale", 0x00000028) },
		                { 0x01D, new CMwFieldInfo("ZClipEnable", 0x00000001) },
		                { 0x01E, new CMwFieldInfo("ZClipValue", 0x00000024) },
		                { 0x01F, new CMwFieldInfo("ZClipMargin", 0x00000024) },
		                { 0x020, new CMwFieldInfo("ZClipZBuffer1End", 0x00000028) },
		                { 0x021, new CMwFieldInfo("ZClipZBuffer2Start", 0x00000028) },
		                { 0x022, new CMwFieldInfo("TargetFpsEnable", 0x00000001) },
		                { 0x023, new CMwFieldInfo("TargetFpsZClipSpeed", 0x00000028) },
		                { 0x024, new CMwFieldInfo("TargetFpsZClipMinValue", 0x00000024) }
	                  })
	                },
	                { 0x002, new CMwClassInfo("CHmsCorpus", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("Item", 0x00000005) }
	                  })
	                },
	                { 0x003, new CMwClassInfo("CHmsItem", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("Solid", 0x00000005) },
		                { 0x001, new CMwFieldInfo("Corpuss", 0x00000007) },
		                { 0x002, new CMwFieldInfo("Portals", 0x00000006) },
		                { 0x003, new CMwEnumInfo("CollisionGroup", new string[] { "None", "Detection", "Dummy", "Actor", "Build", "Ghost" }) },
		                { 0x004, new CMwEnumInfo("DynamicType", new string[] { "None", "Point", "Solid", "Replacement" }) },
		                { 0x005, new CMwEnumInfo("ContactInterest", new string[] { "None", "Minimal", "Standard", "Full" }) },
		                { 0x006, new CMwFieldInfo("KinematicOnly", 0x00000001) },
		                { 0x007, new CMwFieldInfo("IsVisionStatic", 0x00000001) },
		                { 0x008, new CMwFieldInfo("IsCollisionStatic", 0x00000001) },
		                { 0x009, new CMwFieldInfo("IsBackground", 0x00000001) },
		                { 0x00A, new CMwFieldInfo("CopyCameraTranslationXZ", 0x00000001) },
		                { 0x00B, new CMwFieldInfo("BackgroundZClipCullBefore", 0x00000001) },
		                { 0x00C, new CMwFieldInfo("UseAccurateBBoxTest", 0x00000001) },
		                { 0x00D, new CMwFieldInfo("IsNotOccluderForLightMap", 0x00000001) },
		                { 0x00E, new CMwFieldInfo("IsForcePointDynamicCollisionResponse", 0x00000001) },
		                { 0x00F, new CMwFieldInfo("CountShadowTexCasted", 0x0000001F) },
		                { 0x010, new CMwFieldInfo("CanSelfShadow", 0x00000001) },
		                { 0x011, new CMwFieldInfo("CanFakeShadow", 0x00000001) },
		                { 0x012, new CMwFieldInfo("CastShadowGrp0", 0x00000001) },
		                { 0x013, new CMwFieldInfo("CastShadowGrp1", 0x00000001) },
		                { 0x014, new CMwFieldInfo("CastShadowGrp2", 0x00000001) },
		                { 0x015, new CMwFieldInfo("CastShadowGrp3", 0x00000001) },
		                { 0x016, new CMwFieldInfo("RecvShadowGrp0", 0x00000001) },
		                { 0x017, new CMwFieldInfo("RecvShadowGrp1", 0x00000001) },
		                { 0x018, new CMwFieldInfo("RecvShadowGrp2", 0x00000001) },
		                { 0x019, new CMwFieldInfo("RecvShadowGrp3", 0x00000001) },
		                { 0x01A, new CMwFieldInfo("ItemShadow", 0x00000005) },
		                { 0x01B, new CMwFieldInfo("IsProjectorReceiver", 0x00000001) },
		                { 0x01C, new CMwFieldInfo("LightLensFlareEnable", 0x00000001) },
		                { 0x01D, new CMwFieldInfo("LightEGroup0", 0x00000001) },
		                { 0x01E, new CMwFieldInfo("LightEGroup1", 0x00000001) },
		                { 0x01F, new CMwFieldInfo("LightEGroup2", 0x00000001) },
		                { 0x020, new CMwFieldInfo("LightEGroup3", 0x00000001) },
		                { 0x021, new CMwFieldInfo("VIdReflected", 0x00000001) },
		                { 0x022, new CMwFieldInfo("VIdReflectMirror", 0x00000001) },
		                { 0x023, new CMwFieldInfo("VIdRefracted", 0x00000001) },
		                { 0x024, new CMwFieldInfo("VIdViewDepBump", 0x00000001) },
		                { 0x025, new CMwFieldInfo("VIdViewDepOcclusion", 0x00000001) },
		                { 0x026, new CMwFieldInfo("VIdOnlyRefracted", 0x00000001) },
		                { 0x027, new CMwFieldInfo("VIdHideWhenUnderground", 0x00000001) },
		                { 0x028, new CMwFieldInfo("VIdHideWhenOverground", 0x00000001) },
		                { 0x029, new CMwFieldInfo("VIdHideAlways", 0x00000001) },
		                { 0x02A, new CMwFieldInfo("VIdViewDepWindIntens", 0x00000001) },
		                { 0x02B, new CMwFieldInfo("VIdBackground", 0x00000001) },
		                { 0x02C, new CMwFieldInfo("VIdGrassRGB", 0x00000001) },
		                { 0x02D, new CMwFieldInfo("VIdLightGenP", 0x00000001) },
		                { 0x02E, new CMwFieldInfo("VIdVehicle", 0x00000001) },
		                { 0x02F, new CMwFieldInfo("VIdHideOnlyDirect", 0x00000001) },
		                { 0x030, new CMwFieldInfo("IsVisible", 0x00000001) },
		                { 0x031, new CMwMethodInfo("AddImpulse", 0x00000041,
			                new uint[] { 0x00000035, 0x00000035 },
			                new string[] { "Direction", "Origin" })
		                },
		                { 0x032, new CMwFieldInfo("AngularSpeed", 0x00000035) },
		                { 0x033, new CMwFieldInfo("LinearSpeed", 0x00000035) }
	                  })
	                },
	                { 0x004, new CMwClassInfo("CHmsZone", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("IsVisible", 0x00000001) },
		                { 0x001, new CMwFieldInfo("CorpusCats", 0x00000008) },
		                { 0x002, new CMwFieldInfo("FogByVertex", 0x00000001) },
		                { 0x003, new CMwFieldInfo("FogRGB", 0x00000009) },
		                { 0x004, new CMwEnumInfo("FogFormula", new string[] { "None", "Exp", "Exp2", "Linear" }) },
		                { 0x005, new CMwEnumInfo("FogSpace", new string[] { "CameraFarZ", "World" }) },
		                { 0x006, new CMwFieldInfo("FogLinearStart", 0x00000024) },
		                { 0x007, new CMwFieldInfo("FogLinearEnd", 0x00000024) },
		                { 0x008, new CMwFieldInfo("FogExpDensity", 0x00000024) },
		                { 0x009, new CMwFieldInfo("Fog", 0x00000005) },
		                { 0x00A, new CMwFieldInfo("FogPlanes", 0x00000007) },
		                { 0x00B, new CMwFieldInfo("PrecalcRenders", 0x00000007) },
		                { 0x00C, new CMwFieldInfo("Sounds", 0x00000007) },
		                { 0x00D, new CMwFieldInfo("CorpusLights", 0x00000008) },
		                { 0x00E, new CMwFieldInfo("MRIsForced", 0x00000001) },
		                { 0x00F, new CMwFieldInfo("MRMaskWater", 0x00000001) },
		                { 0x010, new CMwFieldInfo("MRTileH", 0x00000024) },
		                { 0x011, new CMwFieldInfo("MRPoint", 0x00000035) },
		                { 0x012, new CMwFieldInfo("MRNormal", 0x00000035) },
		                { 0x013, new CMwFieldInfo("IVIdMaskReflected", 0x00000001) },
		                { 0x014, new CMwFieldInfo("IVIdMaskReflectMirror", 0x00000001) },
		                { 0x015, new CMwFieldInfo("IVIdMaskRefracted", 0x00000001) },
		                { 0x016, new CMwFieldInfo("IVIdMaskViewDepBump", 0x00000001) },
		                { 0x017, new CMwFieldInfo("IVIdMaskViewDepOcclusion", 0x00000001) },
		                { 0x018, new CMwFieldInfo("IVIdMaskOnlyRefracted", 0x00000001) },
		                { 0x019, new CMwFieldInfo("IVIdMaskHideWhenUnderground", 0x00000001) },
		                { 0x01A, new CMwFieldInfo("IVIdMaskHideWhenOverground", 0x00000001) },
		                { 0x01B, new CMwFieldInfo("IVIdMaskHideAlways", 0x00000001) },
		                { 0x01C, new CMwFieldInfo("IVIdMaskViewDepWindIntens", 0x00000001) },
		                { 0x01D, new CMwFieldInfo("IVIdMaskBackground", 0x00000001) },
		                { 0x01E, new CMwFieldInfo("IVIdMaskGrassRGB", 0x00000001) },
		                { 0x01F, new CMwFieldInfo("IVIdMaskLightGenP", 0x00000001) },
		                { 0x020, new CMwFieldInfo("IVIdMaskVehicle", 0x00000001) },
		                { 0x021, new CMwFieldInfo("IVIdMaskHideOnlyDirect", 0x00000001) },
		                { 0x022, new CMwFieldInfo("IVIdRefReflected", 0x00000001) },
		                { 0x023, new CMwFieldInfo("IVIdRefReflectMirror", 0x00000001) },
		                { 0x024, new CMwFieldInfo("IVIdRefRefracted", 0x00000001) },
		                { 0x025, new CMwFieldInfo("IVIdRefViewDepBump", 0x00000001) },
		                { 0x026, new CMwFieldInfo("IVIdRefViewDepOcclusion", 0x00000001) },
		                { 0x027, new CMwFieldInfo("IVIdRefOnlyRefracted", 0x00000001) },
		                { 0x028, new CMwFieldInfo("IVIdRefHideWhenUnderground", 0x00000001) },
		                { 0x029, new CMwFieldInfo("IVIdRefHideWhenOverground", 0x00000001) },
		                { 0x02A, new CMwFieldInfo("IVIdRefHideAlways", 0x00000001) },
		                { 0x02B, new CMwFieldInfo("IVIdRefViewDepWindIntens", 0x00000001) },
		                { 0x02C, new CMwFieldInfo("IVIdRefBackground", 0x00000001) },
		                { 0x02D, new CMwFieldInfo("IVIdRefGrassRGB", 0x00000001) },
		                { 0x02E, new CMwFieldInfo("IVIdRefLightGenP", 0x00000001) },
		                { 0x02F, new CMwFieldInfo("IVIdRefVehicle", 0x00000001) },
		                { 0x030, new CMwFieldInfo("IVIdRefHideOnlyDirect", 0x00000001) },
		                { 0x031, new CMwFieldInfo("SVIdMaskReflected", 0x00000001) },
		                { 0x032, new CMwFieldInfo("SVIdMaskReflectMirror", 0x00000001) },
		                { 0x033, new CMwFieldInfo("SVIdMaskRefracted", 0x00000001) },
		                { 0x034, new CMwFieldInfo("SVIdMaskViewDepBump", 0x00000001) },
		                { 0x035, new CMwFieldInfo("SVIdMaskViewDepOcclusion", 0x00000001) },
		                { 0x036, new CMwFieldInfo("SVIdMaskOnlyRefracted", 0x00000001) },
		                { 0x037, new CMwFieldInfo("SVIdMaskHideWhenUnderground", 0x00000001) },
		                { 0x038, new CMwFieldInfo("SVIdMaskHideWhenOverground", 0x00000001) },
		                { 0x039, new CMwFieldInfo("SVIdMaskHideAlways", 0x00000001) },
		                { 0x03A, new CMwFieldInfo("SVIdMaskViewDepWindIntens", 0x00000001) },
		                { 0x03B, new CMwFieldInfo("SVIdMaskBackground", 0x00000001) },
		                { 0x03C, new CMwFieldInfo("SVIdMaskGrassRGB", 0x00000001) },
		                { 0x03D, new CMwFieldInfo("SVIdMaskLightGenP", 0x00000001) },
		                { 0x03E, new CMwFieldInfo("SVIdMaskVehicle", 0x00000001) },
		                { 0x03F, new CMwFieldInfo("SVIdMaskHideOnlyDirect", 0x00000001) },
		                { 0x040, new CMwFieldInfo("SVIdRefReflected", 0x00000001) },
		                { 0x041, new CMwFieldInfo("SVIdRefReflectMirror", 0x00000001) },
		                { 0x042, new CMwFieldInfo("SVIdRefRefracted", 0x00000001) },
		                { 0x043, new CMwFieldInfo("SVIdRefViewDepBump", 0x00000001) },
		                { 0x044, new CMwFieldInfo("SVIdRefViewDepOcclusion", 0x00000001) },
		                { 0x045, new CMwFieldInfo("SVIdRefOnlyRefracted", 0x00000001) },
		                { 0x046, new CMwFieldInfo("SVIdRefHideWhenUnderground", 0x00000001) },
		                { 0x047, new CMwFieldInfo("SVIdRefHideWhenOverground", 0x00000001) },
		                { 0x048, new CMwFieldInfo("SVIdRefHideAlways", 0x00000001) },
		                { 0x049, new CMwFieldInfo("SVIdRefViewDepWindIntens", 0x00000001) },
		                { 0x04A, new CMwFieldInfo("SVIdRefBackground", 0x00000001) },
		                { 0x04B, new CMwFieldInfo("SVIdRefGrassRGB", 0x00000001) },
		                { 0x04C, new CMwFieldInfo("SVIdRefLightGenP", 0x00000001) },
		                { 0x04D, new CMwFieldInfo("SVIdRefVehicle", 0x00000001) },
		                { 0x04E, new CMwFieldInfo("SVIdRefHideOnlyDirect", 0x00000001) },
		                { 0x04F, new CMwMethodInfo("VPackerCreate", 0x00000000, null, null) },
		                { 0x050, new CMwFieldInfo("VPackerPowerX", 0x0000001F) },
		                { 0x051, new CMwFieldInfo("VPackerPowerY", 0x0000001F) },
		                { 0x052, new CMwFieldInfo("VPackerPowerZ", 0x0000001F) },
		                { 0x053, new CMwFieldInfo("VPackerPercentCellUsed", 0x00000024) },
		                { 0x054, new CMwFieldInfo("VPackerAverageObjectPerCell", 0x00000024) },
		                { 0x055, new CMwFieldInfo("VPackerAveragePackPerCell", 0x00000024) },
		                { 0x056, new CMwFieldInfo("VPackerAverageKVertPerPack", 0x00000024) },
		                { 0x057, new CMwFieldInfo("VPackerPercentObjectOutside", 0x00000024) },
		                { 0x058, new CMwMethodInfo("VPackerForceBBoxUpdate", 0x00000000, null, null) },
		                { 0x059, new CMwFieldInfo("BitmapCubeReflectHardSpecA", 0x00000005) }
	                  })
	                },
	                { 0x005, new CMwClassInfo("CHmsZoneDynamic", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("IsPhysics", 0x00000001) },
		                { 0x001, new CMwEnumInfo("DynaStateType", new string[] { "BlendTime", "BlendConstant", "BeforeCollision" }) },
		                { 0x002, new CMwFieldInfo("DynaBlendVal", 0x00000028) },
		                { 0x003, new CMwEnumInfo("ReplayStateType", new string[] { "BlendTime", "BlendConstant" }) },
		                { 0x004, new CMwFieldInfo("ReplayBlendVal", 0x00000028) },
		                { 0x005, new CMwEnumInfo("Extrapolation", new string[] { "None", "Basic", "Continuous1", "Continuous2" }) },
		                { 0x006, new CMwFieldInfo("ExtrapolationMaxDuration", 0x0000001F) },
		                { 0x007, new CMwFieldInfo("MaxKeepingContinuityDuration", 0x0000001F) },
		                { 0x008, new CMwFieldInfo("ExtrapolationContinuityDuration", 0x0000001F) },
		                { 0x009, new CMwFieldInfo("ExtrapolationContinuity2TargetDuration", 0x0000001F) },
		                { 0x00A, new CMwFieldInfo("ExtrapolationContinuity2MaxDist", 0x0000001F) },
		                { 0x00B, new CMwFieldInfo("IsTweakedSpeeds", 0x00000001) },
		                { 0x00C, new CMwFieldInfo("TweakedSpeedsCoef", 0x00000028) },
		                { 0x00D, new CMwFieldInfo("LinearFluidFrictionCoef", 0x00000028) },
		                { 0x00E, new CMwFieldInfo("AngularFluidFrictionCoef", 0x00000028) },
		                { 0x00F, new CMwFieldInfo("DynamicCorpuss", 0x00000007) },
		                { 0x010, new CMwFieldInfo("ForceFields", 0x00000007) },
		                { 0x011, new CMwFieldInfo("RefluxRatio", 0x00000028) },
		                { 0x012, new CMwFieldInfo("DpThreshold", 0x00000028) },
		                { 0x013, new CMwFieldInfo("Memory", 0x00000028) },
		                { 0x014, new CMwFieldInfo("EpsilonMax", 0x00000028) },
		                { 0x015, new CMwFieldInfo("EpsilonMin", 0x00000028) },
		                { 0x016, new CMwFieldInfo("EpsilonRepl", 0x00000024) },
		                { 0x017, new CMwFieldInfo("DesactivationSpeed", 0x00000024) }
	                  })
	                },
	                { 0x006, new CMwClassInfo("CHmsPortal", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("IsActive", 0x00000001) },
		                { 0x001, new CMwFieldInfo("NeedClipping2D", 0x00000001) },
		                { 0x002, new CMwFieldInfo("NeedClipping3D", 0x00000001) },
		                { 0x003, new CMwFieldInfo("CanSeeThrough", 0x00000001) },
		                { 0x004, new CMwFieldInfo("SeeThroughOpacity", 0x00000028) },
		                { 0x005, new CMwFieldInfo("IsVisualVisible", 0x00000001) },
		                { 0x006, new CMwFieldInfo("IsPickingPossible", 0x00000001) },
		                { 0x007, new CMwFieldInfo("CanPassThrough", 0x00000001) },
		                { 0x008, new CMwFieldInfo("SoundCanPassThrough", 0x00000001) },
		                { 0x009, new CMwFieldInfo("IsDirectPathSet", 0x00000001) },
		                { 0x00A, new CMwFieldInfo("DirectOcclusion", 0x0000000E) },
		                { 0x00B, new CMwFieldInfo("DirectOcclusionSpectralRatio", 0x00000024) },
		                { 0x00C, new CMwFieldInfo("DirectOcclusionRatio", 0x00000024) },
		                { 0x00D, new CMwFieldInfo("IndirectOcclusion", 0x0000000E) },
		                { 0x00E, new CMwFieldInfo("IndirectOcclusionSpectralRatio", 0x00000024) },
		                { 0x00F, new CMwFieldInfo("IndirectOcclusionRatio", 0x00000024) },
		                { 0x010, new CMwFieldInfo("IndirectObstruction", 0x0000000E) },
		                { 0x011, new CMwFieldInfo("IndirectObstructionSpectralRatio", 0x00000024) }
	                  })
	                },
	                { 0x007, new CMwClassInfo("CHmsPoc", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("IsActive", 0x00000001) },
		                { 0x001, new CMwFieldInfo("Speed", 0x00000035) }
	                  })
	                },
	                { 0x008, new CMwClassInfo("CHmsZoneElem", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("Zone", 0x00000005) },
		                { 0x001, new CMwFieldInfo("Location", 0x00000013) }
	                  })
	                },
	                { 0x009, new CMwClassInfo("CHmsZoneOverlay", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwEnumInfo("PickEnableMode", new string[] { "Disabled", "Foreground", "Always" }) },
		                { 0x001, new CMwEnumInfo("AdaptRatio", new string[] { "None", "ReSizeX", "ReSizeY" }) },
		                { 0x002, new CMwFieldInfo("UseZBuffer", 0x00000001) },
		                { 0x003, new CMwFieldInfo("DrawRectMin", 0x00000031) },
		                { 0x004, new CMwFieldInfo("DrawRectMax", 0x00000031) },
		                { 0x005, new CMwFieldInfo("FrustumCenter", 0x00000035) },
		                { 0x006, new CMwFieldInfo("FurstrumHfDiag", 0x00000035) },
		                { 0x007, new CMwFieldInfo("DescIsClearColorEnable", 0x00000001) },
		                { 0x008, new CMwFieldInfo("DescIsClearDepthEnable", 0x00000001) },
		                { 0x009, new CMwFieldInfo("DescIsClearStencilEnable", 0x00000001) },
		                { 0x00A, new CMwFieldInfo("DescClearColor", 0x00000009) },
		                { 0x00B, new CMwFieldInfo("CorpusVisibles", 0x00000007) }
	                  })
	                },
	                { 0x00A, new CMwClassInfo("CHmsListener", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("AudioEnvironment", 0x00000005) }
	                  })
	                },
	                { 0x00B, new CMwClassInfo("CHmsPocEmitter", new Dictionary<int, CMwMemberInfo>()
	                  {
	                  })
	                },
	                { 0x00C, new CMwClassInfo("CHmsLight", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwEnumInfo("UpdateType", new string[] { "Dynamic", "Static" }) },
		                { 0x001, new CMwFieldInfo("MainGxLight", 0x00000005) },
		                { 0x002, new CMwFieldInfo("BitmapFlare", 0x00000005) },
		                { 0x003, new CMwFieldInfo("BitmapSprite", 0x00000005) },
		                { 0x004, new CMwFieldInfo("BitmapProjector", 0x00000005) },
		                { 0x005, new CMwFieldInfo("CorpusLights", 0x00000007) },
		                { 0x006, new CMwFieldInfo("ItemContainer", 0x00000005) },
		                { 0x007, new CMwFieldInfo("ForceShadowGroup", 0x00000001) },
		                { 0x008, new CMwFieldInfo("ForceShadowGroupIndex", 0x0000001F) },
		                { 0x009, new CMwFieldInfo("NeverSkipShadow", 0x00000001) }
	                  })
	                },
	                { 0x00D, new CMwClassInfo("CHmsSoundSource", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("PlugSound", 0x00000005) },
		                { 0x001, new CMwFieldInfo("PriorityAdjustement", 0x00000024) },
		                { 0x002, new CMwFieldInfo("UseLowQuality", 0x00000001) },
		                { 0x003, new CMwFieldInfo("VolumicSize", 0x00000035) },
		                { 0x004, new CMwFieldInfo("Play", 0x00000001) },
		                { 0x005, new CMwFieldInfo("Stop", 0x00000001) },
		                { 0x006, new CMwFieldInfo("Volume", 0x00000028) },
		                { 0x007, new CMwFieldInfo("Pitch", 0x00000028) },
		                { 0x008, new CMwFieldInfo("Variant", 0x0000001F) },
		                { 0x009, new CMwFieldInfo("RpmOrSpeed", 0x00000024) },
		                { 0x00A, new CMwFieldInfo("Accel", 0x00000028) },
		                { 0x00B, new CMwFieldInfo("Alpha", 0x00000028) },
		                { 0x00C, new CMwFieldInfo("SurfaceId", 0x0000001F) },
		                { 0x00D, new CMwFieldInfo("Impact", 0x0000001F) },
		                { 0x00E, new CMwFieldInfo("SkidIntensity", 0x00000024) },
		                { 0x00F, new CMwFieldInfo("AudioSound", 0x00000005) }
	                  })
	                },
	                { 0x00E, new CMwClassInfo("CHmsPortalProperty", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("Portal1", 0x00000005) },
		                { 0x001, new CMwFieldInfo("Portal2", 0x00000005) },
		                { 0x002, new CMwFieldInfo("Visibility", 0x00000001) },
		                { 0x003, new CMwFieldInfo("Audibility", 0x00000001) }
	                  })
	                },
	                { 0x00F, new CMwClassInfo("CHmsCorpusLight", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("Light", 0x00000005) }
	                  })
	                },
	                { 0x010, new CMwClassInfo("CHmsViewport", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwEnumInfo("DeviceKind", new string[] { "PC0", "PC1", "PC2", "PC3" }) },
		                { 0x001, new CMwFieldInfo("iSubDevice", 0x0000001F) },
		                { 0x002, new CMwEnumInfo("PC3_Quality", new string[] { "VeryLow", "Low", "Medium", "High", "VeryHigh" }) },
		                { 0x003, new CMwEnumInfo("BitmapQuality", new string[] { "PC0", "PC1", "PC2", "PC3" }) },
		                { 0x004, new CMwFieldInfo("IsDithering", 0x00000001) },
		                { 0x005, new CMwEnumInfo("MultiSampleType", new string[] {  }) },
		                { 0x006, new CMwEnumInfo("MaxFiltering", new string[] { "Point", "Bilinear", "Trilinear", "Anisotropic", "ForceAniso" }) },
		                { 0x007, new CMwFieldInfo("AnisotropicLevel", 0x0000001F) },
		                { 0x008, new CMwFieldInfo("MinMipMapLodBias", 0x00000024) },
		                { 0x009, new CMwEnumInfo("Lighting", new string[] { "Dynamic & Static", "Dynamic Only" }) },
		                { 0x00A, new CMwFieldInfo("ArePortalsActive", 0x00000001) },
		                { 0x00B, new CMwFieldInfo("PortalMaxRecur", 0x0000001F) },
		                { 0x00C, new CMwFieldInfo("IsPickingPossible", 0x00000001) },
		                { 0x00D, new CMwFieldInfo("TimeTickToPresent", 0x0000001F) },
		                { 0x00E, new CMwFieldInfo("TimeQueryIssueToFinish", 0x0000001F) },
		                { 0x00F, new CMwFieldInfo("TargetFpsEnable", 0x00000001) },
		                { 0x010, new CMwFieldInfo("TargetFps", 0x0000001F) },
		                { 0x011, new CMwFieldInfo("TargetFpsMinScaleZ", 0x00000028) },
		                { 0x012, new CMwFieldInfo("TargetFpsMaxScaleZ", 0x00000028) },
		                { 0x013, new CMwFieldInfo("TargetFpsSpeed", 0x00000028) },
		                { 0x014, new CMwFieldInfo("AverageFps", 0x00000024) },
		                { 0x015, new CMwFieldInfo("MipScaleZ", 0x00000024) },
		                { 0x016, new CMwEnumInfo("TextureRender", new string[] { "None", "CameraFx", "All" }) },
		                { 0x017, new CMwFieldInfo("DisableShadowBuffer", 0x00000001) },
		                { 0x018, new CMwFieldInfo("ShadowCastBack", 0x00000001) },
		                { 0x019, new CMwEnumInfo("RenderShadows", new string[] { "Disable", "Fake", "Mask", "Depth", "Pssm", "PssmAO" }) },
		                { 0x01A, new CMwEnumInfo("RenderProjectors", new string[] { "Disable", "RenderCube", "2dNoClipZ" }) },
		                { 0x01B, new CMwFieldInfo("PreLoadProjectors", 0x00000001) },
		                { 0x01C, new CMwFieldInfo("DeferedProjectors", 0x00000001) },
		                { 0x01D, new CMwFieldInfo("EnableLensFlares", 0x00000001) },
		                { 0x01E, new CMwFieldInfo("RenderZoneVPacker", 0x00000001) },
		                { 0x01F, new CMwEnumInfo("SelectTnL", new string[] { "Hardware", "Software VertexShader", "Software" }) },
		                { 0x020, new CMwFieldInfo("IsVshEmulEnable", 0x00000001) },
		                { 0x021, new CMwFieldInfo("IsBlendingSoft", 0x00000001) },
		                { 0x022, new CMwFieldInfo("EnableTessellation", 0x00000001) },
		                { 0x023, new CMwFieldInfo("IsPureDevice", 0x00000001) },
		                { 0x024, new CMwEnumInfo("FullScreenModeIndex", new string[] {  }) },
		                { 0x025, new CMwEnumInfo("FullScreenBitDepth", new string[] { "16b", "32b" }) },
		                { 0x026, new CMwEnumInfo("FullScreenRefreshRate", new string[] {  }) },
		                { 0x027, new CMwEnumInfo("FullScreenMultiSample", new string[] {  }) },
		                { 0x028, new CMwFieldInfo("FullScreenWantVSync", 0x00000001) },
		                { 0x029, new CMwFieldInfo("FullScreenBrightness", 0x00000028) },
		                { 0x02A, new CMwFieldInfo("FullScreenContrast", 0x00000028) },
		                { 0x02B, new CMwFieldInfo("FullScreenGamma", 0x00000028) },
		                { 0x02C, new CMwFieldInfo("ScreenShotFullName", 0x0000002D) },
		                { 0x02D, new CMwFieldInfo("ScreenShotForceRes", 0x00000001) },
		                { 0x02E, new CMwFieldInfo("ScreenShotWidth", 0x0000001F) },
		                { 0x02F, new CMwFieldInfo("ScreenShotHeight", 0x0000001F) },
		                { 0x030, new CMwMethodInfo("ScreenShotDoCaptureBMP", 0x00000000, null, null) },
		                { 0x031, new CMwMethodInfo("ScreenShotDoCaptureDDS", 0x00000000, null, null) },
		                { 0x032, new CMwFieldInfo("Underlays", 0x00000007) },
		                { 0x033, new CMwFieldInfo("Cameras", 0x00000007) },
		                { 0x034, new CMwFieldInfo("Overlays", 0x00000007) },
		                { 0x035, new CMwFieldInfo("SystemWindow", 0x00000005) },
		                { 0x036, new CMwFieldInfo("Picker", 0x00000005) },
		                { 0x037, new CMwFieldInfo("Config", 0x00000005) },
		                { 0x038, new CMwFieldInfo("AmbientOcc", 0x00000005) },
		                { 0x039, new CMwFieldInfo("Alpha01BlendEdges", 0x00000001) },
		                { 0x03A, new CMwFieldInfo("Alpha01ClipRef", 0x0000001F) }
	                  })
	                },
	                { 0x011, new CMwClassInfo("CHmsPrecalcRender", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("BitmapRGB", 0x00000005) },
		                { 0x001, new CMwFieldInfo("BitmapDepth", 0x00000005) },
		                { 0x002, new CMwEnumInfo("BitmapDepthMode", new string[] { "Linear", "ZBuffer" }) },
		                { 0x003, new CMwFieldInfo("TreeIdDepthGen", 0x00000029) },
		                { 0x004, new CMwFieldInfo("IsTreeDepthGenFound", 0x00000001) },
		                { 0x005, new CMwFieldInfo("ZoomFactor", 0x00000024) },
		                { 0x006, new CMwFieldInfo("ScrollPosX", 0x00000028) },
		                { 0x007, new CMwFieldInfo("ScrollPosY", 0x00000028) }
	                  })
	                },
	                { 0x012, new CMwClassInfo("CHmsShadowGroup", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("Width", 0x0000001F) },
		                { 0x001, new CMwFieldInfo("Height", 0x0000001F) },
		                { 0x002, new CMwFieldInfo("Enable", 0x00000001) },
		                { 0x003, new CMwFieldInfo("NeedSelfShadow", 0x00000001) },
		                { 0x004, new CMwFieldInfo("ForceShadowMask", 0x00000001) },
		                { 0x005, new CMwFieldInfo("DepthNeed32b", 0x00000001) },
		                { 0x006, new CMwFieldInfo("IsStatic", 0x00000001) },
		                { 0x007, new CMwFieldInfo("IsStaticDirty", 0x00000001) },
		                { 0x008, new CMwFieldInfo("ForceWorldAlign", 0x00000001) },
		                { 0x009, new CMwFieldInfo("VDepPlane_UseWaterYs", 0x00000001) },
		                { 0x00A, new CMwFieldInfo("UseClipPlaneY", 0x00000001) },
		                { 0x00B, new CMwFieldInfo("ClipPlaneY", 0x00000024) },
		                { 0x00C, new CMwEnumInfo("ViewDependantMode", new string[] { "None", "ClipBBoxByFrustumToLightXY", "Plane", "PSM" }) },
		                { 0x00D, new CMwFieldInfo("UseHqCasterBBox", 0x00000001) },
		                { 0x00E, new CMwFieldInfo("AllStaticItemAreCaster", 0x00000001) },
		                { 0x00F, new CMwFieldInfo("ShadowInShader", 0x00000001) },
		                { 0x010, new CMwFieldInfo("TmBackgroundReceives", 0x00000001) },
		                { 0x011, new CMwFieldInfo("TmBackgroundCast", 0x00000001) },
		                { 0x012, new CMwFieldInfo("DepthBiasConst", 0x00000024) },
		                { 0x013, new CMwFieldInfo("DepthBiasSlope", 0x00000024) },
		                { 0x014, new CMwFieldInfo("DepthBiasConstShaderExtra", 0x00000024) },
		                { 0x015, new CMwFieldInfo("VDepPlaneMinY", 0x00000024) },
		                { 0x016, new CMwFieldInfo("VDepPlanePrjY", 0x00000024) },
		                { 0x017, new CMwFieldInfo("VDepPlaneMaxY", 0x00000024) },
		                { 0x018, new CMwFieldInfo("ViewDepFarZ", 0x00000024) },
		                { 0x019, new CMwFieldInfo("ShadeSlope", 0x00000028) },
		                { 0x01A, new CMwFieldInfo("Soft2dSlope", 0x00000028) },
		                { 0x01B, new CMwFieldInfo("SoftSizeInW", 0x00000031) },
		                { 0x01C, new CMwFieldInfo("MaskBlurTexelCount", 0x0000001F) },
		                { 0x01D, new CMwFieldInfo("PssmTexSize", 0x0000001F) },
		                { 0x01E, new CMwFieldInfo("PssmTexCount", 0x0000001F) },
		                { 0x01F, new CMwFieldInfo("PssmTexCountActive", 0x0000001F) },
		                { 0x020, new CMwFieldInfo("PssmOverlapIn01", 0x00000028) },
		                { 0x021, new CMwFieldInfo("PssmDistNF0", 0x00000024) },
		                { 0x022, new CMwFieldInfo("PssmDistNF1", 0x00000024) },
		                { 0x023, new CMwFieldInfo("PssmDistNF2", 0x00000024) },
		                { 0x024, new CMwFieldInfo("PssmDistNF3", 0x00000024) },
		                { 0x025, new CMwFieldInfo("PssmDistNF4", 0x00000024) },
		                { 0x026, new CMwFieldInfo("PssmDistScale", 0x00000024) },
		                { 0x027, new CMwFieldInfo("Bitmap", 0x00000005) },
		                { 0x028, new CMwFieldInfo("FileGpuP", 0x00000005) }
	                  })
	                },
	                { 0x014, new CMwClassInfo("CHmsForceField", new Dictionary<int, CMwMemberInfo>()
	                  {
	                  })
	                },
	                { 0x015, new CMwClassInfo("CHmsForceFieldBall", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("InfluenceRadius", 0x00000024) },
		                { 0x001, new CMwFieldInfo("Intensity", 0x00000024) }
	                  })
	                },
	                { 0x016, new CMwClassInfo("CHmsForceFieldUniform", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("FieldValue", 0x00000035) }
	                  })
	                },
	                { 0x017, new CMwClassInfo("CHmsFogPlane", new Dictionary<int, CMwMemberInfo>()
	                  {
	                  })
	                },
	                { 0x018, new CMwClassInfo("CHmsPicker", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("IsEnable", 0x00000001) },
		                { 0x001, new CMwFieldInfo("InputPos", 0x00000031) },
		                { 0x002, new CMwFieldInfo("Camera", 0x00000005) },
		                { 0x003, new CMwFieldInfo("Overlay", 0x00000005) },
		                { 0x004, new CMwFieldInfo("PosRect", 0x00000031) },
		                { 0x005, new CMwFieldInfo("RayDir", 0x00000035) },
		                { 0x006, new CMwFieldInfo("RayPos", 0x00000035) },
		                { 0x007, new CMwFieldInfo("Corpus", 0x00000005) },
		                { 0x008, new CMwFieldInfo("Tree", 0x00000005) },
		                { 0x009, new CMwFieldInfo("PickPosV", 0x00000035) },
		                { 0x00A, new CMwFieldInfo("PickPosZ", 0x00000035) },
		                { 0x00B, new CMwFieldInfo("PickNormalV", 0x00000035) },
		                { 0x00C, new CMwFieldInfo("PickNormalZ", 0x00000035) },
		                { 0x00D, new CMwFieldInfo("PickZ", 0x00000024) },
		                { 0x00E, new CMwFieldInfo("PickZNoBiasZ", 0x00000024) },
		                { 0x00F, new CMwFieldInfo("Depth", 0x00000024) }
	                  })
	                },
	                { 0x019, new CMwClassInfo("CHmsCollisionManager", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("ZonesCount", 0x0000001F) },
		                { 0x001, new CMwMethodInfo("DisableStaticCollision", 0x00000000, null, null) },
		                { 0x002, new CMwEnumInfo("StaticCollisionGeneration", new string[] { "1LevelBoxes", "Octree", "BinTree" }) },
		                { 0x003, new CMwFieldInfo("SphereContactMergeThreshold", 0x00000024) }
	                  })
	                },
	                { 0x01D, new CMwClassInfo("CHmsConfig", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("FreezeViewportRun", 0x00000001) },
		                { 0x001, new CMwFieldInfo("ViewportRunFrameCount", 0x0000001F) },
		                { 0x002, new CMwFieldInfo("ViewportRunFrameRemaining", 0x0000001F) },
		                { 0x003, new CMwFieldInfo("ShadowGroups", 0x00000006) }
	                  })
	                },
	                { 0x020, new CMwClassInfo("CHmsItemShadow", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("Intensity", 0x00000028) },
		                { 0x001, new CMwFieldInfo("FallOffStart", 0x00000024) },
		                { 0x002, new CMwFieldInfo("FallOffEnd", 0x00000024) },
		                { 0x003, new CMwFieldInfo("GroupOverride", 0x00000005) }
	                  })
	                },
	                { 0x021, new CMwClassInfo("CHmsPackLightMap", new Dictionary<int, CMwMemberInfo>()
	                  {
	                  })
	                },
	                { 0x022, new CMwClassInfo("CHmsPackLightMapCache", new Dictionary<int, CMwMemberInfo>()
	                  {
	                  })
	                },
	                { 0x023, new CMwClassInfo("CHmsPackLightMapMood", new Dictionary<int, CMwMemberInfo>()
	                  {
	                  })
	                },
	                { 0x024, new CMwClassInfo("CHmsPackLightMapAlloc", new Dictionary<int, CMwMemberInfo>()
	                  {
	                  })
	                },
	                { 0x025, new CMwClassInfo("CHmsCorpus2d", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("ClipRectIndex", 0x0000001F) }
	                  })
	                },
	                { 0x026, new CMwClassInfo("CHmsAmbientOcc", new Dictionary<int, CMwMemberInfo>()
	                  {
	                  })
	                }
                  })
                },
                { 0x07, new CMwEngineInfo("Control", new Dictionary<int, CMwClassInfo>()
                  {
	                { 0x001, new CMwClassInfo("CControlBase", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwMethodInfo("Draw", 0x00000000, null, null) },
		                { 0x001, new CMwMethodInfo("Clean", 0x00000000, null, null) },
		                { 0x002, new CMwFieldInfo("Parent", 0x00000005) },
		                { 0x003, new CMwFieldInfo("Nod", 0x00000005) },
		                { 0x004, new CMwEnumInfo("AlignHorizontal", new string[] { "Left", "HCenter", "Right", "None" }) },
		                { 0x005, new CMwEnumInfo("AlignVertical", new string[] { "Top", "VCenter", "Bottom", "None", "VCenter2" }) },
		                { 0x006, new CMwFieldInfo("IsReadOnly", 0x00000001) },
		                { 0x007, new CMwFieldInfo("AddFocusArea", 0x00000001) },
		                { 0x008, new CMwFieldInfo("DrawBackground", 0x00000001) },
		                { 0x009, new CMwFieldInfo("HasSolid", 0x00000001) },
		                { 0x00A, new CMwFieldInfo("IsSubSolid", 0x00000001) },
		                { 0x00B, new CMwFieldInfo("IsDynamic", 0x00000001) },
		                { 0x00C, new CMwFieldInfo("IsFocused", 0x00000001) },
		                { 0x00D, new CMwFieldInfo("IsSelected", 0x00000001) },
		                { 0x00E, new CMwFieldInfo("IsHiddenExternal", 0x00000001) },
		                { 0x00F, new CMwFieldInfo("IsVisualFocusForced", 0x00000001) },
		                { 0x010, new CMwFieldInfo("StackText", 0x00000029) },
		                { 0x011, new CMwFieldInfo("ToolTip", 0x0000002D) },
		                { 0x012, new CMwFieldInfo("IsHiddenInternal", 0x00000001) },
		                { 0x013, new CMwFieldInfo("IsFocusCaptured", 0x00000001) },
		                { 0x014, new CMwFieldInfo("IsStackNeeded", 0x00000001) },
		                { 0x015, new CMwFieldInfo("IsCreatedByScript", 0x00000001) },
		                { 0x016, new CMwFieldInfo("Style", 0x00000005) },
		                { 0x017, new CMwFieldInfo("ControlDisplayTree", 0x00000005) },
		                { 0x018, new CMwFieldInfo("ControlDrawTree", 0x00000005) },
		                { 0x019, new CMwFieldInfo("BoxMin", 0x00000031) },
		                { 0x01A, new CMwFieldInfo("BoxMax", 0x00000031) },
		                { 0x01B, new CMwFieldInfo("Layout", 0x00000005) },
		                { 0x01C, new CMwFieldInfo("ClipLength", 0x00000024) },
		                { 0x01D, new CMwMethodInfo("OnAction", 0x00000000, null, null) }
	                  })
	                },
	                { 0x002, new CMwClassInfo("CControlContainer", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("Childs", 0x00000006) },
		                { 0x001, new CMwFieldInfo("CreateScript", 0x00000005) },
		                { 0x002, new CMwFieldInfo("UseScript", 0x00000001) },
		                { 0x003, new CMwFieldInfo("AcceptOwnControls", 0x00000001) },
		                { 0x004, new CMwMethodInfo("AddControl", 0x00000041,
			                new uint[] { 0x07001000, 0x00000029, 0x00000035, 0x00000029, 0x01001000, 0x00000029, 0x00000029, 0x07017000 },
			                new string[] { "Control", "Id", "Position", "Label", "Nod", "Stack", "Type", "Style" })
		                },
		                { 0x005, new CMwMethodInfo("AddInstance", 0x00000041,
			                new uint[] { 0x07001000, 0x07001000, 0x00000029, 0x00000035 },
			                new string[] { "Control", "Model", "Id", "Position" })
		                },
		                { 0x006, new CMwMethodInfo("AddLabel", 0x00000041,
			                new uint[] { 0x07006000, 0x00000029, 0x00000035, 0x00000029, 0x07017000 },
			                new string[] { "Control", "Id", "Position", "Label", "Style" })
		                },
		                { 0x007, new CMwMethodInfo("AddButtonScript", 0x00000041,
			                new uint[] { 0x07007000, 0x00000029, 0x00000035, 0x00000029, 0x01067000, 0x07017000 },
			                new string[] { "Control", "Id", "Position", "Label", "Script", "Style" })
		                },
		                { 0x008, new CMwMethodInfo("RemoveControl", 0x00000041,
			                new uint[] { 0x07001000 },
			                new string[] { "Control" })
		                }
	                  })
	                },
	                { 0x003, new CMwClassInfo("CControlEffectSwitchStyle", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("FocusedStyle", 0x00000005) }
	                  })
	                },
	                { 0x004, new CMwClassInfo("CControlUiElement", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("Resources", 0x00000005) }
	                  })
	                },
	                { 0x005, new CMwClassInfo("CControlEffect", new Dictionary<int, CMwMemberInfo>()
	                  {
	                  })
	                },
	                { 0x006, new CMwClassInfo("CControlLabel", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("Label", 0x0000002D) },
		                { 0x001, new CMwFieldInfo("Bitmap", 0x00000005) },
		                { 0x002, new CMwFieldInfo("DontDrawText", 0x00000001) },
		                { 0x003, new CMwFieldInfo("CurrentPage", 0x0000001F) },
		                { 0x004, new CMwFieldInfo("PageCount", 0x0000001F) }
	                  })
	                },
	                { 0x007, new CMwClassInfo("CControlButton", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("Label", 0x0000002D) },
		                { 0x001, new CMwFieldInfo("ActionSound", 0x00000005) },
		                { 0x002, new CMwFieldInfo("Icons", 0x00000005) },
		                { 0x003, new CMwEnumInfo("DisplayType", new string[] { "Text", "Icon", "Text&Icon" }) },
		                { 0x004, new CMwFieldInfo("ActionScript", 0x00000005) },
		                { 0x005, new CMwFieldInfo("SubIconIndexOff", 0x0000001F) },
		                { 0x006, new CMwFieldInfo("SubIconIndexOn", 0x0000001F) },
		                { 0x007, new CMwFieldInfo("IconId", 0x00000029) }
	                  })
	                },
	                { 0x009, new CMwClassInfo("CControlEntry", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("String", 0x0000002D) },
		                { 0x001, new CMwFieldInfo("MaxLength", 0x0000001F) },
		                { 0x002, new CMwEnumInfo("Type", new string[] { "Unknown", "Natural", "Integer", "Real", "String", "TimeMmSsCc", "TimeHhMmSs", "RealFormated", "TimeMmSs", "Ascii7bit", "Real3decimals", "TimeHhMmSs_24", "TimeHhMm", " ", "Hexa", "TimeHhMmSsOrMmSs" }) },
		                { 0x003, new CMwFieldInfo("IsPassword", 0x00000001) },
		                { 0x004, new CMwFieldInfo("ValidateOnLostFocus", 0x00000001) },
		                { 0x005, new CMwFieldInfo("ClearOnFocusGained", 0x00000001) }
	                  })
	                },
	                { 0x00A, new CMwClassInfo("CControlEnum", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("FuncEnum", 0x00000005) },
		                { 0x001, new CMwFieldInfo("IsLooping", 0x00000001) },
		                { 0x002, new CMwEnumInfo("DisplayType", new string[] { "Text", "Icon", "Text&Icon" }) },
		                { 0x003, new CMwMethodInfo("Incr", 0x00000000, null, null) },
		                { 0x004, new CMwMethodInfo("Decr", 0x00000000, null, null) },
		                { 0x005, new CMwFieldInfo("EnumIndex", 0x0000001F) },
		                { 0x006, new CMwFieldInfo("EnumString", 0x00000029) }
	                  })
	                },
	                { 0x00B, new CMwClassInfo("CControlSlider", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwEnumInfo("Axis", new string[] { "X", "Y" }) },
		                { 0x001, new CMwFieldInfo("Ratio", 0x00000024) },
		                { 0x002, new CMwFieldInfo("IconIdBar", 0x0000001B) },
		                { 0x003, new CMwFieldInfo("IconIdCursor", 0x0000001B) },
		                { 0x004, new CMwFieldInfo("ManualSize", 0x00000001) }
	                  })
	                },
	                { 0x00C, new CMwClassInfo("CControlLayout", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwEnumInfo("AlignHorizontal", new string[] { "Left", "HCenter", "Right", "None" }) },
		                { 0x001, new CMwEnumInfo("AlignVertical", new string[] { "Top", "VCenter", "Bottom", "None", "VCenter2" }) },
		                { 0x002, new CMwFieldInfo("RatioHorizontal", 0x00000024) },
		                { 0x003, new CMwFieldInfo("RatioVertical", 0x00000024) },
		                { 0x004, new CMwFieldInfo("PaddingHorizontal", 0x00000024) },
		                { 0x005, new CMwFieldInfo("PaddingVertical", 0x00000024) }
	                  })
	                },
	                { 0x00D, new CMwClassInfo("CControlListItem", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("Str1", 0x00000029) },
		                { 0x001, new CMwFieldInfo("Str2", 0x00000029) },
		                { 0x002, new CMwFieldInfo("Str3", 0x00000029) },
		                { 0x003, new CMwFieldInfo("Str4", 0x00000029) },
		                { 0x004, new CMwFieldInfo("Str5", 0x00000029) },
		                { 0x005, new CMwFieldInfo("Str6", 0x00000029) },
		                { 0x006, new CMwFieldInfo("Str7", 0x00000029) },
		                { 0x007, new CMwFieldInfo("StrInt1", 0x0000002D) },
		                { 0x008, new CMwFieldInfo("StrInt2", 0x0000002D) },
		                { 0x009, new CMwFieldInfo("StrInt3", 0x0000002D) },
		                { 0x00A, new CMwFieldInfo("StrInt4", 0x0000002D) },
		                { 0x00B, new CMwFieldInfo("Nat1", 0x0000001F) },
		                { 0x00C, new CMwFieldInfo("Nat2", 0x0000001F) },
		                { 0x00D, new CMwFieldInfo("Nat3", 0x0000001F) },
		                { 0x00E, new CMwFieldInfo("Nat4", 0x0000001F) },
		                { 0x00F, new CMwFieldInfo("Time1", 0x0000001F) },
		                { 0x010, new CMwFieldInfo("Time2", 0x0000001F) },
		                { 0x011, new CMwFieldInfo("Time3", 0x0000001F) },
		                { 0x012, new CMwFieldInfo("Real1", 0x00000024) },
		                { 0x013, new CMwFieldInfo("Real2", 0x00000024) },
		                { 0x014, new CMwFieldInfo("Real3", 0x00000024) },
		                { 0x015, new CMwFieldInfo("MapCoordOrigin", 0x00000031) },
		                { 0x016, new CMwFieldInfo("MapCoordTarget", 0x00000031) },
		                { 0x017, new CMwFieldInfo("Nod1", 0x00000005) },
		                { 0x018, new CMwFieldInfo("Nod2", 0x00000005) },
		                { 0x019, new CMwFieldInfo("Nod3", 0x00000005) },
		                { 0x01A, new CMwFieldInfo("IsSelected", 0x00000001) }
	                  })
	                },
	                { 0x00E, new CMwClassInfo("CControlUiDockable", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwMethodInfo("Close", 0x00000000, null, null) },
		                { 0x001, new CMwMethodInfo("Open", 0x00000000, null, null) },
		                { 0x002, new CMwMethodInfo("Switch", 0x00000000, null, null) },
		                { 0x003, new CMwFieldInfo("IsClosed", 0x00000001) },
		                { 0x004, new CMwFieldInfo("IsOpened", 0x00000001) },
		                { 0x005, new CMwFieldInfo("Screens", 0x00000007) },
		                { 0x006, new CMwMethodInfo("NextScreen", 0x00000000, null, null) },
		                { 0x007, new CMwMethodInfo("PrevScreen", 0x00000000, null, null) },
		                { 0x008, new CMwFieldInfo("CurScreen", 0x0000001F) },
		                { 0x009, new CMwMethodInfo("CreateRotate", 0x00000041,
			                new uint[] { 0x00000035, 0x00000024, 0x00000024 },
			                new string[] { "Axis", "MinAngle", "MaxAngle" })
		                },
		                { 0x00A, new CMwMethodInfo("CreateTranslate", 0x00000041,
			                new uint[] { 0x00000035, 0x00000035 },
			                new string[] { "StartPoint", "EndPoint" })
		                },
		                { 0x00B, new CMwMethodInfo("AddScreen", 0x00000041,
			                new uint[] { 0x07012000, 0x00000029 },
			                new string[] { "Screen", "Id" })
		                }
	                  })
	                },
	                { 0x00F, new CMwClassInfo("CControlList", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("NbColumns", 0x0000001F) },
		                { 0x001, new CMwFieldInfo("NbLinesPerPage", 0x0000001F) },
		                { 0x002, new CMwFieldInfo("NbColumnsPerPage", 0x0000001F) },
		                { 0x003, new CMwFieldInfo("Transpose", 0x00000001) },
		                { 0x004, new CMwFieldInfo("FastPageStep", 0x0000001F) },
		                { 0x005, new CMwFieldInfo("ColumnStacks", 0x0000002A) },
		                { 0x006, new CMwMethodInfo("ClearList", 0x00000000, null, null) },
		                { 0x007, new CMwFieldInfo("LinesHeight", 0x00000024) },
		                { 0x008, new CMwFieldInfo("HorizontalSpacing", 0x00000024) },
		                { 0x009, new CMwFieldInfo("MarginLeft", 0x00000024) },
		                { 0x00A, new CMwFieldInfo("MarginRight", 0x00000024) },
		                { 0x00B, new CMwFieldInfo("AutoHideNavigationButtons", 0x00000001) },
		                { 0x00C, new CMwFieldInfo("ColumnsWidth", 0x00000025) },
		                { 0x00D, new CMwFieldInfo("ColumnStyles", 0x00000006) },
		                { 0x00E, new CMwFieldInfo("ColumnReadOnly", 0x00000002) },
		                { 0x00F, new CMwMethodInfo("NextPage", 0x00000000, null, null) },
		                { 0x010, new CMwMethodInfo("PrevPage", 0x00000000, null, null) },
		                { 0x011, new CMwMethodInfo("NextPageFast", 0x00000000, null, null) },
		                { 0x012, new CMwMethodInfo("PrevPageFast", 0x00000000, null, null) },
		                { 0x013, new CMwFieldInfo("CurLine", 0x0000001F) },
		                { 0x014, new CMwFieldInfo("CurPage", 0x0000001F) },
		                { 0x015, new CMwFieldInfo("NbLines", 0x0000001F) },
		                { 0x016, new CMwFieldInfo("NbPages", 0x0000001F) },
		                { 0x017, new CMwFieldInfo("ChildIndexes", 0x00000020) },
		                { 0x018, new CMwFieldInfo("LineSelectionZOffset", 0x00000024) },
		                { 0x019, new CMwFieldInfo("StyleLineSelected", 0x00000005) }
	                  })
	                },
	                { 0x010, new CMwClassInfo("CControlEffectSimi", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("IsInterpolated", 0x00000001) },
		                { 0x001, new CMwFieldInfo("Centered", 0x00000001) },
		                { 0x002, new CMwEnumInfo("ColorBlendMode", new string[] { "Set", "Mult" }) },
		                { 0x003, new CMwFieldInfo("IsContinousEffect", 0x00000001) },
		                { 0x004, new CMwFieldInfo("KeyCount", 0x0000001F) },
		                { 0x005, new CMwFieldInfo("CurrentKey", 0x0000001F) },
		                { 0x006, new CMwMethodInfo("InsertKey", 0x00000000, null, null) },
		                { 0x007, new CMwMethodInfo("RemoveKey", 0x00000000, null, null) },
		                { 0x008, new CMwFieldInfo("Time", 0x00000024) },
		                { 0x009, new CMwFieldInfo("Rot", 0x00000024) },
		                { 0x00A, new CMwFieldInfo("Pos", 0x00000031) },
		                { 0x00B, new CMwFieldInfo("Scale", 0x00000031) },
		                { 0x00C, new CMwFieldInfo("Depth", 0x00000024) },
		                { 0x00D, new CMwFieldInfo("Opacity", 0x00000024) },
		                { 0x00E, new CMwFieldInfo("ColorBlend", 0x00000024) },
		                { 0x00F, new CMwFieldInfo("Color", 0x00000009) }
	                  })
	                },
	                { 0x011, new CMwClassInfo("CControlEffectMotion", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("ParticleEmitterModel", 0x00000005) },
		                { 0x001, new CMwFieldInfo("ParticleEmitterId", 0x00000029) },
		                { 0x002, new CMwFieldInfo("Period", 0x00000024) },
		                { 0x003, new CMwFieldInfo("Envelope", 0x00000005) },
		                { 0x004, new CMwFieldInfo("EnveloppePosStart", 0x00000024) },
		                { 0x005, new CMwFieldInfo("EnveloppePosCharged", 0x00000024) },
		                { 0x006, new CMwFieldInfo("IntensityStart", 0x00000024) },
		                { 0x007, new CMwFieldInfo("IntensityCharged", 0x00000024) },
		                { 0x008, new CMwFieldInfo("SpeedStart", 0x00000024) },
		                { 0x009, new CMwFieldInfo("SpeedCharged", 0x00000024) },
		                { 0x00A, new CMwFieldInfo("SpeedDirection", 0x00000035) },
		                { 0x00B, new CMwFieldInfo("ChargeTime", 0x00000024) }
	                  })
	                },
	                { 0x012, new CMwClassInfo("CControlForm", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwMethodInfo("ClearCache", 0x00000000, null, null) }
	                  })
	                },
	                { 0x013, new CMwClassInfo("CControlOverlay", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("Scene", 0x00000005) },
		                { 0x001, new CMwFieldInfo("Camera", 0x00000005) }
	                  })
	                },
	                { 0x014, new CMwClassInfo("CControlUiRange", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("Ratio", 0x00000028) },
		                { 0x001, new CMwMethodInfo("CreateTranslate", 0x00000041,
			                new uint[] { 0x00000035, 0x00000035 },
			                new string[] { "StartPoint", "EndPoint" })
		                },
		                { 0x002, new CMwMethodInfo("CreateRotate", 0x00000041,
			                new uint[] { 0x00000035, 0x00000024, 0x00000024 },
			                new string[] { "Axis", "MinAngle", "MaxAngle" })
		                },
		                { 0x003, new CMwFieldInfo("IconId", 0x0000001B) }
	                  })
	                },
	                { 0x015, new CMwClassInfo("CControlGrid", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("ChildsSquares", 0x00000032) },
		                { 0x001, new CMwFieldInfo("ChildsSquaresParam", 0x0000002A) },
		                { 0x002, new CMwFieldInfo("MainLayout", 0x00000005) },
		                { 0x003, new CMwFieldInfo("PackEmptyRows", 0x00000001) },
		                { 0x004, new CMwFieldInfo("ResizeTextWidth", 0x00000001) },
		                { 0x005, new CMwFieldInfo("ResizeTextHeight", 0x00000001) },
		                { 0x006, new CMwFieldInfo("ForceColumnsUniformWidth", 0x00000024) },
		                { 0x007, new CMwFieldInfo("ForceRowsUniformHeight", 0x00000024) },
		                { 0x008, new CMwFieldInfo("ForceColumnsWidths", 0x00000026) },
		                { 0x009, new CMwFieldInfo("HorizontalSkewOffset", 0x00000024) },
		                { 0x00A, new CMwFieldInfo("VerticalSkewOffset", 0x00000024) }
	                  })
	                },
	                { 0x016, new CMwClassInfo("CControlFrame", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("ChildsRelativeLocations", 0x00000014) }
	                  })
	                },
	                { 0x017, new CMwClassInfo("CControlStyle", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("Font", 0x00000005) },
		                { 0x001, new CMwFieldInfo("FontHeight", 0x00000024) },
		                { 0x002, new CMwFieldInfo("FontRatioXY", 0x00000024) },
		                { 0x003, new CMwEnumInfo("LabelColorFromPalette", new string[] { "Color 0", "Color 1", "Color 2", "Color 3", "Color 4", "Color 5", "Color 6", "Color 7", "Custom" }) },
		                { 0x004, new CMwFieldInfo("LabelColor", 0x00000009) },
		                { 0x005, new CMwFieldInfo("LabelColorAlpha", 0x00000028) },
		                { 0x006, new CMwFieldInfo("LabelCharAttributes", 0x0000002D) },
		                { 0x007, new CMwEnumInfo("EditableColorFromPalette", new string[] { "Color 0", "Color 1", "Color 2", "Color 3", "Color 4", "Color 5", "Color 6", "Color 7", "Custom" }) },
		                { 0x008, new CMwFieldInfo("EditableColor", 0x00000009) },
		                { 0x009, new CMwFieldInfo("EditableColorAlpha", 0x00000028) },
		                { 0x00A, new CMwFieldInfo("EditableCharAttributes", 0x0000002D) },
		                { 0x00B, new CMwEnumInfo("GrayedColorFromPalette", new string[] { "Color 0", "Color 1", "Color 2", "Color 3", "Color 4", "Color 5", "Color 6", "Color 7", "Custom" }) },
		                { 0x00C, new CMwFieldInfo("GrayedColor", 0x00000009) },
		                { 0x00D, new CMwFieldInfo("GrayedColorAlpha", 0x00000028) },
		                { 0x00E, new CMwFieldInfo("GrayedCharAttributes", 0x0000002D) },
		                { 0x00F, new CMwFieldInfo("FitTextSize", 0x00000001) },
		                { 0x010, new CMwFieldInfo("Skew", 0x00000028) },
		                { 0x011, new CMwFieldInfo("DefaultShader", 0x00000005) },
		                { 0x012, new CMwFieldInfo("EntrySound", 0x00000005) },
		                { 0x013, new CMwFieldInfo("ActionSound", 0x00000005) },
		                { 0x014, new CMwFieldInfo("ButtonDefaultIcons", 0x00000005) },
		                { 0x015, new CMwFieldInfo("ButtonDefaultIconId", 0x0000001B) },
		                { 0x016, new CMwFieldInfo("ButtonIconWidth", 0x00000024) },
		                { 0x017, new CMwFieldInfo("ButtonIconHeight", 0x00000024) },
		                { 0x018, new CMwFieldInfo("EnumSound", 0x00000005) },
		                { 0x019, new CMwFieldInfo("EnumListShader", 0x00000005) },
		                { 0x01A, new CMwFieldInfo("EnumMaxElemCount", 0x0000001F) },
		                { 0x01B, new CMwFieldInfo("EnumIconWidth", 0x00000024) },
		                { 0x01C, new CMwFieldInfo("EnumIconHeight", 0x00000024) },
		                { 0x01D, new CMwEnumInfo("EnumForceDisplayType", new string[] { "Text", "Icon", "Text&Icon" }) },
		                { 0x01E, new CMwFieldInfo("EnumForceIcons", 0x00000005) },
		                { 0x01F, new CMwFieldInfo("QuadZ", 0x00000024) },
		                { 0x020, new CMwFieldInfo("QuadIsLines", 0x00000001) },
		                { 0x021, new CMwFieldInfo("QuadIsFill", 0x00000001) },
		                { 0x022, new CMwFieldInfo("QuadZLines", 0x00000024) },
		                { 0x023, new CMwFieldInfo("QuadGradientColor0", 0x00000009) },
		                { 0x024, new CMwFieldInfo("QuadGradientColor0Alpha", 0x00000028) },
		                { 0x025, new CMwFieldInfo("QuadGradientColor1", 0x00000009) },
		                { 0x026, new CMwFieldInfo("QuadGradientColor1Alpha", 0x00000028) },
		                { 0x027, new CMwFieldInfo("LineGradientColor0", 0x00000009) },
		                { 0x028, new CMwFieldInfo("LineGradientColor0Alpha", 0x00000028) },
		                { 0x029, new CMwFieldInfo("LineGradientColor1", 0x00000009) },
		                { 0x02A, new CMwFieldInfo("LineGradientColor1Alpha", 0x00000028) },
		                { 0x02B, new CMwFieldInfo("QuadLinesColor", 0x00000009) },
		                { 0x02C, new CMwFieldInfo("QuadLinesColorAlpha", 0x00000028) },
		                { 0x02D, new CMwFieldInfo("Quad_UvTopLeft", 0x00000031) },
		                { 0x02E, new CMwFieldInfo("Quad_UvBottomRight", 0x00000031) },
		                { 0x02F, new CMwFieldInfo("SliderBarWidth", 0x00000024) },
		                { 0x030, new CMwFieldInfo("SliderBarHeight", 0x00000024) },
		                { 0x031, new CMwFieldInfo("SliderCursorWidth", 0x00000024) },
		                { 0x032, new CMwFieldInfo("SliderCursorHeight", 0x00000024) },
		                { 0x033, new CMwFieldInfo("SliderBarIcons", 0x00000005) },
		                { 0x034, new CMwFieldInfo("SliderCursorIcons", 0x00000005) },
		                { 0x035, new CMwFieldInfo("SliderSound", 0x00000005) },
		                { 0x036, new CMwFieldInfo("FocusGainedScript", 0x00000005) },
		                { 0x037, new CMwFieldInfo("FocusLostScript", 0x00000005) },
		                { 0x038, new CMwFieldInfo("FocusSound", 0x00000005) },
		                { 0x039, new CMwFieldInfo("FocusAreaEnable", 0x00000001) },
		                { 0x03A, new CMwFieldInfo("FocusAreaMaterial", 0x00000005) },
		                { 0x03B, new CMwFieldInfo("FocusAreaMaterialReadOnly", 0x00000005) },
		                { 0x03C, new CMwFieldInfo("FocusAreaMaterialSelected", 0x00000005) },
		                { 0x03D, new CMwFieldInfo("FocusAreaMaterialFocused", 0x00000005) },
		                { 0x03E, new CMwFieldInfo("FocusAreaMinWidth", 0x00000024) },
		                { 0x03F, new CMwFieldInfo("FocusAreaMinHeight", 0x00000024) },
		                { 0x040, new CMwFieldInfo("FocusAreaXMargin", 0x00000024) },
		                { 0x041, new CMwFieldInfo("FocusAreaYMargin", 0x00000024) },
		                { 0x042, new CMwFieldInfo("FocusAreaZOffset", 0x00000024) },
		                { 0x043, new CMwFieldInfo("FocusAreaSolid", 0x00000005) },
		                { 0x044, new CMwFieldInfo("EffectMaster", 0x00000005) }
	                  })
	                },
	                { 0x018, new CMwClassInfo("CControlField2", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwEnumInfo("ControlMode", new string[] { "View", "Edit" }) },
		                { 0x001, new CMwEnumInfo("DisplayMode", new string[] { "RotationAndScale", "Scale" }) },
		                { 0x002, new CMwEnumInfo("RenderMode", new string[] { "Triangles", "Lines" }) },
		                { 0x003, new CMwFieldInfo("ArrowRatio", 0x00000028) },
		                { 0x004, new CMwFieldInfo("DisplaySize", 0x00000024) },
		                { 0x005, new CMwEnumInfo("IntensityPaintMode", new string[] { "None", "Static", "Dynamic" }) },
		                { 0x006, new CMwEnumInfo("DirectionPaintMode", new string[] { "None", "Static", "Dynamic" }) },
		                { 0x007, new CMwFieldInfo("BrushSize", 0x00000028) },
		                { 0x008, new CMwFieldInfo("BrushIntensity", 0x00000028) },
		                { 0x009, new CMwFieldInfo("BrushDirection", 0x00000028) },
		                { 0x00A, new CMwFieldInfo("DeltaDebug", 0x00000031) },
		                { 0x00B, new CMwFieldInfo("RotationModifSpeed", 0x00000024) },
		                { 0x00C, new CMwFieldInfo("ScaleModifSpeed", 0x00000024) },
		                { 0x00D, new CMwFieldInfo("IsInterpolate", 0x00000001) },
		                { 0x00E, new CMwFieldInfo("IsIntensityInArrowSize", 0x00000001) },
		                { 0x00F, new CMwFieldInfo("ArrowColor", 0x00000009) },
		                { 0x010, new CMwFieldInfo("ArrowColorMin", 0x00000009) },
		                { 0x011, new CMwFieldInfo("ArrowColorMax", 0x00000009) },
		                { 0x012, new CMwFieldInfo("IsDisplayFieldRect", 0x00000001) },
		                { 0x013, new CMwFieldInfo("IsDisplayDrawRect", 0x00000001) },
		                { 0x014, new CMwFieldInfo("DisplaySkipLevel", 0x0000001F) },
		                { 0x015, new CMwFieldInfo("IsAutoDisplaySkipLevel", 0x00000001) }
	                  })
	                },
	                { 0x019, new CMwClassInfo("CControlUrlLinks", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwMethodInfo("ForceDirty", 0x00000000, null, null) },
		                { 0x001, new CMwFieldInfo("CurFocusedLink", 0x0000001F) }
	                  })
	                },
	                { 0x01A, new CMwClassInfo("CControlTimeLine", new Dictionary<int, CMwMemberInfo>()
	                  {
	                  })
	                },
	                { 0x01B, new CMwClassInfo("CControlQuad", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("IsLines", 0x00000001) },
		                { 0x001, new CMwFieldInfo("IsFill", 0x00000001) },
		                { 0x002, new CMwEnumInfo("GradientDir", new string[] { "LeftToRight", "RightToLeft", "TopToBottom", "BottomToTop" }) },
		                { 0x003, new CMwFieldInfo("IconId", 0x0000001B) }
	                  })
	                },
	                { 0x01C, new CMwClassInfo("CControlEffectMaster", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("FocusEffect", 0x00000005) },
		                { 0x001, new CMwFieldInfo("FocusGainedEffect", 0x00000005) },
		                { 0x002, new CMwFieldInfo("FocusLostEffect", 0x00000005) },
		                { 0x003, new CMwFieldInfo("FocusGainedByAnotherEffect", 0x00000005) },
		                { 0x004, new CMwFieldInfo("FocusLostByAnotherEffect", 0x00000005) },
		                { 0x005, new CMwFieldInfo("SleepingEffect", 0x00000005) },
		                { 0x006, new CMwFieldInfo("ShowingEffect", 0x00000005) },
		                { 0x007, new CMwFieldInfo("HidingEffect", 0x00000005) },
		                { 0x008, new CMwFieldInfo("ActionEffect", 0x00000005) },
		                { 0x009, new CMwFieldInfo("ManagedEffect", 0x00000005) },
		                { 0x00A, new CMwFieldInfo("UseRefBBox", 0x00000001) },
		                { 0x00B, new CMwFieldInfo("ShowActivated", 0x00000001) },
		                { 0x00C, new CMwFieldInfo("HideActivated", 0x00000001) },
		                { 0x00D, new CMwFieldInfo("SpecialEffect", 0x00000005) }
	                  })
	                },
	                { 0x01D, new CMwClassInfo("CControlCredit", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("Constructed", 0x00000001) },
		                { 0x001, new CMwFieldInfo("IsFinished", 0x00000001) },
		                { 0x002, new CMwFieldInfo("AnimEnabled", 0x00000001) },
		                { 0x003, new CMwFieldInfo("LocalTimer", 0x00000024) },
		                { 0x004, new CMwFieldInfo("Speed", 0x00000024) },
		                { 0x005, new CMwMethodInfo("Start", 0x00000000, null, null) },
		                { 0x006, new CMwMethodInfo("Stop", 0x00000000, null, null) },
		                { 0x007, new CMwMethodInfo("Rewind", 0x00000000, null, null) },
		                { 0x008, new CMwFieldInfo("StyleBlock", 0x00000005) },
		                { 0x009, new CMwFieldInfo("StyleTitle", 0x00000005) },
		                { 0x00A, new CMwFieldInfo("StyleSubTitle", 0x00000005) },
		                { 0x00B, new CMwFieldInfo("StyleText", 0x00000005) },
		                { 0x00C, new CMwFieldInfo("GlobalLayout", 0x00000005) },
		                { 0x00D, new CMwFieldInfo("BlocksLayout", 0x00000005) }
	                  })
	                },
	                { 0x01E, new CMwClassInfo("CControlColorChooser", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwEnumInfo("StyleType", new string[] { "HueSlider", "SLSquare" }) },
		                { 0x001, new CMwFieldInfo("ColorChooserSize", 0x00000031) },
		                { 0x002, new CMwFieldInfo("Color", 0x00000009) },
		                { 0x003, new CMwFieldInfo("Hue", 0x00000028) },
		                { 0x004, new CMwFieldInfo("ColorChooserShader", 0x00000005) }
	                  })
	                },
	                { 0x01F, new CMwClassInfo("CControlColorChooser2", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("Color", 0x00000009) },
		                { 0x001, new CMwFieldInfo("ColorChooserHue", 0x00000005) },
		                { 0x002, new CMwFieldInfo("ColorChooserSV", 0x00000005) }
	                  })
	                },
	                { 0x021, new CMwClassInfo("CControlSimi2", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("PosX", 0x00000024) },
		                { 0x001, new CMwFieldInfo("PosY", 0x00000024) },
		                { 0x002, new CMwFieldInfo("Rot", 0x00000024) },
		                { 0x003, new CMwFieldInfo("ScaleX", 0x00000024) },
		                { 0x004, new CMwFieldInfo("ScaleY", 0x00000024) }
	                  })
	                },
	                { 0x022, new CMwClassInfo("CControlTimeLine2", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("DisplayTrackCount", 0x0000001F) },
		                { 0x001, new CMwFieldInfo("DisplayTrackStart", 0x0000001F) },
		                { 0x002, new CMwFieldInfo("TimeMin", 0x00000024) },
		                { 0x003, new CMwFieldInfo("TimeMax", 0x00000024) },
		                { 0x004, new CMwFieldInfo("RulerLength", 0x00000024) },
		                { 0x005, new CMwFieldInfo("Width", 0x00000024) },
		                { 0x006, new CMwFieldInfo("Height", 0x00000024) },
		                { 0x007, new CMwFieldInfo("TrackHeight", 0x00000024) },
		                { 0x008, new CMwFieldInfo("BlockHeight", 0x00000024) },
		                { 0x009, new CMwFieldInfo("RulerHeight", 0x00000024) },
		                { 0x00A, new CMwFieldInfo("TimeCursorWidth", 0x00000024) },
		                { 0x00B, new CMwFieldInfo("TimeCursorHeight", 0x00000024) },
		                { 0x00C, new CMwFieldInfo("KeyHeight", 0x00000024) },
		                { 0x00D, new CMwFieldInfo("KeyWidth", 0x00000024) },
		                { 0x00E, new CMwFieldInfo("KeyHighLightHeight", 0x00000024) },
		                { 0x00F, new CMwFieldInfo("KeyHighLightWidth", 0x00000024) },
		                { 0x010, new CMwFieldInfo("RulerLinesColorBig", 0x00000009) },
		                { 0x011, new CMwFieldInfo("RulerLinesColorSmall", 0x00000009) },
		                { 0x012, new CMwFieldInfo("LineSeparationColor", 0x00000009) },
		                { 0x013, new CMwFieldInfo("TimeCursorLineColor", 0x00000009) },
		                { 0x014, new CMwFieldInfo("BlockColor", 0x00000009) },
		                { 0x015, new CMwFieldInfo("BlockZ", 0x00000024) },
		                { 0x016, new CMwFieldInfo("KeyZ", 0x00000024) },
		                { 0x017, new CMwFieldInfo("BackgroundZ", 0x00000024) },
		                { 0x018, new CMwFieldInfo("RulerZ", 0x00000024) },
		                { 0x019, new CMwFieldInfo("TimeCursorZ", 0x00000024) },
		                { 0x01A, new CMwFieldInfo("LineSeparationZ", 0x00000024) },
		                { 0x01B, new CMwFieldInfo("ShaderBackground", 0x00000005) },
		                { 0x01C, new CMwFieldInfo("ShaderKey", 0x00000005) },
		                { 0x01D, new CMwFieldInfo("ShaderKeyHighLight", 0x00000005) },
		                { 0x01E, new CMwFieldInfo("ShaderBlock", 0x00000005) },
		                { 0x01F, new CMwFieldInfo("ShaderBlockHighLight", 0x00000005) },
		                { 0x020, new CMwFieldInfo("ShaderTimeCursor", 0x00000005) },
		                { 0x021, new CMwFieldInfo("ShaderRulerBackground", 0x00000005) },
		                { 0x022, new CMwFieldInfo("ShaderDeadZone", 0x00000005) },
		                { 0x023, new CMwMethodInfo("CreateSampleData", 0x00000000, null, null) },
		                { 0x024, new CMwFieldInfo("Time", 0x00000024) }
	                  })
	                },
	                { 0x023, new CMwClassInfo("CControlEffectCombined", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("Effects", 0x00000005) }
	                  })
	                },
	                { 0x024, new CMwClassInfo("CControlDisplayGraph", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwMethodInfo("AddAutoVal", 0x00000041,
			                new uint[] { 0x01001000, 0x00000029, 0x0000001F, 0x00000035, 0x00000024, 0x00000024 },
			                new string[] { "Nod", "Param", "DisplayMode", "Color", "MinRange", "MaxRange" })
		                },
		                { 0x001, new CMwMethodInfo("AddConstant", 0x00000041,
			                new uint[] { 0x00000024, 0x00000035 },
			                new string[] { "Value", "Color" })
		                }
	                  })
	                },
	                { 0x025, new CMwClassInfo("CControlEffectMoveFrame", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("DefaultShift", 0x00000031) },
		                { 0x001, new CMwFieldInfo("Period", 0x00000024) },
		                { 0x002, new CMwMethodInfo("AddChildShift", 0x00000000, null, null) },
		                { 0x003, new CMwMethodInfo("RemoveChildShift", 0x00000000, null, null) },
		                { 0x004, new CMwFieldInfo("IsInverse", 0x00000001) },
		                { 0x005, new CMwFieldInfo("AngleFrom", 0x00000035) }
	                  })
	                },
	                { 0x026, new CMwClassInfo("CControlFrameStyled", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("StyleSheet", 0x00000005) }
	                  })
	                },
	                { 0x027, new CMwClassInfo("CControlStyleSheet", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("MasterStyle", 0x00000005) },
		                { 0x001, new CMwFieldInfo("Buffer", 0x00000005) },
		                { 0x002, new CMwFieldInfo("Colors", 0x0000000A) },
		                { 0x003, new CMwFieldInfo("ColorsAlpha", 0x00000025) }
	                  })
	                },
	                { 0x028, new CMwClassInfo("CControlListMap", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("Bitmap", 0x00000005) },
		                { 0x001, new CMwFieldInfo("StackTextTargetCoord", 0x00000029) },
		                { 0x002, new CMwFieldInfo("StackTextOriginCoord", 0x00000029) },
		                { 0x003, new CMwFieldInfo("MapRectMin", 0x00000031) },
		                { 0x004, new CMwFieldInfo("MapRectMax", 0x00000031) }
	                  })
	                },
	                { 0x029, new CMwClassInfo("CControlListMap2", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("Bitmap", 0x00000005) },
		                { 0x001, new CMwFieldInfo("Material", 0x00000005) },
		                { 0x002, new CMwFieldInfo("MapRectMin", 0x00000031) },
		                { 0x003, new CMwFieldInfo("MapRectMax", 0x00000031) },
		                { 0x004, new CMwFieldInfo("StyleElem", 0x00000005) },
		                { 0x005, new CMwFieldInfo("StyleHelper1", 0x00000005) },
		                { 0x006, new CMwFieldInfo("StyleHelper1Elem", 0x00000005) },
		                { 0x007, new CMwFieldInfo("StyleHelper2", 0x00000005) },
		                { 0x008, new CMwFieldInfo("SpecialStyleElem", 0x00000005) },
		                { 0x009, new CMwFieldInfo("SpecialStyleHelper1", 0x00000005) },
		                { 0x00A, new CMwFieldInfo("SpecialStyleHelper1Elem", 0x00000005) },
		                { 0x00B, new CMwFieldInfo("SpecialStyleHelper2", 0x00000005) },
		                { 0x00C, new CMwFieldInfo("StackTextElem", 0x00000029) },
		                { 0x00D, new CMwFieldInfo("StackTextElemCoord", 0x00000029) },
		                { 0x00E, new CMwFieldInfo("StackTextHelper1", 0x00000029) },
		                { 0x00F, new CMwFieldInfo("StackTextHelper1Coord", 0x00000029) },
		                { 0x010, new CMwFieldInfo("StackTextHelper1Elem", 0x00000029) },
		                { 0x011, new CMwFieldInfo("StackTextHelper1ElemCoord", 0x00000029) },
		                { 0x012, new CMwFieldInfo("StackTextHelper2", 0x00000029) },
		                { 0x013, new CMwFieldInfo("StackTextHelper2Coord", 0x00000029) },
		                { 0x014, new CMwFieldInfo("SpecialElemName", 0x00000029) },
		                { 0x015, new CMwFieldInfo("Helper1EnableIconIndex", 0x0000001F) },
		                { 0x016, new CMwFieldInfo("Helper1DisableIconIndex", 0x0000001F) }
	                  })
	                },
	                { 0x02A, new CMwClassInfo("CControlCurve", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("Curve", 0x00000005) },
		                { 0x001, new CMwFieldInfo("AreControlPointsVisible", 0x00000001) },
		                { 0x002, new CMwFieldInfo("CurveColor", 0x00000009) },
		                { 0x003, new CMwFieldInfo("ControlPointsColor", 0x00000009) },
		                { 0x004, new CMwFieldInfo("SelectedControlPointColor", 0x00000009) },
		                { 0x005, new CMwFieldInfo("CurrentLayer", 0x0000001F) },
		                { 0x006, new CMwFieldInfo("LayersCount", 0x0000001F) },
		                { 0x007, new CMwFieldInfo("SelectedControlPoint", 0x0000001F) },
		                { 0x008, new CMwFieldInfo("CurvePrecision", 0x0000001F) },
		                { 0x009, new CMwFieldInfo("CurveHalfWidth", 0x00000024) },
		                { 0x00A, new CMwFieldInfo("UseSnapping", 0x00000001) },
		                { 0x00B, new CMwFieldInfo("GridX", 0x00000024) },
		                { 0x00C, new CMwFieldInfo("GridY", 0x00000024) },
		                { 0x00D, new CMwFieldInfo("StartX", 0x00000024) },
		                { 0x00E, new CMwFieldInfo("EndX", 0x00000024) },
		                { 0x00F, new CMwFieldInfo("StartY", 0x00000024) },
		                { 0x010, new CMwFieldInfo("EndY", 0x00000024) },
		                { 0x011, new CMwFieldInfo("CurveTree", 0x00000005) },
		                { 0x012, new CMwFieldInfo("CurveVisual", 0x00000005) },
		                { 0x013, new CMwFieldInfo("ControlPointVisual", 0x00000005) },
		                { 0x014, new CMwFieldInfo("ControlPointShader", 0x00000005) },
		                { 0x015, new CMwFieldInfo("SelectedControlPointShader", 0x00000005) },
		                { 0x016, new CMwEnumInfo("CurveDrawMode", new string[] { "Steps", "Linear", "Smooth" }) },
		                { 0x017, new CMwMethodInfo("ScreenToControlSpace", 0x00000041,
			                new uint[] { 0x00000031, 0x00000031 },
			                new string[] { "PosInControlSpace", "PosInScreenSpace" })
		                }
	                  })
	                },
	                { 0x02B, new CMwClassInfo("CControlIconIndex", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("IndexOff", 0x0000001F) },
		                { 0x001, new CMwFieldInfo("IndexOffFocused", 0x0000001F) },
		                { 0x002, new CMwFieldInfo("IndexOffGrayed", 0x0000001F) },
		                { 0x003, new CMwFieldInfo("IndexOn", 0x0000001F) },
		                { 0x004, new CMwFieldInfo("IndexOnFocused", 0x0000001F) },
		                { 0x005, new CMwFieldInfo("IndexOnGrayed", 0x0000001F) },
		                { 0x006, new CMwFieldInfo("MarginPercentU", 0x0000001F) },
		                { 0x007, new CMwFieldInfo("MarginPercentV", 0x0000001F) },
		                { 0x008, new CMwFieldInfo("MarginSizeX", 0x00000024) },
		                { 0x009, new CMwFieldInfo("MarginSizeY", 0x00000024) }
	                  })
	                },
	                { 0x02C, new CMwClassInfo("CControlMediaPlayer", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("MediaAudio", 0x00000005) },
		                { 0x001, new CMwFieldInfo("MediaVideo", 0x00000005) }
	                  })
	                },
	                { 0x02D, new CMwClassInfo("CControlRadar", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("Resources", 0x00000005) },
		                { 0x001, new CMwFieldInfo("Overlay", 0x00000005) },
		                { 0x002, new CMwFieldInfo("Screen", 0x00000005) },
		                { 0x003, new CMwFieldInfo("Scale", 0x00000024) },
		                { 0x004, new CMwFieldInfo("FollowOnlyPosition", 0x00000001) },
		                { 0x005, new CMwMethodInfo("AddDummyMobil", 0x00000000, null, null) }
	                  })
	                },
	                { 0x02E, new CMwClassInfo("CControlMediaItem", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("BitmapFid", 0x00000005) }
	                  })
	                },
	                { 0x02F, new CMwClassInfo("CControlImage", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwEnumInfo("VisualType", new string[] { "Image", "Quad", "Icon" }) },
		                { 0x001, new CMwFieldInfo("Bitmap", 0x00000005) },
		                { 0x002, new CMwFieldInfo("SizeX", 0x00000024) },
		                { 0x003, new CMwFieldInfo("SizeY", 0x00000024) },
		                { 0x004, new CMwFieldInfo("IsLines", 0x00000001) },
		                { 0x005, new CMwFieldInfo("IsFill", 0x00000001) },
		                { 0x006, new CMwEnumInfo("GradientDir", new string[] { "LeftToRight", "RightToLeft", "TopToBottom", "BottomToTop" }) },
		                { 0x007, new CMwFieldInfo("Icons", 0x00000005) },
		                { 0x008, new CMwFieldInfo("SubIconIndexOff", 0x0000001F) },
		                { 0x009, new CMwFieldInfo("SubIconIndexOn", 0x0000001F) },
		                { 0x00A, new CMwFieldInfo("IconId", 0x00000029) }
	                  })
	                },
	                { 0x030, new CMwClassInfo("CControlPager", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("ButtonsStyle", 0x00000005) },
		                { 0x001, new CMwFieldInfo("TextStyle", 0x00000005) },
		                { 0x002, new CMwFieldInfo("StackPageCountText", 0x00000029) },
		                { 0x003, new CMwFieldInfo("UseCounter", 0x00000001) },
		                { 0x004, new CMwFieldInfo("UseFastPrevNext", 0x00000001) },
		                { 0x005, new CMwFieldInfo("UseFirstLast", 0x00000001) },
		                { 0x006, new CMwFieldInfo("FastPrevNextIncrement", 0x0000001F) },
		                { 0x007, new CMwFieldInfo("LabelPageCounter", 0x00000005) },
		                { 0x008, new CMwFieldInfo("ButtonPrevPage", 0x00000005) },
		                { 0x009, new CMwFieldInfo("ButtonNextPage", 0x00000005) },
		                { 0x00A, new CMwFieldInfo("ButtonFastPrevPage", 0x00000005) },
		                { 0x00B, new CMwFieldInfo("ButtonFastNextPage", 0x00000005) },
		                { 0x00C, new CMwFieldInfo("ButtonFirstPage", 0x00000005) },
		                { 0x00D, new CMwFieldInfo("ButtonLastPage", 0x00000005) },
		                { 0x00E, new CMwMethodInfo("OnPrevPage", 0x00000000, null, null) },
		                { 0x00F, new CMwMethodInfo("OnNextPage", 0x00000000, null, null) },
		                { 0x010, new CMwMethodInfo("OnFastPrevPage", 0x00000000, null, null) },
		                { 0x011, new CMwMethodInfo("OnFastNextPage", 0x00000000, null, null) },
		                { 0x012, new CMwMethodInfo("OnFirstPage", 0x00000000, null, null) },
		                { 0x013, new CMwMethodInfo("OnLastPage", 0x00000000, null, null) }
	                  })
	                },
	                { 0x031, new CMwClassInfo("CControlText", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("ClipLength", 0x00000024) },
		                { 0x001, new CMwFieldInfo("MaxLine", 0x0000001F) },
		                { 0x002, new CMwEnumInfo("TextMode", new string[] { "Normal", "Editable", "GrayedOut", "Default" }) },
		                { 0x003, new CMwFieldInfo("TextTree", 0x00000005) }
	                  })
	                },
	                { 0x032, new CMwClassInfo("CControlFrameAnimated", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("IsClippingContainer", 0x00000001) },
		                { 0x001, new CMwFieldInfo("ScrollVerticalDistance", 0x00000024) },
		                { 0x002, new CMwFieldInfo("ScrollHorizontalDistance", 0x00000024) },
		                { 0x003, new CMwFieldInfo("ScrollPeriod", 0x00000024) },
		                { 0x004, new CMwFieldInfo("ScrollCycleTime", 0x00000024) },
		                { 0x005, new CMwFieldInfo("DoScrolling", 0x00000001) },
		                { 0x006, new CMwFieldInfo("ScrolledVerticalDistance", 0x00000024) },
		                { 0x007, new CMwFieldInfo("ScrolledHorizontalDistance", 0x00000024) },
		                { 0x008, new CMwFieldInfo("ScrollVerticalHistory", 0x00000024) },
		                { 0x009, new CMwFieldInfo("ScrollHorizontalHistory", 0x00000024) }
	                  })
	                }
                  })
                },
                { 0x08, new CMwEngineInfo("Motion", new Dictionary<int, CMwClassInfo>()
                  {
	                { 0x001, new CMwClassInfo("CMotion", new Dictionary<int, CMwMemberInfo>()
	                  {
	                  })
	                },
	                { 0x027, new CMwClassInfo("CMotionFunc", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("Func", 0x00000005) }
	                  })
	                },
	                { 0x028, new CMwClassInfo("CMotions", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("Motions", 0x00000006) }
	                  })
	                },
	                { 0x029, new CMwClassInfo("CMotionCmdBase", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("Period", 0x00000024) },
		                { 0x001, new CMwFieldInfo("Phase", 0x00000024) },
		                { 0x002, new CMwFieldInfo("Value", 0x00000024) },
		                { 0x003, new CMwEnumInfo("WaveType", new string[] { "Sin", "Triangle", "Square", "SawTooth", "InverseSawTooth" }) },
		                { 0x004, new CMwFieldInfo("IsOnce", 0x00000001) },
		                { 0x005, new CMwFieldInfo("NormedValue", 0x00000024) },
		                { 0x006, new CMwFieldInfo("IsAbsolutePhase", 0x00000001) },
		                { 0x007, new CMwFieldInfo("LoopCount", 0x0000001F) },
		                { 0x008, new CMwFieldInfo("CmdBaseParams", 0x00000005) }
	                  })
	                },
	                { 0x02B, new CMwClassInfo("CMotionShader", new Dictionary<int, CMwMemberInfo>()
	                  {
	                  })
	                },
	                { 0x02D, new CMwClassInfo("CMotionCmdBaseParams", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwEnumInfo("TimeUnit", new string[] { "mSecond", "Second", "Minute", "Hour", "Day" }) },
		                { 0x001, new CMwFieldInfo("Period", 0x00000024) },
		                { 0x002, new CMwFieldInfo("Phase", 0x00000024) }
	                  })
	                },
	                { 0x02E, new CMwClassInfo("CMotionPlaySound", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("KeysSound", 0x00000005) },
		                { 0x001, new CMwFieldInfo("SoundToRecord", 0x00000005) }
	                  })
	                },
	                { 0x02F, new CMwClassInfo("CMotionPlaySoundMobil", new Dictionary<int, CMwMemberInfo>()
	                  {
	                  })
	                },
	                { 0x030, new CMwClassInfo("CMotionPath", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("Path", 0x00000005) },
		                { 0x001, new CMwFieldInfo("IsOrientation", 0x00000001) },
		                { 0x002, new CMwFieldInfo("Target", 0x00000005) }
	                  })
	                },
	                { 0x031, new CMwClassInfo("CMotionPlay", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("LastPlayedKey", 0x0000001F) }
	                  })
	                },
	                { 0x032, new CMwClassInfo("CMotionPlayCmd", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("KeysCmd", 0x00000005) }
	                  })
	                },
	                { 0x033, new CMwClassInfo("CMotionTrack", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("Owner", 0x00000005) }
	                  })
	                },
	                { 0x034, new CMwClassInfo("CMotionPlayer", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwMethodInfo("Play", 0x00000000, null, null) },
		                { 0x001, new CMwMethodInfo("Pause", 0x00000000, null, null) },
		                { 0x002, new CMwMethodInfo("Stop", 0x00000000, null, null) },
		                { 0x003, new CMwFieldInfo("IsPlaying", 0x00000001) },
		                { 0x004, new CMwFieldInfo("Base", 0x00000005) },
		                { 0x005, new CMwFieldInfo("BaseValue", 0x00000024) },
		                { 0x006, new CMwEnumInfo("SavePlayState", new string[] { "Playing", "Paused", "Stopped", "Current" }) },
		                { 0x007, new CMwFieldInfo("Tracks", 0x00000006) },
		                { 0x008, new CMwFieldInfo("IsPhysics", 0x00000001) }
	                  })
	                },
	                { 0x035, new CMwClassInfo("CMotionTrackMobilRotate", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("LoopAngle", 0x00000024) },
		                { 0x001, new CMwFieldInfo("Axe", 0x00000035) },
		                { 0x002, new CMwFieldInfo("IsLeftMult", 0x00000001) },
		                { 0x003, new CMwFieldInfo("RotationCenter", 0x00000035) }
	                  })
	                },
	                { 0x036, new CMwClassInfo("CMotionSkelSimple", new Dictionary<int, CMwMemberInfo>()
	                  {
	                  })
	                },
	                { 0x037, new CMwClassInfo("CMotionSkel", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("KeysSkel", 0x00000005) },
		                { 0x001, new CMwFieldInfo("CurFrame", 0x0000001F) }
	                  })
	                },
	                { 0x038, new CMwClassInfo("CMotionLight", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("FuncLight", 0x00000005) }
	                  })
	                },
	                { 0x039, new CMwClassInfo("CMotionTrackMobilMove", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("KeysTrans", 0x00000005) },
		                { 0x001, new CMwFieldInfo("IsAbsolute", 0x00000001) },
		                { 0x002, new CMwMethodInfo("BuildChordLengthParametrization", 0x00000000, null, null) },
		                { 0x003, new CMwFieldInfo("ParametrizationDistMax", 0x00000024) }
	                  })
	                },
	                { 0x040, new CMwClassInfo("CMotionTrackVisual", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("Visual", 0x00000005) },
		                { 0x001, new CMwFieldInfo("FuncVisual", 0x00000005) }
	                  })
	                },
	                { 0x041, new CMwClassInfo("CMotionTrackMobilPitchin", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwEnumInfo("PitchinMode", new string[] { "Normal", "Simple" }) },
		                { 0x001, new CMwFieldInfo("Sea", 0x00000005) },
		                { 0x002, new CMwFieldInfo("Flottaison", 0x00000024) },
		                { 0x003, new CMwFieldInfo("Tangage", 0x00000024) },
		                { 0x004, new CMwFieldInfo("Roulis", 0x00000024) },
		                { 0x005, new CMwFieldInfo("OffsetHauteur", 0x00000024) },
		                { 0x006, new CMwFieldInfo("PeriodDelta", 0x00000024) },
		                { 0x007, new CMwFieldInfo("MaxAngle", 0x00000024) }
	                  })
	                },
	                { 0x042, new CMwClassInfo("CMotionTrackTree", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("Tree", 0x00000005) },
		                { 0x001, new CMwFieldInfo("FuncTree", 0x00000005) }
	                  })
	                },
	                { 0x043, new CMwClassInfo("CMotionTeamActionInfo", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwEnumInfo("TeamMateName", new string[] {  }) },
		                { 0x001, new CMwEnumInfo("Destination", new string[] {  }) },
		                { 0x002, new CMwEnumInfo("AnimAtDest", new string[] {  }) },
		                { 0x003, new CMwFieldInfo("AnimAtDestDuration", 0x00000024) },
		                { 0x004, new CMwFieldInfo("TeamMateSpeed", 0x00000024) }
	                  })
	                },
	                { 0x044, new CMwClassInfo("CMotionTeamManager", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("FuncPathMesh", 0x00000005) },
		                { 0x001, new CMwFieldInfo("TeamTrees", 0x00000007) },
		                { 0x002, new CMwFieldInfo("AnimList", 0x00000007) },
		                { 0x003, new CMwFieldInfo("WalkAnimIndex", 0x0000001F) },
		                { 0x004, new CMwMethodInfo("AddAction", 0x00000000, null, null) },
		                { 0x005, new CMwFieldInfo("Actions", 0x00000007) }
	                  })
	                },
	                { 0x045, new CMwClassInfo("CMotionTeamAction", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwMethodInfo("AddTeamMate", 0x00000000, null, null) },
		                { 0x001, new CMwFieldInfo("TeamMates", 0x00000007) }
	                  })
	                },
	                { 0x046, new CMwClassInfo("CMotionManagerCharacter", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwMethodInfo("Install", 0x00000000, null, null) },
		                { 0x001, new CMwMethodInfo("Uninstall", 0x00000000, null, null) },
		                { 0x002, new CMwFieldInfo("FuncManager", 0x00000005) },
		                { 0x003, new CMwFieldInfo("GroupPlayers", 0x00000007) },
		                { 0x004, new CMwFieldInfo("EyeLeft", 0x00000005) },
		                { 0x005, new CMwFieldInfo("EyeRight", 0x00000005) },
		                { 0x006, new CMwFieldInfo("Head", 0x00000005) },
		                { 0x007, new CMwFieldInfo("LookAtObject", 0x00000005) }
	                  })
	                },
	                { 0x047, new CMwClassInfo("CMotionManagerCharacterAdv", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwMethodInfo("MoveFalling", 0x00000000, null, null) },
		                { 0x001, new CMwMethodInfo("MoveRun", 0x00000000, null, null) },
		                { 0x002, new CMwMethodInfo("MoveWalk", 0x00000000, null, null) },
		                { 0x003, new CMwMethodInfo("MoveWalkback", 0x00000000, null, null) },
		                { 0x004, new CMwMethodInfo("MoveIdle", 0x00000000, null, null) },
		                { 0x005, new CMwMethodInfo("MoveStrafeLeft", 0x00000000, null, null) },
		                { 0x006, new CMwMethodInfo("MoveStrafeRight", 0x00000000, null, null) }
	                  })
	                },
	                { 0x048, new CMwClassInfo("CMotionGroupPlayers", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("Players", 0x00000007) },
		                { 0x001, new CMwFieldInfo("TransitionDuration", 0x0000001F) },
		                { 0x002, new CMwMethodInfo("Play", 0x00000041,
			                new uint[] { 0x01001000 },
			                new string[] { "Player" })
		                }
	                  })
	                },
	                { 0x049, new CMwClassInfo("CMotionBone", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("Id", 0x00000029) },
		                { 0x001, new CMwFieldInfo("MinAlpha", 0x00000024) },
		                { 0x002, new CMwFieldInfo("MaxAlpha", 0x00000024) },
		                { 0x003, new CMwFieldInfo("MinBeta", 0x00000024) },
		                { 0x004, new CMwFieldInfo("MaxBeta", 0x00000024) },
		                { 0x005, new CMwFieldInfo("Active", 0x00000001) }
	                  })
	                },
	                { 0x04A, new CMwClassInfo("CMotionTrackMobilScale", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("ScaleValue", 0x00000024) }
	                  })
	                },
	                { 0x04B, new CMwClassInfo("CMotionManager", new Dictionary<int, CMwMemberInfo>()
	                  {
	                  })
	                },
	                { 0x04C, new CMwClassInfo("CMotionEmitterLeaves", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("ManagerModel", 0x00000005) },
		                { 0x001, new CMwFieldInfo("Pos", 0x00000035) },
		                { 0x002, new CMwFieldInfo("Radius", 0x00000035) }
	                  })
	                },
	                { 0x04D, new CMwClassInfo("CMotionManagerLeaves", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("MobilLeaves", 0x00000005) },
		                { 0x001, new CMwFieldInfo("IsActive", 0x00000001) }
	                  })
	                },
	                { 0x04E, new CMwClassInfo("CMotionManaged", new Dictionary<int, CMwMemberInfo>()
	                  {
	                  })
	                },
	                { 0x051, new CMwClassInfo("CMotionWindBlocker", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("UseNewWindShadowing", 0x00000001) },
		                { 0x001, new CMwFieldInfo("Pos0", 0x00000031) },
		                { 0x002, new CMwFieldInfo("Pos1", 0x00000031) },
		                { 0x003, new CMwFieldInfo("Height", 0x00000024) },
		                { 0x004, new CMwFieldInfo("BlockerCoeffX", 0x00000024) },
		                { 0x005, new CMwFieldInfo("BlockerCoeffY", 0x00000024) },
		                { 0x006, new CMwFieldInfo("BlockerCoeffYMax", 0x00000024) },
		                { 0x007, new CMwFieldInfo("BlockerValXCoeff", 0x00000024) },
		                { 0x008, new CMwFieldInfo("BlockerOffsetX", 0x00000024) },
		                { 0x009, new CMwFieldInfo("BlockerBase", 0x00000024) },
		                { 0x00A, new CMwFieldInfo("BlockerBaseXCenter", 0x00000024) },
		                { 0x00B, new CMwFieldInfo("BlockerBaseXCenterVal", 0x00000024) },
		                { 0x00C, new CMwFieldInfo("BlockerBaseXMult", 0x00000024) },
		                { 0x00D, new CMwFieldInfo("TurbulenceCoeffX", 0x00000024) },
		                { 0x00E, new CMwFieldInfo("TurbulenceCoeffY", 0x00000024) },
		                { 0x00F, new CMwFieldInfo("TurbulenceBase", 0x00000024) },
		                { 0x010, new CMwFieldInfo("TurbulenceBaseXCenter", 0x00000024) },
		                { 0x011, new CMwFieldInfo("TurbulenceBaseXCenterVal", 0x00000024) },
		                { 0x012, new CMwFieldInfo("TurbulenceBaseXMult", 0x00000024) },
		                { 0x013, new CMwFieldInfo("TurbulenceAngleDeviationMult", 0x00000024) },
		                { 0x014, new CMwFieldInfo("SillageCoeffX", 0x00000024) },
		                { 0x015, new CMwFieldInfo("SillageCoeffY", 0x00000024) },
		                { 0x016, new CMwFieldInfo("SillageBase", 0x00000024) },
		                { 0x017, new CMwFieldInfo("SillageOffsetMult", 0x00000024) },
		                { 0x018, new CMwFieldInfo("SizeF7F3Ratio", 0x00000024) },
		                { 0x019, new CMwFieldInfo("TurbulenceMaxAwa", 0x00000024) },
		                { 0x01A, new CMwFieldInfo("TurbulenceFadderAwa", 0x00000024) },
		                { 0x01B, new CMwFieldInfo("TurbulencePosRatio", 0x00000024) },
		                { 0x01C, new CMwFieldInfo("BlockerPosRatio", 0x00000024) },
		                { 0x01D, new CMwFieldInfo("SillageMaxAwa", 0x00000024) },
		                { 0x01E, new CMwFieldInfo("SillageFadderAwa", 0x00000024) },
		                { 0x01F, new CMwFieldInfo("SillageCoeffYRot", 0x00000024) },
		                { 0x020, new CMwFieldInfo("SillageRotAwa", 0x00000024) },
		                { 0x021, new CMwFieldInfo("SillageAngleDeviationDiv", 0x00000024) },
		                { 0x022, new CMwFieldInfo("SecondSegmentCenterRatio", 0x00000024) },
		                { 0x023, new CMwFieldInfo("SecondSegmentLength", 0x00000024) },
		                { 0x024, new CMwFieldInfo("WindDirDevCoef", 0x00000024) },
		                { 0x025, new CMwFieldInfo("WindDirDevMax", 0x00000024) }
	                  })
	                },
	                { 0x052, new CMwClassInfo("CMotionManagerMeteo", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("WindBlockers", 0x00000007) },
		                { 0x001, new CMwFieldInfo("WindGlobalDirection", 0x00000028) },
		                { 0x002, new CMwFieldInfo("WindGlobalIntensity", 0x00000024) },
		                { 0x003, new CMwFieldInfo("StreamGlobalIntensity", 0x00000024) },
		                { 0x004, new CMwFieldInfo("BlockerDist", 0x00000024) },
		                { 0x005, new CMwFieldInfo("TideIn01", 0x00000028) },
		                { 0x006, new CMwFieldInfo("TideBlend", 0x00000028) },
		                { 0x007, new CMwFieldInfo("MeteoPuffLull", 0x00000005) },
		                { 0x008, new CMwFieldInfo("VariationsAmp", 0x00000024) },
		                { 0x009, new CMwFieldInfo("VariationsTimeFactor", 0x00000024) },
		                { 0x00A, new CMwFieldInfo("TideBlendFunc", 0x00000005) }
	                  })
	                },
	                { 0x053, new CMwClassInfo("CMotionManagerWeathers", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwMethodInfo("SaveInModelFid", 0x00000000, null, null) },
		                { 0x001, new CMwFieldInfo("SiteLatitude", 0x00000028) },
		                { 0x002, new CMwFieldInfo("FuncWeathers", 0x00000007) },
		                { 0x003, new CMwFieldInfo("Timer", 0x00000005) },
		                { 0x004, new CMwFieldInfo("TimeRemapped", 0x00000028) },
		                { 0x005, new CMwFieldInfo("BitmapSpecularDir", 0x00000005) },
		                { 0x006, new CMwEnumInfo("ClearMode", new string[] { "Fixed", "Fog" }) },
		                { 0x007, new CMwFieldInfo("ClearColor", 0x00000009) }
	                  })
	                },
	                { 0x054, new CMwClassInfo("CMotionWeather", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("Manager", 0x00000005) }
	                  })
	                },
	                { 0x055, new CMwClassInfo("CMotionDayTime", new Dictionary<int, CMwMemberInfo>()
	                  {
	                  })
	                },
	                { 0x056, new CMwClassInfo("CMotionTimerLoop", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwMethodInfo("Stop", 0x00000000, null, null) },
		                { 0x001, new CMwFieldInfo("RealTimePhase", 0x0000001F) },
		                { 0x002, new CMwFieldInfo("TimeIn01", 0x00000028) }
	                  })
	                },
	                { 0x057, new CMwClassInfo("CMotionManagerMeteoPuffLull", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("FuncPuffLull", 0x00000005) },
		                { 0x001, new CMwFieldInfo("IsVisible", 0x00000001) }
	                  })
	                },
	                { 0x058, new CMwClassInfo("CMotionEmitterParticles", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("EmitterModel", 0x00000005) },
		                { 0x001, new CMwFieldInfo("IsActive", 0x00000001) },
		                { 0x002, new CMwFieldInfo("IsEventMode", 0x00000001) },
		                { 0x003, new CMwFieldInfo("UseOwnerLoc", 0x00000001) },
		                { 0x004, new CMwFieldInfo("EmitLoc", 0x00000013) },
		                { 0x005, new CMwFieldInfo("EmitZoneScale", 0x00000035) },
		                { 0x006, new CMwFieldInfo("EmitSpeed", 0x00000035) },
		                { 0x007, new CMwFieldInfo("EmitIntensity", 0x00000024) },
		                { 0x008, new CMwFieldInfo("EmitColor", 0x00000009) }
	                  })
	                },
	                { 0x059, new CMwClassInfo("CMotionManagerParticles", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("MobilParticles", 0x00000005) },
		                { 0x001, new CMwFieldInfo("IsActive", 0x00000001) },
		                { 0x002, new CMwFieldInfo("Emitters", 0x00000007) },
		                { 0x003, new CMwMethodInfo("UpdatePrecalc", 0x00000000, null, null) }
	                  })
	                },
	                { 0x05A, new CMwClassInfo("CMotionParticleType", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("IsActive", 0x00000001) },
		                { 0x001, new CMwEnumInfo("ParticleType", new string[] { "Standard", "MultiState", "OneParticle" }) },
		                { 0x002, new CMwEnumInfo("MultiStateRenderMode", new string[] { "LineNormal", "LineWideWorld", "LineWideScreen", "QuadCenterLeft", "QuadUp", "WaterSplash", "LightTrail", "GrassMarks" }) },
		                { 0x003, new CMwFieldInfo("MultiState_IsAsyncLink", 0x00000001) },
		                { 0x004, new CMwFieldInfo("MultiState_IsStaticParts", 0x00000001) },
		                { 0x005, new CMwFieldInfo("SortSprites", 0x00000001) },
		                { 0x006, new CMwEnumInfo("StandardRenderMode", new string[] { "QuadCamera", "WaterSplash", "QuadSpeed", "LinesSpeedCamera" }) },
		                { 0x007, new CMwFieldInfo("Material", 0x00000005) },
		                { 0x008, new CMwFieldInfo("Shader", 0x00000005) },
		                { 0x009, new CMwFieldInfo("RatioXY", 0x00000024) },
		                { 0x00A, new CMwFieldInfo("RefPos", 0x00000031) },
		                { 0x00B, new CMwEnumInfo("BirthPosType", new string[] { "Fixed", "RandomInZone", "ScaleZone", "RandomInEllipsoid", "RandomOnCircleXY" }) },
		                { 0x00C, new CMwFieldInfo("BirthPos", 0x00000035) },
		                { 0x00D, new CMwFieldInfo("ZoneScale", 0x00000035) },
		                { 0x00E, new CMwFieldInfo("Pitch", 0x00000028) },
		                { 0x00F, new CMwFieldInfo("PitchVariation", 0x00000028) },
		                { 0x010, new CMwFieldInfo("Yaw", 0x00000028) },
		                { 0x011, new CMwFieldInfo("YawVariation", 0x00000028) },
		                { 0x012, new CMwFieldInfo("Roll", 0x00000028) },
		                { 0x013, new CMwFieldInfo("RollVariation", 0x00000028) },
		                { 0x014, new CMwFieldInfo("IntensityFilter", 0x00000005) },
		                { 0x015, new CMwFieldInfo("ColorGradient", 0x00000005) },
		                { 0x016, new CMwEnumInfo("ColorGradientUse", new string[] { "RandomConstantColor", "ColorOverLife" }) },
		                { 0x017, new CMwFieldInfo("ColorModulateWithTransparency", 0x00000001) },
		                { 0x018, new CMwFieldInfo("ColorUseIntensity", 0x00000001) },
		                { 0x019, new CMwFieldInfo("MaxParticleCount", 0x0000001F) },
		                { 0x01A, new CMwEnumInfo("BirthStepType", new string[] { "Active&FixedPeriod", "Active&MinDist", "Active", "Splash", "Active&FixedDist", "SplashSimple" }) },
		                { 0x01B, new CMwFieldInfo("BirthPeriod", 0x00000024) },
		                { 0x01C, new CMwFieldInfo("BirthMinDist", 0x00000024) },
		                { 0x01D, new CMwFieldInfo("Life", 0x00000024) },
		                { 0x01E, new CMwFieldInfo("LifeVariation", 0x00000024) },
		                { 0x01F, new CMwFieldInfo("Size", 0x00000024) },
		                { 0x020, new CMwFieldInfo("SizeVariation", 0x00000024) },
		                { 0x021, new CMwFieldInfo("SizeUseIntensity", 0x00000001) },
		                { 0x022, new CMwFieldInfo("SizeUseEmissionZone", 0x00000001) },
		                { 0x023, new CMwFieldInfo("SizeEmissionZoneScale", 0x00000024) },
		                { 0x024, new CMwFieldInfo("SizeSpeedScale", 0x00000024) },
		                { 0x025, new CMwEnumInfo("SizeGen", new string[] { "GenRandom", "GenSinus" }) },
		                { 0x026, new CMwFieldInfo("SizeGenPeriod", 0x00000024) },
		                { 0x027, new CMwFieldInfo("SizeOverLife", 0x00000005) },
		                { 0x028, new CMwFieldInfo("SizeUseSizeX", 0x00000001) },
		                { 0x029, new CMwFieldInfo("SizeXOverLife", 0x00000005) },
		                { 0x02A, new CMwFieldInfo("Velocity", 0x00000024) },
		                { 0x02B, new CMwFieldInfo("VelocityVariation", 0x00000024) },
		                { 0x02C, new CMwFieldInfo("RollSpeed", 0x00000024) },
		                { 0x02D, new CMwFieldInfo("RollSpeedVariation", 0x00000024) },
		                { 0x02E, new CMwFieldInfo("ViewDist2Max", 0x00000024) },
		                { 0x02F, new CMwFieldInfo("Weight", 0x00000024) },
		                { 0x030, new CMwFieldInfo("WeightVariation", 0x00000024) },
		                { 0x031, new CMwFieldInfo("Transparency", 0x00000024) },
		                { 0x032, new CMwFieldInfo("TransparencyVariation", 0x00000024) },
		                { 0x033, new CMwFieldInfo("TransparencyOverLife", 0x00000005) },
		                { 0x034, new CMwFieldInfo("TransparencyUseIntensity", 0x00000001) },
		                { 0x035, new CMwFieldInfo("PrecalcEnable", 0x00000001) },
		                { 0x036, new CMwFieldInfo("PrecalcPartCount", 0x0000001F) },
		                { 0x037, new CMwFieldInfo("PrecalcSampleRate", 0x0000001F) },
		                { 0x038, new CMwFieldInfo("PhysicsEnable", 0x00000001) },
		                { 0x039, new CMwFieldInfo("PhysicsBounce", 0x00000024) },
		                { 0x03A, new CMwFieldInfo("PhysicsDamper", 0x00000024) },
		                { 0x03B, new CMwFieldInfo("PhysicsRadius", 0x00000024) },
		                { 0x03C, new CMwFieldInfo("OnePartPeriod", 0x0000001F) },
		                { 0x03D, new CMwFieldInfo("UScaleDist", 0x00000024) },
		                { 0x03E, new CMwFieldInfo("VScaleDist", 0x00000024) },
		                { 0x03F, new CMwEnumInfo("TextureAtlas", new string[] { "None", "Fixed", "Random" }) },
		                { 0x040, new CMwFieldInfo("TextureAtlasDimX", 0x0000001F) },
		                { 0x041, new CMwFieldInfo("TextureAtlasDimY", 0x0000001F) },
		                { 0x042, new CMwFieldInfo("TextureAtlasFixedIndex", 0x0000001F) },
		                { 0x043, new CMwFieldInfo("FluidFriction", 0x00000024) },
		                { 0x044, new CMwFieldInfo("FluidFrictionVariation", 0x00000024) },
		                { 0x045, new CMwFieldInfo("FluidFrictionUseIntensity", 0x00000001) },
		                { 0x046, new CMwFieldInfo("FluidFrictionIntensityBase", 0x00000028) },
		                { 0x047, new CMwFieldInfo("VertPerPartCount", 0x0000001F) },
		                { 0x048, new CMwFieldInfo("SplashPartCount", 0x0000001F) },
		                { 0x049, new CMwFieldInfo("SplashRadius", 0x00000024) },
		                { 0x04A, new CMwFieldInfo("SplashRadiusVaritation", 0x00000024) },
		                { 0x04B, new CMwFieldInfo("SplashSpeedAngleY", 0x00000024) },
		                { 0x04C, new CMwFieldInfo("SplashSpeedAngleVariation", 0x00000024) },
		                { 0x04D, new CMwFieldInfo("SplashSpeedCenterScaleMin", 0x00000024) },
		                { 0x04E, new CMwFieldInfo("SplashSpeedCenterScaleMax", 0x00000024) },
		                { 0x04F, new CMwFieldInfo("SplashVelocity", 0x00000024) },
		                { 0x050, new CMwFieldInfo("SplashVelocityVariation", 0x00000024) },
		                { 0x051, new CMwFieldInfo("SceneFxDistor2d", 0x00000005) },
		                { 0x052, new CMwFieldInfo("UseGameTimerElseHuman", 0x00000001) }
	                  })
	                },
	                { 0x05B, new CMwClassInfo("CMotionParticleEmitterModel", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("ParticleTypes", 0x00000007) }
	                  })
	                }
                  })
                },
                { 0x09, new CMwEngineInfo("Plug", new Dictionary<int, CMwClassInfo>()
                  {
	                { 0x001, new CMwClassInfo("CPlugAudio", new Dictionary<int, CMwMemberInfo>()
	                  {
	                  })
	                },
	                { 0x002, new CMwClassInfo("CPlugShader", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("IsDoubleSided", 0x00000001) },
		                { 0x001, new CMwFieldInfo("BiasZ", 0x0000001F) },
		                { 0x002, new CMwEnumInfo("FillMode", new string[] { "Solid", "Wired", "LineWired", "SolidWired" }) },
		                { 0x003, new CMwFieldInfo("ShadowRecvGrp0", 0x00000001) },
		                { 0x004, new CMwFieldInfo("ShadowRecvGrp1", 0x00000001) },
		                { 0x005, new CMwFieldInfo("ShadowRecvGrp2", 0x00000001) },
		                { 0x006, new CMwFieldInfo("ShadowRecvGrp3", 0x00000001) },
		                { 0x007, new CMwFieldInfo("ShadowMPassEnable", 0x00000001) },
		                { 0x008, new CMwFieldInfo("ShadowDepthBiasExtra", 0x00000001) },
		                { 0x009, new CMwFieldInfo("ShadowCasterDisable", 0x00000001) },
		                { 0x00A, new CMwFieldInfo("ShadowImageSpaceDisable", 0x00000001) },
		                { 0x00B, new CMwFieldInfo("ProjectorReceiver", 0x00000001) },
		                { 0x00C, new CMwFieldInfo("StaticLighting", 0x00000001) },
		                { 0x00D, new CMwFieldInfo("IsFogEyeZEnable", 0x00000001) },
		                { 0x00E, new CMwFieldInfo("TweakFogColorBlack", 0x00000001) },
		                { 0x00F, new CMwFieldInfo("IsFogPlaneEnable", 0x00000001) },
		                { 0x010, new CMwFieldInfo("TransTreeMip", 0x00000001) },
		                { 0x011, new CMwFieldInfo("TexCoordCount", 0x0000001F) },
		                { 0x012, new CMwFieldInfo("GReqColor0", 0x00000001) },
		                { 0x013, new CMwFieldInfo("GReqNormal", 0x00000001) },
		                { 0x014, new CMwFieldInfo("GReqTangentU", 0x00000001) },
		                { 0x015, new CMwFieldInfo("GReqTangentV", 0x00000001) },
		                { 0x016, new CMwFieldInfo("IsTranslucent", 0x00000001) },
		                { 0x017, new CMwFieldInfo("IsAlphaBlended", 0x00000001) },
		                { 0x018, new CMwFieldInfo("Alpha01SoftEdges", 0x00000001) },
		                { 0x019, new CMwEnumInfo("VertexSpace", new string[] { "None", "Fixed", "PlaneY" }) },
		                { 0x01A, new CMwEnumInfo("SpriteColor0", new string[] { "Copy", "nHalfDiagInWorld", "TmF RallyTreeRGB" }) },
		                { 0x01B, new CMwFieldInfo("FuncShader", 0x00000005) },
		                { 0x01C, new CMwFieldInfo("Passes", 0x00000006) },
		                { 0x01D, new CMwFieldInfo("VIdReflected", 0x00000001) },
		                { 0x01E, new CMwFieldInfo("VIdReflectMirror", 0x00000001) },
		                { 0x01F, new CMwFieldInfo("VIdRefracted", 0x00000001) },
		                { 0x020, new CMwFieldInfo("VIdViewDepBump", 0x00000001) },
		                { 0x021, new CMwFieldInfo("VIdViewDepOcclusion", 0x00000001) },
		                { 0x022, new CMwFieldInfo("VIdOnlyRefracted", 0x00000001) },
		                { 0x023, new CMwFieldInfo("VIdHideWhenUnderground", 0x00000001) },
		                { 0x024, new CMwFieldInfo("VIdHideWhenOverground", 0x00000001) },
		                { 0x025, new CMwFieldInfo("VIdHideAlways", 0x00000001) },
		                { 0x026, new CMwFieldInfo("VIdViewDepWindIntens", 0x00000001) },
		                { 0x027, new CMwFieldInfo("VIdBackground", 0x00000001) },
		                { 0x028, new CMwFieldInfo("VIdGrassRGB", 0x00000001) },
		                { 0x029, new CMwFieldInfo("VIdLightGenP", 0x00000001) },
		                { 0x02A, new CMwFieldInfo("VIdVehicle", 0x00000001) },
		                { 0x02B, new CMwFieldInfo("VIdHideOnlyDirect", 0x00000001) },
		                { 0x02C, new CMwFieldInfo("SortCustom", 0x00000001) },
		                { 0x02D, new CMwEnumInfo("SortPosition", new string[] { "Beginning", "BeforeAlpha01", "BeforeTrans", "Ending" }) },
		                { 0x02E, new CMwFieldInfo("SortIndex", 0x0000000E) },
		                { 0x02F, new CMwFieldInfo("SortZTest", 0x00000001) },
		                { 0x030, new CMwFieldInfo("SortZWrite", 0x00000001) },
		                { 0x031, new CMwFieldInfo("ComputeFillValue", 0x00000001) },
		                { 0x032, new CMwFieldInfo("ShaderCustom_DepthToAlpha", 0x00000005) },
		                { 0x033, new CMwFieldInfo("ShaderCustom_GlowInput", 0x00000005) }
	                  })
	                },
	                { 0x003, new CMwClassInfo("CPlugCrystal", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwEnumInfo("GenerateMode", new string[] { "Triangles", "Sprites" }) },
		                { 0x001, new CMwFieldInfo("SpriteSize", 0x00000024) }
	                  })
	                },
	                { 0x004, new CMwClassInfo("CPlugShaderGeneric", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwEnumInfo("VertexColor", new string[] { "Custom", "CopyGlobal", "CopyVertex", "LightGlobal", "LightVertex" }) },
		                { 0x001, new CMwFieldInfo("GlobalRGB", 0x00000009) },
		                { 0x002, new CMwFieldInfo("GlobalAlpha", 0x00000028) },
		                { 0x003, new CMwFieldInfo("UseAmbient", 0x00000001) },
		                { 0x004, new CMwFieldInfo("UseAmbientCV", 0x00000001) },
		                { 0x005, new CMwFieldInfo("AmbientRGB", 0x00000009) },
		                { 0x006, new CMwFieldInfo("UseDiffuse", 0x00000001) },
		                { 0x007, new CMwFieldInfo("UseDiffuseCV", 0x00000001) },
		                { 0x008, new CMwFieldInfo("DiffuseRGB", 0x00000009) },
		                { 0x009, new CMwFieldInfo("DiffuseAlpha", 0x00000028) },
		                { 0x00A, new CMwFieldInfo("UseSpecular", 0x00000001) },
		                { 0x00B, new CMwFieldInfo("SpecularRGB", 0x00000009) },
		                { 0x00C, new CMwFieldInfo("SpecularExp", 0x00000028) },
		                { 0x00D, new CMwFieldInfo("UseEmissive", 0x00000001) },
		                { 0x00E, new CMwFieldInfo("UseEmissiveCV", 0x00000001) },
		                { 0x00F, new CMwFieldInfo("EmissiveRGB", 0x00000009) },
		                { 0x010, new CMwFieldInfo("NormalizeNs", 0x00000001) },
		                { 0x011, new CMwFieldInfo("OnlyInfiniteLights", 0x00000001) },
		                { 0x012, new CMwFieldInfo("WireRGB", 0x00000009) },
		                { 0x013, new CMwFieldInfo("WireAlpha", 0x00000028) },
		                { 0x014, new CMwFieldInfo("DblSidedLighting", 0x00000001) }
	                  })
	                },
	                { 0x005, new CMwClassInfo("CPlugSolid", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("Mass", 0x00000024) },
		                { 0x001, new CMwFieldInfo("LinearFluidFriction", 0x00000024) },
		                { 0x002, new CMwFieldInfo("AngularFluidFriction", 0x00000024) },
		                { 0x003, new CMwFieldInfo("Tree", 0x00000005) },
		                { 0x004, new CMwFieldInfo("Model", 0x00000005) },
		                { 0x005, new CMwFieldInfo("UseModel", 0x00000001) },
		                { 0x006, new CMwFieldInfo("PreLightGenTileCountU", 0x0000001F) },
		                { 0x007, new CMwFieldInfo("MaxDistPerStep", 0x00000024) },
		                { 0x008, new CMwFieldInfo("ExclusionEllipsoidRadius", 0x00000024) }
	                  })
	                },
	                { 0x006, new CMwClassInfo("CPlugVisual", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwMethodInfo("Inverse", 0x00000000, null, null) },
		                { 0x001, new CMwFieldInfo("IsGeometryStatic", 0x00000001) },
		                { 0x002, new CMwFieldInfo("IsGeometryDynaPart", 0x00000001) },
		                { 0x003, new CMwFieldInfo("IsIndexationStatic", 0x00000001) },
		                { 0x004, new CMwFieldInfo("OptimizeInVision", 0x00000001) },
		                { 0x005, new CMwFieldInfo("UseVertexNormal", 0x00000001) },
		                { 0x006, new CMwFieldInfo("UseVertexColor", 0x00000001) },
		                { 0x007, new CMwFieldInfo("FuncVisual", 0x00000005) },
		                { 0x008, new CMwFieldInfo("VertexStreams", 0x00000006) },
		                { 0x009, new CMwFieldInfo("VisualToBones", 0x00000015) }
	                  })
	                },
	                { 0x008, new CMwClassInfo("CPlugBitmapHighLevel", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwEnumInfo("Mode", new string[] { "RenderPlaneReflect", "RenderCameraDepth" }) },
		                { 0x001, new CMwFieldInfo("BlurTexelCount", 0x0000001F) },
		                { 0x002, new CMwFieldInfo("Width", 0x0000001F) },
		                { 0x003, new CMwFieldInfo("Height", 0x0000001F) },
		                { 0x004, new CMwFieldInfo("CameraToWorld", 0x00000013) },
		                { 0x005, new CMwFieldInfo("CameraFovY", 0x00000024) },
		                { 0x006, new CMwFieldInfo("CameraRatioXY", 0x00000024) },
		                { 0x007, new CMwFieldInfo("CameraNearZ", 0x00000024) },
		                { 0x008, new CMwFieldInfo("CameraFarZ", 0x00000024) }
	                  })
	                },
	                { 0x009, new CMwClassInfo("CPlugVisualIndexedLines", new Dictionary<int, CMwMemberInfo>()
	                  {
	                  })
	                },
	                { 0x00A, new CMwClassInfo("CPlugVisualOctree", new Dictionary<int, CMwMemberInfo>()
	                  {
	                  })
	                },
	                { 0x00B, new CMwClassInfo("CPlugBitmapRenderShadow", new Dictionary<int, CMwMemberInfo>()
	                  {
	                  })
	                },
	                { 0x00C, new CMwClassInfo("CPlugSurface", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("Geom", 0x00000005) },
		                { 0x001, new CMwFieldInfo("Materials", 0x00000007) }
	                  })
	                },
	                { 0x00E, new CMwClassInfo("CPlugVisualIndexedTriangles2D", new Dictionary<int, CMwMemberInfo>()
	                  {
	                  })
	                },
	                { 0x00F, new CMwClassInfo("CPlugSurfaceGeom", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwEnumInfo("GmSurfType", new string[] { "Sphere", "Ellipsoid", "Plane", "QuadHeight", "TriangleHeight", "Polygon", "Box", "Mesh", "Cylinder" }) },
		                { 0x001, new CMwFieldInfo("Radius", 0x00000024) },
		                { 0x002, new CMwFieldInfo("Radii", 0x00000035) },
		                { 0x003, new CMwFieldInfo("IsWeight", 0x00000001) }
	                  })
	                },
	                { 0x010, new CMwClassInfo("CPlugVisualSprite", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwEnumInfo("RenderMode", new string[] { "Quad", "RotatedQuad" }) },
		                { 0x001, new CMwFieldInfo("RadiusInScreen", 0x00000001) },
		                { 0x002, new CMwFieldInfo("UseGlobalDir", 0x00000001) },
		                { 0x003, new CMwFieldInfo("UseTextureAtlas", 0x00000001) },
		                { 0x004, new CMwFieldInfo("SortBackToFront", 0x00000001) },
		                { 0x005, new CMwFieldInfo("GlobalDirection", 0x00000035) },
		                { 0x006, new CMwFieldInfo("PivotPoint", 0x00000031) },
		                { 0x007, new CMwFieldInfo("ZBiasFactor", 0x00000028) },
		                { 0x008, new CMwFieldInfo("AtlasGridCountU", 0x0000001F) },
		                { 0x009, new CMwFieldInfo("AtlasGridCountV", 0x0000001F) }
	                  })
	                },
	                { 0x011, new CMwClassInfo("CPlugBitmap", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwEnumInfo("Usage", new string[] { "Color", "Light", "Height2DuDv", "Render", "Height2DuDvLumi", "Height2NxNyNz", "Height2NxNy", "Depth", "DispH01", "Height2NormPal8b", "NxNyNz", "NxNy", "NormPal8b", "NormPal16b", "ColorFloat", "RenderFloat", "Height2DuDv1", "Alpha", "LightAlpha", "Height2RxG0BzAy", "NormRxG0BzAy", "TexCoord", "Render16b", "Vertex", "Height2BumpTxTy", "BumpTxTy", "Height2R0GyBzAx", "NormR0GyBzAx", "NormXYZ->0YZX" }) },
		                { 0x001, new CMwFieldInfo("NormalAreSigned", 0x00000001) },
		                { 0x002, new CMwFieldInfo("NormalCanBeSigned", 0x00000001) },
		                { 0x003, new CMwEnumInfo("WantedColorDepth", new string[] { "DefaultColorDepth", "Color16b", "Color32b" }) },
		                { 0x004, new CMwFieldInfo("IsOneBitAlpha", 0x00000001) },
		                { 0x005, new CMwFieldInfo("IgnoreImageAlpha01", 0x00000001) },
		                { 0x006, new CMwFieldInfo("ShadowCasterIgnoreAlpha", 0x00000001) },
		                { 0x007, new CMwFieldInfo("AlphaToCoverage", 0x00000001) },
		                { 0x008, new CMwFieldInfo("IsNonPow2Conditional", 0x00000001) },
		                { 0x009, new CMwFieldInfo("IsCubeMap", 0x00000001) },
		                { 0x00A, new CMwFieldInfo("IsOriginTop", 0x00000001) },
		                { 0x00B, new CMwEnumInfo("CubeMapAuto2dFace", new string[] { "None", "XPos", "XNeg", "YPos", "YNeg", "ZPos", "ZNeg" }) },
		                { 0x00C, new CMwEnumInfo("TexFilter", new string[] { "Point", "Bilinear", "Trilinear", "Anisotropic" }) },
		                { 0x00D, new CMwEnumInfo("TexAddressU", new string[] { "Wrap", "Mirror", "Clamp", "BorderSM3" }) },
		                { 0x00E, new CMwEnumInfo("TexAddressV", new string[] { "Wrap", "Mirror", "Clamp", "BorderSM3" }) },
		                { 0x00F, new CMwEnumInfo("TexAddressW", new string[] { "Wrap", "Mirror", "Clamp", "BorderSM3" }) },
		                { 0x010, new CMwFieldInfo("MipMapLodBiasDefault", 0x00000024) },
		                { 0x011, new CMwFieldInfo("DefaultTexCoordScale", 0x00000031) },
		                { 0x012, new CMwFieldInfo("DefaultTexCoordTrans", 0x00000031) },
		                { 0x013, new CMwFieldInfo("DefaultTexCoordRotate", 0x00000028) },
		                { 0x014, new CMwEnumInfo("DefaultVideoTimer", new string[] { "Game", "Human", "External" }) },
		                { 0x015, new CMwFieldInfo("AtlasCountUs", 0x00000021) },
		                { 0x016, new CMwFieldInfo("AtlasCountVs", 0x00000021) },
		                { 0x017, new CMwFieldInfo("AtlasSubCountIs", 0x0000002B) },
		                { 0x018, new CMwFieldInfo("AtlasSubIndexUs", 0x00000021) },
		                { 0x019, new CMwFieldInfo("AtlasSubIndexVs", 0x00000021) },
		                { 0x01A, new CMwFieldInfo("Force1stPixelAlpha0", 0x00000001) },
		                { 0x01B, new CMwFieldInfo("ForceBorderRGB", 0x00000001) },
		                { 0x01C, new CMwFieldInfo("BorderRGB", 0x00000009) },
		                { 0x01D, new CMwFieldInfo("ForceBorderAlpha", 0x00000001) },
		                { 0x01E, new CMwFieldInfo("BorderAlpha", 0x00000028) },
		                { 0x01F, new CMwEnumInfo("ForceBorderSize", new string[] { "1 texel", "2 texels", "3 texels", "4 texels" }) },
		                { 0x020, new CMwFieldInfo("WantMipMapping", 0x00000001) },
		                { 0x021, new CMwFieldInfo("IsMipMapLowerAlphaEnable", 0x00000001) },
		                { 0x022, new CMwFieldInfo("MipMapLowerAlpha", 0x00000028) },
		                { 0x023, new CMwFieldInfo("MipMapFadeAlphas", 0x00000025) },
		                { 0x024, new CMwEnumInfo("MipMapAlpha01", new string[] { "HalfBinary", "ForceBinary", "ShadeOfGray" }) },
		                { 0x025, new CMwFieldInfo("CanBeDeletedFromSystemMemory", 0x00000001) },
		                { 0x026, new CMwFieldInfo("RenderTexelsMustPersist", 0x00000001) },
		                { 0x027, new CMwFieldInfo("CanBeCompressedInVideoMemory", 0x00000001) },
		                { 0x028, new CMwFieldInfo("CompressInterpolatedAlpha", 0x00000001) },
		                { 0x029, new CMwFieldInfo("CompressSkipDXT1", 0x00000001) },
		                { 0x02A, new CMwFieldInfo("CompressUseDithering", 0x00000001) },
		                { 0x02B, new CMwEnumInfo("Compressor", new string[] { "NVidia", "DirectX" }) },
		                { 0x02C, new CMwEnumInfo("BumpCompressMode", new string[] { "None(32b)", "Pal8b", "DXT1(4b)", "Pal16b" }) },
		                { 0x02D, new CMwFieldInfo("BumpScaleFactor", 0x00000024) },
		                { 0x02E, new CMwFieldInfo("BumpScaleMipLevel", 0x00000024) },
		                { 0x02F, new CMwEnumInfo("NormalRotate", new string[] { "None", "+x +z -y" }) },
		                { 0x030, new CMwFieldInfo("Image", 0x00000005) },
		                { 0x031, new CMwFieldInfo("MipLevelSkipFromQuality", 0x00000001) },
		                { 0x032, new CMwFieldInfo("MipLevelSkipCountMax", 0x0000001F) },
		                { 0x033, new CMwFieldInfo("FloatRequireFiltering", 0x00000001) },
		                { 0x034, new CMwFieldInfo("RenderRequireBlending", 0x00000001) },
		                { 0x035, new CMwFieldInfo("MultiSampleCount", 0x0000001F) },
		                { 0x036, new CMwEnumInfo("PixelUpdate", new string[] { "None", "Render", "Shader", "DynaSpecular", "Clear" }) },
		                { 0x037, new CMwFieldInfo("SpecularRGB", 0x00000009) },
		                { 0x038, new CMwFieldInfo("SpecularExp", 0x00000028) },
		                { 0x039, new CMwFieldInfo("ClearRGB", 0x00000009) },
		                { 0x03A, new CMwFieldInfo("ClearAlpha", 0x00000028) },
		                { 0x03B, new CMwFieldInfo("Render", 0x00000005) },
		                { 0x03C, new CMwFieldInfo("Shader", 0x00000005) },
		                { 0x03D, new CMwFieldInfo("ForceShaderBitmapTc", 0x00000001) },
		                { 0x03E, new CMwFieldInfo("ForceShaderGenerateUV", 0x00000001) },
		                { 0x03F, new CMwEnumInfo("GenerateUV", new string[] { "NoGenerate", "CameraVertex", "WorldVertex", "WorldVertexXY", "WorldVertexXZ", "WorldVertexYZ", "CameraNormal", "WorldNormal", "CameraReflectionVector", "WorldReflectionVector", "WorldNormalNeg", "WaterReflectionVector", "Hack1Vertex", "MapTexel", "FogPlane0", "Vsk3SeaFoam", "ImageSpace", "LightDir0Reflect", "EyeNormal", "ShadowB1Pw01", "Tex3AsPosPrCamera", "FlatWaterReflect", "FlatWaterRefract", "FlatWaterFresnel" }) }
	                  })
	                },
	                { 0x012, new CMwClassInfo("CPlugBitmapApply", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwEnumInfo("TexInput", new string[] { "Current", "Constant" }) },
		                { 0x001, new CMwEnumInfo("TexArg", new string[] { "Texture", "VColor0", "Temporary", "VColor1" }) },
		                { 0x002, new CMwEnumInfo("TextureOp", new string[] { "Replace", "Modulate", "Modulate2X", "Add", "BlendAlpha", "AddDstColorModBySrcAlpha", "AddSrcColorModByDstAlpha", "BumpEnvMap", "BumpEnvMapLumi", "DotProduct3", "AddSigned", "AddSigned2X", "Subtract", "AddSmooth", "BlendVAlpha", "ReplaceRGB", "ReplaceAlpha", "ModulateRGB", "ModulateAlpha", "AddAlpha", "Modulate2xAlpha", "ReplaceRGB_ModulateAlpha", "Mod2xRGB_ReplaceAlpha" }) },
		                { 0x003, new CMwEnumInfo("TexOutput", new string[] { "Current", "Temporary" }) }
	                  })
	                },
	                { 0x013, new CMwClassInfo("CPlugVisualLines", new Dictionary<int, CMwMemberInfo>()
	                  {
	                  })
	                },
	                { 0x014, new CMwClassInfo("CPlugVisualLines2D", new Dictionary<int, CMwMemberInfo>()
	                  {
	                  })
	                },
	                { 0x015, new CMwClassInfo("CPlugTreeVisualMip", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("LevelsFarZ", 0x00000025) },
		                { 0x001, new CMwFieldInfo("LevelsTree", 0x00000006) }
	                  })
	                },
	                { 0x016, new CMwClassInfo("CPlugVisualStrip", new Dictionary<int, CMwMemberInfo>()
	                  {
	                  })
	                },
	                { 0x017, new CMwClassInfo("CPlugVisualVertexs", new Dictionary<int, CMwMemberInfo>()
	                  {
	                  })
	                },
	                { 0x019, new CMwClassInfo("CPlugFilePack", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("KeyStringForImport", 0x00000029) }
	                  })
	                },
	                { 0x01A, new CMwClassInfo("CPlugSound", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("PlugFile", 0x00000005) },
		                { 0x001, new CMwMethodInfo("SetDirty", 0x00000000, null, null) },
		                { 0x002, new CMwEnumInfo("Mode", new string[] { "Static2d", "Dynamic2d", "3d", "3dOmni", "InternalForceHard3d" }) },
		                { 0x003, new CMwFieldInfo("Volume", 0x00000028) },
		                { 0x004, new CMwFieldInfo("IsLooping", 0x00000001) },
		                { 0x005, new CMwFieldInfo("IsContinuous", 0x00000001) },
		                { 0x006, new CMwFieldInfo("Priority", 0x00000024) },
		                { 0x007, new CMwFieldInfo("RefDistance", 0x00000024) },
		                { 0x008, new CMwFieldInfo("MaxDistanceOmni", 0x00000024) },
		                { 0x009, new CMwFieldInfo("EnableDoppler", 0x00000001) },
		                { 0x00A, new CMwFieldInfo("DopplerFactor", 0x00000024) },
		                { 0x00B, new CMwEnumInfo("SoundKind", new string[] { "Point", "Directional" }) },
		                { 0x00C, new CMwFieldInfo("InsideConeAngle", 0x00000023) },
		                { 0x00D, new CMwFieldInfo("OutsideConeAngle", 0x00000023) },
		                { 0x00E, new CMwFieldInfo("ConeOutsideAttenuation", 0x00000028) },
		                { 0x00F, new CMwFieldInfo("ConeOutsideAttenuationHF", 0x00000028) },
		                { 0x010, new CMwFieldInfo("VolumeAttenuationDirect", 0x00000028) },
		                { 0x011, new CMwFieldInfo("VolumeAttenuationDirectHF", 0x00000028) },
		                { 0x012, new CMwFieldInfo("VolumeAttenuationRoom", 0x00000028) },
		                { 0x013, new CMwFieldInfo("VolumeAttenuationRoomHF", 0x00000028) },
		                { 0x014, new CMwFieldInfo("RolloffFactor", 0x00000028) },
		                { 0x015, new CMwFieldInfo("RoomRolloffFactor", 0x00000028) },
		                { 0x016, new CMwFieldInfo("AirAbsorptionFactor", 0x00000028) }
	                  })
	                },
	                { 0x01B, new CMwClassInfo("CPlugSoundMood", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("EventSounds", 0x00000006) },
		                { 0x001, new CMwFieldInfo("EventPeriods", 0x00000025) }
	                  })
	                },
	                { 0x01C, new CMwClassInfo("CPlugMusic", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("AdditionalTracks", 0x00000005) }
	                  })
	                },
	                { 0x01D, new CMwClassInfo("CPlugLight", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("Light", 0x00000005) },
		                { 0x001, new CMwFieldInfo("FuncLight", 0x00000005) },
		                { 0x002, new CMwFieldInfo("BitmapProjector", 0x00000005) },
		                { 0x003, new CMwFieldInfo("BitmapFlare", 0x00000005) },
		                { 0x004, new CMwFieldInfo("NightOnly", 0x00000001) },
		                { 0x005, new CMwFieldInfo("ReflectByGround", 0x00000001) },
		                { 0x006, new CMwFieldInfo("DuplicateGxLight", 0x00000001) },
		                { 0x007, new CMwFieldInfo("SceneLightOnlyWhenTreeVisible", 0x00000001) },
		                { 0x008, new CMwMethodInfo("MakePoint", 0x00000000, null, null) },
		                { 0x009, new CMwMethodInfo("MakeBall", 0x00000000, null, null) },
		                { 0x00A, new CMwMethodInfo("MakeFrustum", 0x00000000, null, null) },
		                { 0x00B, new CMwMethodInfo("MakeSpot", 0x00000000, null, null) }
	                  })
	                },
	                { 0x01E, new CMwClassInfo("CPlugVisualIndexedTriangles", new Dictionary<int, CMwMemberInfo>()
	                  {
	                  })
	                },
	                { 0x020, new CMwClassInfo("CPlugFile", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwMethodInfo("ReGenerate", 0x00000000, null, null) }
	                  })
	                },
	                { 0x021, new CMwClassInfo("CPlugBitmapRenderLightFromMap", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("SampledWidthPerObject", 0x0000001F) },
		                { 0x001, new CMwFieldInfo("ObjectCountPerAxisMin", 0x0000001F) },
		                { 0x002, new CMwFieldInfo("ObjectCountPerAxisMax", 0x0000001F) },
		                { 0x003, new CMwFieldInfo("ObjectCountPerAxisVision", 0x0000001F) },
		                { 0x004, new CMwFieldInfo("CameraNearZ_FactorInObject", 0x00000028) },
		                { 0x005, new CMwFieldInfo("CameraFarZ_ToAdd", 0x00000024) },
		                { 0x006, new CMwFieldInfo("StartFadeToWhite", 0x00000028) },
		                { 0x007, new CMwFieldInfo("RemapMin_Night", 0x00000028) },
		                { 0x008, new CMwFieldInfo("RemapMax_Night", 0x00000028) },
		                { 0x009, new CMwFieldInfo("RemapMin_DayAmb", 0x00000028) },
		                { 0x00A, new CMwFieldInfo("RemapMax_DayAmb", 0x00000028) },
		                { 0x00B, new CMwFieldInfo("RemapMin_DayDir", 0x00000028) },
		                { 0x00C, new CMwFieldInfo("RemapMax_DayDir", 0x00000028) },
		                { 0x00D, new CMwFieldInfo("CameraDovWorldY_MaxDot", 0x00000028) }
	                  })
	                },
	                { 0x022, new CMwClassInfo("CPlugFileJpg", new Dictionary<int, CMwMemberInfo>()
	                  {
	                  })
	                },
	                { 0x023, new CMwClassInfo("CPlugFileTga", new Dictionary<int, CMwMemberInfo>()
	                  {
	                  })
	                },
	                { 0x024, new CMwClassInfo("CPlugFileDds", new Dictionary<int, CMwMemberInfo>()
	                  {
	                  })
	                },
	                { 0x025, new CMwClassInfo("CPlugFileImg", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwEnumInfo("Dimension", new string[] { "1d", "2d", "Cube", "Volume" }) },
		                { 0x001, new CMwFieldInfo("Width", 0x0000001F) },
		                { 0x002, new CMwFieldInfo("Height", 0x0000001F) },
		                { 0x003, new CMwFieldInfo("Depth", 0x0000001F) },
		                { 0x004, new CMwFieldInfo("NbComp", 0x0000001F) },
		                { 0x005, new CMwEnumInfo("Format", new string[] { "BGRA", "DXT1", "DXT2", "DXT3", "DXT4", "DXT5", "RGBA16", "RGBA16F", "RGBA32F" }) },
		                { 0x006, new CMwFieldInfo("cMipLevel", 0x0000001F) },
		                { 0x007, new CMwFieldInfo("cMipLevelSkipAtLoad", 0x0000001F) },
		                { 0x008, new CMwMethodInfo("ScaleToPowerOfTwo", 0x00000041,
			                new uint[] { 0x00000001, 0x0000001F },
			                new string[] { "Shrink", "TexFilter" })
		                }
	                  })
	                },
	                { 0x026, new CMwClassInfo("CPlugShaderApply", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("SetBlendFromOpacityMap", 0x00000001) },
		                { 0x001, new CMwEnumInfo("BlendSrc", new string[] { "0", "1", "SrcColor", "1-SrcColor", "SrcAlpha", "1-SrcAlpha", "DstColor", "1-DstColor", "DstAlpha", "1-DstAlpha", "SrcAlphaSat", "Constant", "1-Constant" }) },
		                { 0x002, new CMwEnumInfo("BlendDst", new string[] { "0", "1", "SrcColor", "1-SrcColor", "SrcAlpha", "1-SrcAlpha", "DstColor", "1-DstColor", "DstAlpha", "1-DstAlpha", "SrcAlphaSat", "Constant", "1-Constant" }) },
		                { 0x003, new CMwEnumInfo("BlendOp", new string[] { "Add", "Src-Dst", "Dst-Src", "Min", "Max" }) },
		                { 0x004, new CMwFieldInfo("BlendConstant", 0x00000028) },
		                { 0x005, new CMwEnumInfo("AlphaCmpPass", new string[] { "<", "=", "<=", ">", "!=", ">=", "Always pass" }) },
		                { 0x006, new CMwFieldInfo("AlphaRef", 0x0000001F) },
		                { 0x007, new CMwFieldInfo("BitmapApply", 0x00000006) },
		                { 0x008, new CMwFieldInfo("TexOpConstRGB", 0x00000009) },
		                { 0x009, new CMwFieldInfo("TexOpConstAlpha", 0x00000028) },
		                { 0x00A, new CMwFieldInfo("StencilToFillOnce", 0x00000001) },
		                { 0x00B, new CMwFieldInfo("AlphaToCoverage", 0x00000001) },
		                { 0x00C, new CMwFieldInfo("ForceIsAlphaBlend", 0x00000001) }
	                  })
	                },
	                { 0x027, new CMwClassInfo("CPlugVisualQuads", new Dictionary<int, CMwMemberInfo>()
	                  {
	                  })
	                },
	                { 0x028, new CMwClassInfo("CPlugVisualTriangles", new Dictionary<int, CMwMemberInfo>()
	                  {
	                  })
	                },
	                { 0x029, new CMwClassInfo("CPlugVisualHeightField", new Dictionary<int, CMwMemberInfo>()
	                  {
	                  })
	                },
	                { 0x02A, new CMwClassInfo("CPlugVisualIndexedStrip", new Dictionary<int, CMwMemberInfo>()
	                  {
	                  })
	                },
	                { 0x02B, new CMwClassInfo("CPlug", new Dictionary<int, CMwMemberInfo>()
	                  {
	                  })
	                },
	                { 0x02C, new CMwClassInfo("CPlugVisual3D", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("UseTgtU", 0x00000001) },
		                { 0x001, new CMwFieldInfo("UseTgtV", 0x00000001) },
		                { 0x002, new CMwFieldInfo("BlendShapes", 0x00000005) },
		                { 0x003, new CMwMethodInfo("NegNormals", 0x00000000, null, null) },
		                { 0x004, new CMwMethodInfo("ComputeFaceCull", 0x00000000, null, null) },
		                { 0x005, new CMwMethodInfo("ComputeOccBox", 0x00000000, null, null) }
	                  })
	                },
	                { 0x02D, new CMwClassInfo("CPlugFileFont", new Dictionary<int, CMwMemberInfo>()
	                  {
	                  })
	                },
	                { 0x02F, new CMwClassInfo("CPlugFileGen", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwEnumInfo("GenKind", new string[] { "Checker", "LightMap", "Plain", "Point", "Shade", "Render", "RenderCube", "CubeNormals", "Identity", "Pixels", "Depth", "DepthCube", "RenderF", "Iradiance", "Specular", "TestNormal", "RandNormal", "SpecularCube", "HemiReflec", "CubeInvHemiReflec", "SpecularsLA", "HueGradient", "SLGradient", "VolumeRotate", "SpecularCubeVect", "TestMipMap", "SpecCubeVectRgb", "DdxInMipMap", "RandVolume", "Unalloc", "PotentialField", "RenderCubeF", "TestAnaglyph" }) }
	                  })
	                },
	                { 0x030, new CMwClassInfo("CPlugFileSnd", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("DataSize", 0x0000001F) },
		                { 0x001, new CMwFieldInfo("Crc32", 0x0000001F) },
		                { 0x002, new CMwFieldInfo("Duration", 0x00000024) },
		                { 0x003, new CMwFieldInfo("FormatTag", 0x0000001F) },
		                { 0x004, new CMwFieldInfo("Channels", 0x0000001F) },
		                { 0x005, new CMwFieldInfo("SamplesPerSec", 0x0000001F) },
		                { 0x006, new CMwFieldInfo("AvgBytesPerSec", 0x0000001F) },
		                { 0x007, new CMwFieldInfo("BlockAlign", 0x0000001F) },
		                { 0x008, new CMwFieldInfo("BitsPerSample", 0x0000001F) },
		                { 0x009, new CMwFieldInfo("Keeper", 0x00000005) }
	                  })
	                },
	                { 0x031, new CMwClassInfo("CPlugFileWav", new Dictionary<int, CMwMemberInfo>()
	                  {
	                  })
	                },
	                { 0x032, new CMwClassInfo("CPlugFileAvi", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwEnumInfo("TimeMode", new string[] { "GameTimer", "HumanTimer", "External" }) },
		                { 0x001, new CMwMethodInfo("Rewind", 0x00000000, null, null) }
	                  })
	                },
	                { 0x035, new CMwClassInfo("CPlugFileFidContainer", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("Location", 0x00000005) },
		                { 0x001, new CMwMethodInfo("UiInstallFidsInSubFolder", 0x00000000, null, null) },
		                { 0x002, new CMwMethodInfo("UiInstallFids", 0x00000000, null, null) },
		                { 0x003, new CMwMethodInfo("UninstallFids", 0x00000000, null, null) },
		                { 0x004, new CMwMethodInfo("EdDumpStatistics", 0x00000000, null, null) },
		                { 0x005, new CMwMethodInfo("DumpContents", 0x00000000, null, null) },
		                { 0x006, new CMwFieldInfo("NbFolders", 0x0000001F) },
		                { 0x007, new CMwFieldInfo("NbFiles", 0x0000001F) }
	                  })
	                },
	                { 0x036, new CMwClassInfo("CPlugBitmapPacker", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("BitmapSizeMax", 0x0000001F) },
		                { 0x001, new CMwFieldInfo("Packs", 0x00000007) },
		                { 0x002, new CMwFieldInfo("FidsBrowseMaterials", 0x00000005) },
		                { 0x003, new CMwFieldInfo("FidsBrowseSolids", 0x00000005) },
		                { 0x004, new CMwMethodInfo("FindPackListFromPath", 0x00000000, null, null) },
		                { 0x005, new CMwMethodInfo("PackBitmaps", 0x00000000, null, null) },
		                { 0x006, new CMwMethodInfo("AddPackInput", 0x00000000, null, null) },
		                { 0x007, new CMwFieldInfo("PackInputs", 0x00000007) },
		                { 0x008, new CMwMethodInfo("FindTextureTiling", 0x00000000, null, null) },
		                { 0x009, new CMwMethodInfo("FidParametersPush", 0x00000000, null, null) }
	                  })
	                },
	                { 0x037, new CMwClassInfo("CPlugMusicType", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("NativeMusic", 0x00000005) }
	                  })
	                },
	                { 0x039, new CMwClassInfo("CPlugAudioEnvironment", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("DopplerFactor", 0x00000028) },
		                { 0x001, new CMwFieldInfo("Gain", 0x00000024) },
		                { 0x002, new CMwFieldInfo("ReflectionsGain", 0x00000024) },
		                { 0x003, new CMwFieldInfo("LateReverbGain", 0x00000024) }
	                  })
	                },
	                { 0x03A, new CMwClassInfo("CPlugMaterialCustom", new Dictionary<int, CMwMemberInfo>()
	                  {
	                  })
	                },
	                { 0x03B, new CMwClassInfo("CPlugVisualGrid", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("NbPointX", 0x0000001F) },
		                { 0x001, new CMwFieldInfo("NbPointZ", 0x0000001F) },
		                { 0x002, new CMwFieldInfo("RangeX", 0x00000024) },
		                { 0x003, new CMwFieldInfo("RangeZ", 0x00000024) },
		                { 0x004, new CMwMethodInfo("Courbificateur", 0x00000000, null, null) },
		                { 0x005, new CMwFieldInfo("Courbifiant", 0x00000024) },
		                { 0x006, new CMwFieldInfo("Courbifiant2", 0x00000024) }
	                  })
	                },
	                { 0x03C, new CMwClassInfo("CPlugVisualPath", new Dictionary<int, CMwMemberInfo>()
	                  {
	                  })
	                },
	                { 0x03D, new CMwClassInfo("CPlugFilePng", new Dictionary<int, CMwMemberInfo>()
	                  {
	                  })
	                },
	                { 0x03E, new CMwClassInfo("CPlugBlendShapes", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("BlendVals", 0x00000025) },
		                { 0x001, new CMwFieldInfo("NormalizeNormals", 0x00000001) },
		                { 0x002, new CMwFieldInfo("BlendNormals", 0x00000001) }
	                  })
	                },
	                { 0x03F, new CMwClassInfo("CPlugTreeGenText", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("Text", 0x0000002D) },
		                { 0x001, new CMwFieldInfo("Font", 0x00000005) },
		                { 0x002, new CMwFieldInfo("Color", 0x00000009) },
		                { 0x003, new CMwFieldInfo("Height", 0x00000024) },
		                { 0x004, new CMwFieldInfo("RatioXY", 0x00000024) },
		                { 0x005, new CMwEnumInfo("AlignHorizontal", new string[] { "Left", "HCenter", "Right", "None" }) },
		                { 0x006, new CMwEnumInfo("AlignVertical", new string[] { "Top", "VCenter", "Bottom", "None", "VCenter2" }) },
		                { 0x007, new CMwFieldInfo("ClipLength", 0x00000024) },
		                { 0x008, new CMwFieldInfo("MaxLine", 0x0000001F) },
		                { 0x009, new CMwFieldInfo("ClipLineMin", 0x0000001F) },
		                { 0x00A, new CMwFieldInfo("ClipLineMax", 0x0000001F) }
	                  })
	                },
	                { 0x040, new CMwClassInfo("CPlugFileGPU", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("IsGeneratedFromFixedPipe", 0x00000001) },
		                { 0x001, new CMwFieldInfo("VersionIdMin", 0x0000001F) },
		                { 0x002, new CMwFieldInfo("OpCountArith", 0x0000001F) },
		                { 0x003, new CMwFieldInfo("OpCountTexLd", 0x0000001F) }
	                  })
	                },
	                { 0x041, new CMwClassInfo("CPlugFileText", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("Text", 0x00000029) }
	                  })
	                },
	                { 0x042, new CMwClassInfo("CPlugFileVsh", new Dictionary<int, CMwMemberInfo>()
	                  {
	                  })
	                },
	                { 0x044, new CMwClassInfo("CPlugBitmapPack", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("SizeX", 0x0000001F) },
		                { 0x001, new CMwFieldInfo("SizeY", 0x0000001F) },
		                { 0x002, new CMwEnumInfo("Format", new string[] { "BGRA", "DXT1", "DXT2", "DXT3", "DXT4", "DXT5", "RGBA16", "RGBA16F", "RGBA32F" }) },
		                { 0x003, new CMwFieldInfo("NbComp", 0x0000001F) },
		                { 0x004, new CMwEnumInfo("TexAdrU", new string[] { "Wrap", "Mirror", "Clamp", "BorderSM3" }) },
		                { 0x005, new CMwFieldInfo("Bitmap", 0x00000005) },
		                { 0x006, new CMwFieldInfo("PackElems", 0x00000007) },
		                { 0x007, new CMwMethodInfo("LoadBitmap", 0x00000000, null, null) }
	                  })
	                },
	                { 0x045, new CMwClassInfo("CPlugFilePsh", new Dictionary<int, CMwMemberInfo>()
	                  {
	                  })
	                },
	                { 0x046, new CMwClassInfo("CPlugBitmapPackElem", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("FidBitmapSrc", 0x00000005) },
		                { 0x001, new CMwFieldInfo("TexelStartX", 0x0000001F) },
		                { 0x002, new CMwFieldInfo("TexelStartY", 0x0000001F) },
		                { 0x003, new CMwFieldInfo("TexelCountX", 0x0000001F) },
		                { 0x004, new CMwFieldInfo("TexelCountY", 0x0000001F) }
	                  })
	                },
	                { 0x047, new CMwClassInfo("CPlugBitmapAddress", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("ForcePassFill", 0x00000001) },
		                { 0x001, new CMwFieldInfo("UseBitmapTcScale", 0x00000001) },
		                { 0x002, new CMwEnumInfo("GenerateUV", new string[] { "NoGenerate", "CameraVertex", "WorldVertex", "WorldVertexXY", "WorldVertexXZ", "WorldVertexYZ", "CameraNormal", "WorldNormal", "CameraReflectionVector", "WorldReflectionVector", "WorldNormalNeg", "WaterReflectionVector", "Hack1Vertex", "MapTexel", "FogPlane0", "Vsk3SeaFoam", "ImageSpace", "LightDir0Reflect", "EyeNormal", "ShadowB1Pw01", "Tex3AsPosPrCamera", "FlatWaterReflect", "FlatWaterRefract", "FlatWaterFresnel" }) },
		                { 0x003, new CMwFieldInfo("UVTransfoIso3", 0x00000001) },
		                { 0x004, new CMwFieldInfo("UVTransfoMat4", 0x00000001) },
		                { 0x005, new CMwFieldInfo("AreUVProjected", 0x00000001) },
		                { 0x006, new CMwFieldInfo("UseBumpEnvScale", 0x00000001) },
		                { 0x007, new CMwFieldInfo("BumpEnvScale", 0x00000024) },
		                { 0x008, new CMwFieldInfo("TexCoordIndex", 0x0000001F) },
		                { 0x009, new CMwFieldInfo("SkipAuto_m11_01", 0x00000001) },
		                { 0x00A, new CMwFieldInfo("TransfoIso3", 0x00000017) },
		                { 0x00B, new CMwFieldInfo("TransfoMat4U", 0x00000039) },
		                { 0x00C, new CMwFieldInfo("TransfoMat4V", 0x00000039) },
		                { 0x00D, new CMwFieldInfo("TransfoMat4W", 0x00000039) }
	                  })
	                },
	                { 0x048, new CMwClassInfo("CPlugBitmapPackInput", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("LayerCount", 0x0000001F) },
		                { 0x001, new CMwFieldInfo("FidBitmaps", 0x00000007) }
	                  })
	                },
	                { 0x049, new CMwClassInfo("CPlugFileFidCache", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("FidsToCreateCacheFrom", 0x00000005) },
		                { 0x001, new CMwFieldInfo("Version", 0x0000001F) },
		                { 0x002, new CMwFieldInfo("RootEnumFullName", 0x0000002D) }
	                  })
	                },
	                { 0x04A, new CMwClassInfo("CPlugVisual2D", new Dictionary<int, CMwMemberInfo>()
	                  {
	                  })
	                },
	                { 0x04B, new CMwClassInfo("CPlugVisualQuads2D", new Dictionary<int, CMwMemberInfo>()
	                  {
	                  })
	                },
	                { 0x04D, new CMwClassInfo("CPlugFont", new Dictionary<int, CMwMemberInfo>()
	                  {
	                  })
	                },
	                { 0x04E, new CMwClassInfo("CPlugFontBitmap", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("NbPages", 0x0000001F) },
		                { 0x001, new CMwFieldInfo("PageShaders", 0x00000006) },
		                { 0x002, new CMwFieldInfo("FontHeight", 0x0000001F) },
		                { 0x003, new CMwFieldInfo("FontAscent", 0x0000001F) },
		                { 0x004, new CMwFieldInfo("BBoxCapitalMin", 0x00000031) },
		                { 0x005, new CMwFieldInfo("BBoxCapitalMax", 0x00000031) },
		                { 0x006, new CMwMethodInfo("LoadAllPages", 0x00000000, null, null) },
		                { 0x007, new CMwMethodInfo("CreateCharRemap", 0x00000000, null, null) },
		                { 0x008, new CMwFieldInfo("NbCharRemapPages", 0x0000001F) },
		                { 0x009, new CMwFieldInfo("MaterialModel", 0x00000005) },
		                { 0x00A, new CMwFieldInfo("PageTextureFids", 0x00000006) }
	                  })
	                },
	                { 0x04F, new CMwClassInfo("CPlugTree", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("Childs", 0x00000007) },
		                { 0x001, new CMwFieldInfo("IsVisible", 0x00000001) },
		                { 0x002, new CMwFieldInfo("IsCollidable", 0x00000001) },
		                { 0x003, new CMwFieldInfo("IsRooted", 0x00000001) },
		                { 0x004, new CMwFieldInfo("IsLightVolume", 0x00000001) },
		                { 0x005, new CMwFieldInfo("IsLightVolumeVisible", 0x00000001) },
		                { 0x006, new CMwFieldInfo("UseLocation", 0x00000001) },
		                { 0x007, new CMwFieldInfo("IsShadowCaster", 0x00000001) },
		                { 0x008, new CMwFieldInfo("IsFixedRatio2D", 0x00000001) },
		                { 0x009, new CMwFieldInfo("IsPickable", 0x00000001) },
		                { 0x00A, new CMwFieldInfo("IsPickableVisual", 0x00000001) },
		                { 0x00B, new CMwFieldInfo("IsPortal", 0x00000001) },
		                { 0x00C, new CMwFieldInfo("HasModel", 0x00000001) },
		                { 0x00D, new CMwFieldInfo("TestBBoxVisibility", 0x00000001) },
		                { 0x00E, new CMwFieldInfo("UseRenderBefore", 0x00000001) },
		                { 0x00F, new CMwFieldInfo("Location", 0x00000013) },
		                { 0x010, new CMwFieldInfo("Visual", 0x00000005) },
		                { 0x011, new CMwFieldInfo("SubVisualIndex1", 0x0000001F) },
		                { 0x012, new CMwFieldInfo("SubVisualIndex2", 0x0000001F) },
		                { 0x013, new CMwFieldInfo("SubVisualIndexB", 0x00000028) },
		                { 0x014, new CMwFieldInfo("SplitVisualIndex", 0x0000001F) },
		                { 0x015, new CMwFieldInfo("SplitVisualCount", 0x0000001F) },
		                { 0x016, new CMwFieldInfo("Generator", 0x00000005) },
		                { 0x017, new CMwFieldInfo("Material", 0x00000005) },
		                { 0x018, new CMwFieldInfo("Shader", 0x00000005) },
		                { 0x019, new CMwFieldInfo("Surface", 0x00000005) },
		                { 0x01A, new CMwMethodInfo("UpdateBBox", 0x00000000, null, null) },
		                { 0x01B, new CMwMethodInfo("Generate", 0x00000000, null, null) },
		                { 0x01C, new CMwFieldInfo("FuncTree", 0x00000005) },
		                { 0x01D, new CMwFieldInfo("BoundingBoxCenter", 0x00000035) },
		                { 0x01E, new CMwFieldInfo("BoundingBoxHalfDiag", 0x00000035) },
		                { 0x01F, new CMwFieldInfo("BonesToVisual", 0x00000015) }
	                  })
	                },
	                { 0x051, new CMwClassInfo("CPlugTreeGenerator", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("IsSaveGenerated", 0x00000001) },
		                { 0x001, new CMwFieldInfo("IsToKeepInSaveAsRelease", 0x00000001) }
	                  })
	                },
	                { 0x052, new CMwClassInfo("CPlugRessourceStrings", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("Strings", 0x0000002B) }
	                  })
	                },
	                { 0x055, new CMwClassInfo("CPlugFileI18n", new Dictionary<int, CMwMemberInfo>()
	                  {
	                  })
	                },
	                { 0x056, new CMwClassInfo("CPlugVertexStream", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("IsStatic", 0x00000001) },
		                { 0x001, new CMwFieldInfo("SkipVision", 0x00000001) },
		                { 0x002, new CMwFieldInfo("DirtyVision", 0x00000001) },
		                { 0x003, new CMwFieldInfo("Octree", 0x00000005) }
	                  })
	                },
	                { 0x057, new CMwClassInfo("CPlugIndexBuffer", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("IsStatic", 0x00000001) },
		                { 0x001, new CMwEnumInfo("IndexType", new string[] { "16b", "32b" }) },
		                { 0x002, new CMwFieldInfo("IndexCount", 0x0000001F) }
	                  })
	                },
	                { 0x058, new CMwClassInfo("CPlugBitmapRenderHemisphere", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("SpecularPower0", 0x00000024) },
		                { 0x001, new CMwFieldInfo("SpecularPower1", 0x00000024) },
		                { 0x002, new CMwEnumInfo("HemiLayout", new string[] { "1", "2 4 16" }) }
	                  })
	                },
	                { 0x05A, new CMwClassInfo("CPlugFileOggVorbis", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("Comment_Artist", 0x0000002D) },
		                { 0x001, new CMwFieldInfo("Comment_Album", 0x0000002D) },
		                { 0x002, new CMwFieldInfo("Comment_TrackNumber", 0x0000002D) },
		                { 0x003, new CMwFieldInfo("Comment_Title", 0x0000002D) },
		                { 0x004, new CMwFieldInfo("Comment_Date", 0x0000002D) },
		                { 0x005, new CMwFieldInfo("Comment_Genre", 0x0000002D) },
		                { 0x006, new CMwFieldInfo("Comment_Version", 0x0000002D) },
		                { 0x007, new CMwFieldInfo("Comment_Comment", 0x0000002D) },
		                { 0x008, new CMwFieldInfo("Comment_Vendor", 0x0000002D) },
		                { 0x009, new CMwFieldInfo("BitrateLower", 0x0000001F) },
		                { 0x00A, new CMwFieldInfo("BitrateNominal", 0x0000001F) },
		                { 0x00B, new CMwFieldInfo("BitrateUpper", 0x0000001F) }
	                  })
	                },
	                { 0x05B, new CMwClassInfo("CPlugBitmapRenderPortal", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("DepthFadeOutStart", 0x00000024) },
		                { 0x001, new CMwFieldInfo("DepthFadeOutEnd", 0x00000024) }
	                  })
	                },
	                { 0x05C, new CMwClassInfo("CPlugBitmapRenderPlaneR", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwEnumInfo("Plane", new string[] { "Fixed", "TreeId", "FaceAverage" }) },
		                { 0x001, new CMwFieldInfo("IsPlaneEqValid", 0x00000001) },
		                { 0x002, new CMwFieldInfo("TreeId", 0x00000029) },
		                { 0x003, new CMwFieldInfo("PlaneEq", 0x00000039) }
	                  })
	                },
	                { 0x05D, new CMwClassInfo("CPlugModelShell", new Dictionary<int, CMwMemberInfo>()
	                  {
	                  })
	                },
	                { 0x05E, new CMwClassInfo("CPlugSoundSurface", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("MaxSpeed", 0x00000024) },
		                { 0x001, new CMwFieldInfo("SmallImpactAttenuation", 0x00000024) },
		                { 0x002, new CMwFieldInfo("BigImpactAttenuation", 0x00000024) },
		                { 0x003, new CMwFieldInfo("SmallImpact", 0x00000006) },
		                { 0x004, new CMwFieldInfo("BigImpact", 0x00000006) },
		                { 0x005, new CMwFieldInfo("Texture", 0x00000006) },
		                { 0x006, new CMwFieldInfo("Skid", 0x00000006) }
	                  })
	                },
	                { 0x05F, new CMwClassInfo("CPlugFileBink", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("MustCopyAll", 0x00000001) },
		                { 0x001, new CMwFieldInfo("DebugMode", 0x00000001) },
		                { 0x002, new CMwFieldInfo("IsFrameReadyToBeRendered", 0x00000001) }
	                  })
	                },
	                { 0x060, new CMwClassInfo("CPlugFileVideo", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwEnumInfo("TimeMode", new string[] { "Game", "Human", "External" }) },
		                { 0x001, new CMwFieldInfo("IsLooping", 0x00000001) },
		                { 0x002, new CMwFieldInfo("HasSound", 0x00000001) },
		                { 0x003, new CMwMethodInfo("Play", 0x00000000, null, null) },
		                { 0x004, new CMwMethodInfo("Stop", 0x00000000, null, null) },
		                { 0x005, new CMwMethodInfo("Rewind", 0x00000000, null, null) }
	                  })
	                },
	                { 0x062, new CMwClassInfo("CPlugTreeLight", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("PlugLight", 0x00000005) },
		                { 0x001, new CMwFieldInfo("Light", 0x00000005) }
	                  })
	                },
	                { 0x064, new CMwClassInfo("CPlugSoundMulti", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("AdditionalSounds", 0x00000005) },
		                { 0x001, new CMwFieldInfo("ForceUseRandom", 0x00000001) }
	                  })
	                },
	                { 0x065, new CMwClassInfo("CPlugSoundVideo", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("Video", 0x00000005) },
		                { 0x001, new CMwFieldInfo("ImageNonVideo", 0x00000005) }
	                  })
	                },
	                { 0x066, new CMwClassInfo("CPlugPointsInSphereOpt", new Dictionary<int, CMwMemberInfo>()
	                  {
	                  })
	                },
	                { 0x067, new CMwClassInfo("CPlugShaderPass", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("iPass", 0x0000001F) },
		                { 0x001, new CMwEnumInfo("BlendSrc", new string[] { "0", "1", "SrcColor", "1-SrcColor", "SrcAlpha", "1-SrcAlpha", "DstColor", "1-DstColor", "DstAlpha", "1-DstAlpha", "SrcAlphaSat", "Constant", "1-Constant" }) },
		                { 0x002, new CMwEnumInfo("BlendDst", new string[] { "0", "1", "SrcColor", "1-SrcColor", "SrcAlpha", "1-SrcAlpha", "DstColor", "1-DstColor", "DstAlpha", "1-DstAlpha", "SrcAlphaSat", "Constant", "1-Constant" }) },
		                { 0x003, new CMwEnumInfo("CullMode", new string[] { "Default", "Inverse Culling", "DblSidedLighting" }) },
		                { 0x004, new CMwFieldInfo("IsValid", 0x00000001) },
		                { 0x005, new CMwFieldInfo("VertexShader", 0x00000005) },
		                { 0x006, new CMwFieldInfo("PixelShader", 0x00000005) }
	                  })
	                },
	                { 0x068, new CMwClassInfo("CPlugShaderSprite", new Dictionary<int, CMwMemberInfo>()
	                  {
	                  })
	                },
	                { 0x069, new CMwClassInfo("CPlugShaderSpritePath", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("FuncKeysPath", 0x00000005) }
	                  })
	                },
	                { 0x06A, new CMwClassInfo("CPlugVisualIndexed", new Dictionary<int, CMwMemberInfo>()
	                  {
	                  })
	                },
	                { 0x06B, new CMwClassInfo("CPlugTreeFrustum", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("Update", 0x00000001) },
		                { 0x001, new CMwMethodInfo("ResetGrid", 0x00000000, null, null) },
		                { 0x002, new CMwFieldInfo("FarZ", 0x00000024) },
		                { 0x003, new CMwFieldInfo("NbVisualX", 0x0000001F) },
		                { 0x004, new CMwFieldInfo("NbVisualZ", 0x0000001F) },
		                { 0x005, new CMwFieldInfo("NbPointX", 0x0000001F) },
		                { 0x006, new CMwFieldInfo("NbPointZ", 0x0000001F) }
	                  })
	                },
	                { 0x072, new CMwClassInfo("CPlugModelTree", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("Childs", 0x00000007) },
		                { 0x001, new CMwFieldInfo("LodMeshes", 0x00000007) },
		                { 0x002, new CMwFieldInfo("LodMeshesLocs", 0x00000015) },
		                { 0x003, new CMwFieldInfo("Surfaces", 0x00000007) },
		                { 0x004, new CMwFieldInfo("SurfaceLocs", 0x00000015) },
		                { 0x005, new CMwFieldInfo("RotationPivot", 0x00000035) },
		                { 0x006, new CMwFieldInfo("ScalePivot", 0x00000035) },
		                { 0x007, new CMwFieldInfo("Location", 0x00000013) },
		                { 0x008, new CMwFieldInfo("ChildGens", 0x00000007) },
		                { 0x009, new CMwFieldInfo("ChildGensLocs", 0x00000015) },
		                { 0x00A, new CMwFieldInfo("OptimIsKeepTree", 0x00000001) }
	                  })
	                },
	                { 0x073, new CMwClassInfo("CPlugModelMesh", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("VertexCount", 0x0000001F) },
		                { 0x001, new CMwFieldInfo("InfluenceCount", 0x0000001F) },
		                { 0x002, new CMwFieldInfo("BlendShapeCount", 0x0000001F) },
		                { 0x003, new CMwFieldInfo("FrameCount", 0x0000001F) },
		                { 0x004, new CMwFieldInfo("VertexInfluenceCount", 0x0000001F) },
		                { 0x005, new CMwFieldInfo("PolyCount", 0x0000001F) },
		                { 0x006, new CMwFieldInfo("TriCount", 0x0000001F) },
		                { 0x007, new CMwFieldInfo("QuadCount", 0x0000001F) },
		                { 0x008, new CMwFieldInfo("PolyIsMaterialIndex", 0x00000001) },
		                { 0x009, new CMwFieldInfo("PolyIsSmoothingGroup", 0x00000001) },
		                { 0x00A, new CMwFieldInfo("PolyIsVertexNormal", 0x00000001) },
		                { 0x00B, new CMwFieldInfo("PolyIsVertexColor", 0x00000001) },
		                { 0x00C, new CMwFieldInfo("PolyVertexUvLayerCount", 0x0000001F) },
		                { 0x00D, new CMwFieldInfo("PolyIsVertexTangent", 0x00000001) },
		                { 0x00E, new CMwFieldInfo("PolyIsDoubleSide", 0x00000001) },
		                { 0x00F, new CMwFieldInfo("SpriteCount", 0x0000001F) },
		                { 0x010, new CMwFieldInfo("SpriteIsMaterialIndex", 0x00000001) },
		                { 0x011, new CMwFieldInfo("SpriteIsDiameter", 0x00000001) },
		                { 0x012, new CMwFieldInfo("SpriteIsColor", 0x00000001) },
		                { 0x013, new CMwFieldInfo("SpriteIsRotAngle", 0x00000001) },
		                { 0x014, new CMwFieldInfo("SpriteIsXYRatio", 0x00000001) },
		                { 0x015, new CMwFieldInfo("SpriteIsTextureAtlas", 0x00000001) },
		                { 0x016, new CMwFieldInfo("Exts", 0x00000007) }
	                  })
	                },
	                { 0x074, new CMwClassInfo("CPlugFileVHlsl", new Dictionary<int, CMwMemberInfo>()
	                  {
	                  })
	                },
	                { 0x075, new CMwClassInfo("CPlugFileGPUV", new Dictionary<int, CMwMemberInfo>()
	                  {
	                  })
	                },
	                { 0x076, new CMwClassInfo("CPlugFileGPUP", new Dictionary<int, CMwMemberInfo>()
	                  {
	                  })
	                },
	                { 0x077, new CMwClassInfo("CPlugFilePHlsl", new Dictionary<int, CMwMemberInfo>()
	                  {
	                  })
	                },
	                { 0x079, new CMwClassInfo("CPlugMaterial", new Dictionary<int, CMwMemberInfo>()
	                  {
	                  })
	                },
	                { 0x07A, new CMwClassInfo("CPlugMaterialFx", new Dictionary<int, CMwMemberInfo>()
	                  {
	                  })
	                },
	                { 0x07B, new CMwClassInfo("CPlugMaterialFxFlags", new Dictionary<int, CMwMemberInfo>()
	                  {
	                  })
	                },
	                { 0x07C, new CMwClassInfo("CPlugMaterialFxFur", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("ShellCount", 0x0000001F) },
		                { 0x001, new CMwFieldInfo("ShellThick", 0x00000024) },
		                { 0x002, new CMwFieldInfo("ShellLowRGB", 0x00000009) },
		                { 0x003, new CMwFieldInfo("ShellLowAlpha", 0x00000024) },
		                { 0x004, new CMwFieldInfo("ShellHighRGB", 0x00000009) },
		                { 0x005, new CMwFieldInfo("ShellHighAlpha", 0x00000024) },
		                { 0x006, new CMwFieldInfo("FenceCount", 0x0000001F) },
		                { 0x007, new CMwFieldInfo("FenceHeight", 0x00000024) },
		                { 0x008, new CMwFieldInfo("FenceBitmap", 0x00000005) }
	                  })
	                },
	                { 0x07D, new CMwClassInfo("CPlugMaterialFxs", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("MaterialFxs", 0x00000006) }
	                  })
	                },
	                { 0x07E, new CMwClassInfo("CPlugBitmapSampler", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("IsInternal", 0x00000001) },
		                { 0x001, new CMwFieldInfo("IsSharedByDevices", 0x00000001) },
		                { 0x002, new CMwFieldInfo("UseBitmapDefaults", 0x00000001) },
		                { 0x003, new CMwFieldInfo("SynchNameWithShader", 0x00000001) },
		                { 0x004, new CMwEnumInfo("WantedFiltering", new string[] { "Point", "Bilinear", "Trilinear", "Anisotropic" }) },
		                { 0x005, new CMwEnumInfo("TexAddressU", new string[] { "Wrap", "Mirror", "Clamp", "BorderSM3" }) },
		                { 0x006, new CMwEnumInfo("TexAddressV", new string[] { "Wrap", "Mirror", "Clamp", "BorderSM3" }) },
		                { 0x007, new CMwEnumInfo("TexAddressW", new string[] { "Wrap", "Mirror", "Clamp", "BorderSM3" }) },
		                { 0x008, new CMwFieldInfo("BorderRGB", 0x00000009) },
		                { 0x009, new CMwFieldInfo("BorderAlpha", 0x00000028) },
		                { 0x00A, new CMwFieldInfo("TreeMipUseLodBias", 0x00000001) },
		                { 0x00B, new CMwFieldInfo("FixedLodBias", 0x00000024) },
		                { 0x00C, new CMwFieldInfo("TreeMipStartZ", 0x00000028) },
		                { 0x00D, new CMwFieldInfo("MaxMipLevel", 0x0000001F) },
		                { 0x00E, new CMwFieldInfo("Bitmap", 0x00000005) }
	                  })
	                },
	                { 0x080, new CMwClassInfo("CPlugBitmapShader", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("Shader", 0x00000005) },
		                { 0x001, new CMwFieldInfo("BitmapToSwap", 0x00000005) }
	                  })
	                },
	                { 0x081, new CMwClassInfo("CPlugMaterialFxDynaBump", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("IsCollidable", 0x00000001) },
		                { 0x001, new CMwFieldInfo("SpeedMaxIntens", 0x00000024) },
		                { 0x002, new CMwFieldInfo("MaxIntens", 0x00000024) },
		                { 0x003, new CMwFieldInfo("Inter1SizeX", 0x00000024) },
		                { 0x004, new CMwFieldInfo("Inter1SizeZ", 0x00000024) }
	                  })
	                },
	                { 0x082, new CMwClassInfo("CPlugMaterialFxDynaMobil", new Dictionary<int, CMwMemberInfo>()
	                  {
	                  })
	                },
	                { 0x083, new CMwClassInfo("CPlugMaterialFxGenUvProj", new Dictionary<int, CMwMemberInfo>()
	                  {
	                  })
	                },
	                { 0x084, new CMwClassInfo("CPlugFileZip", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("Dummy", 0x0000001F) }
	                  })
	                },
	                { 0x086, new CMwClassInfo("CPlugBitmapRender", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwEnumInfo("TriggerRender", new string[] { "None", "Once", "EachFrame" }) },
		                { 0x001, new CMwFieldInfo("LightOnly", 0x00000001) },
		                { 0x002, new CMwEnumInfo("RenderPath", new string[] { "Normal", "DepthAsAlpha", "GlowInput", "Opacity", "SpecIntens", "LightFromMap", "PlaneDToAlpha" }) },
		                { 0x003, new CMwEnumInfo("RenderPathFails", new string[] { "Ignore", "Hide" }) },
		                { 0x004, new CMwFieldInfo("IVIdMaskReflected", 0x00000001) },
		                { 0x005, new CMwFieldInfo("IVIdMaskReflectMirror", 0x00000001) },
		                { 0x006, new CMwFieldInfo("IVIdMaskRefracted", 0x00000001) },
		                { 0x007, new CMwFieldInfo("IVIdMaskViewDepBump", 0x00000001) },
		                { 0x008, new CMwFieldInfo("IVIdMaskViewDepOcclusion", 0x00000001) },
		                { 0x009, new CMwFieldInfo("IVIdMaskOnlyRefracted", 0x00000001) },
		                { 0x00A, new CMwFieldInfo("IVIdMaskHideWhenUnderground", 0x00000001) },
		                { 0x00B, new CMwFieldInfo("IVIdMaskHideWhenOverground", 0x00000001) },
		                { 0x00C, new CMwFieldInfo("IVIdMaskHideAlways", 0x00000001) },
		                { 0x00D, new CMwFieldInfo("IVIdMaskViewDepWindIntens", 0x00000001) },
		                { 0x00E, new CMwFieldInfo("IVIdMaskBackground", 0x00000001) },
		                { 0x00F, new CMwFieldInfo("IVIdMaskGrassRGB", 0x00000001) },
		                { 0x010, new CMwFieldInfo("IVIdMaskLightGenP", 0x00000001) },
		                { 0x011, new CMwFieldInfo("IVIdMaskVehicle", 0x00000001) },
		                { 0x012, new CMwFieldInfo("IVIdMaskHideOnlyDirect", 0x00000001) },
		                { 0x013, new CMwFieldInfo("IVIdRefReflected", 0x00000001) },
		                { 0x014, new CMwFieldInfo("IVIdRefReflectMirror", 0x00000001) },
		                { 0x015, new CMwFieldInfo("IVIdRefRefracted", 0x00000001) },
		                { 0x016, new CMwFieldInfo("IVIdRefViewDepBump", 0x00000001) },
		                { 0x017, new CMwFieldInfo("IVIdRefViewDepOcclusion", 0x00000001) },
		                { 0x018, new CMwFieldInfo("IVIdRefOnlyRefracted", 0x00000001) },
		                { 0x019, new CMwFieldInfo("IVIdRefHideWhenUnderground", 0x00000001) },
		                { 0x01A, new CMwFieldInfo("IVIdRefHideWhenOverground", 0x00000001) },
		                { 0x01B, new CMwFieldInfo("IVIdRefHideAlways", 0x00000001) },
		                { 0x01C, new CMwFieldInfo("IVIdRefViewDepWindIntens", 0x00000001) },
		                { 0x01D, new CMwFieldInfo("IVIdRefBackground", 0x00000001) },
		                { 0x01E, new CMwFieldInfo("IVIdRefGrassRGB", 0x00000001) },
		                { 0x01F, new CMwFieldInfo("IVIdRefLightGenP", 0x00000001) },
		                { 0x020, new CMwFieldInfo("IVIdRefVehicle", 0x00000001) },
		                { 0x021, new CMwFieldInfo("IVIdRefHideOnlyDirect", 0x00000001) },
		                { 0x022, new CMwFieldInfo("SVIdMaskReflected", 0x00000001) },
		                { 0x023, new CMwFieldInfo("SVIdMaskReflectMirror", 0x00000001) },
		                { 0x024, new CMwFieldInfo("SVIdMaskRefracted", 0x00000001) },
		                { 0x025, new CMwFieldInfo("SVIdMaskViewDepBump", 0x00000001) },
		                { 0x026, new CMwFieldInfo("SVIdMaskViewDepOcclusion", 0x00000001) },
		                { 0x027, new CMwFieldInfo("SVIdMaskOnlyRefracted", 0x00000001) },
		                { 0x028, new CMwFieldInfo("SVIdMaskHideWhenUnderground", 0x00000001) },
		                { 0x029, new CMwFieldInfo("SVIdMaskHideWhenOverground", 0x00000001) },
		                { 0x02A, new CMwFieldInfo("SVIdMaskHideAlways", 0x00000001) },
		                { 0x02B, new CMwFieldInfo("SVIdMaskViewDepWindIntens", 0x00000001) },
		                { 0x02C, new CMwFieldInfo("SVIdMaskBackground", 0x00000001) },
		                { 0x02D, new CMwFieldInfo("SVIdMaskGrassRGB", 0x00000001) },
		                { 0x02E, new CMwFieldInfo("SVIdMaskLightGenP", 0x00000001) },
		                { 0x02F, new CMwFieldInfo("SVIdMaskVehicle", 0x00000001) },
		                { 0x030, new CMwFieldInfo("SVIdMaskHideOnlyDirect", 0x00000001) },
		                { 0x031, new CMwFieldInfo("SVIdRefReflected", 0x00000001) },
		                { 0x032, new CMwFieldInfo("SVIdRefReflectMirror", 0x00000001) },
		                { 0x033, new CMwFieldInfo("SVIdRefRefracted", 0x00000001) },
		                { 0x034, new CMwFieldInfo("SVIdRefViewDepBump", 0x00000001) },
		                { 0x035, new CMwFieldInfo("SVIdRefViewDepOcclusion", 0x00000001) },
		                { 0x036, new CMwFieldInfo("SVIdRefOnlyRefracted", 0x00000001) },
		                { 0x037, new CMwFieldInfo("SVIdRefHideWhenUnderground", 0x00000001) },
		                { 0x038, new CMwFieldInfo("SVIdRefHideWhenOverground", 0x00000001) },
		                { 0x039, new CMwFieldInfo("SVIdRefHideAlways", 0x00000001) },
		                { 0x03A, new CMwFieldInfo("SVIdRefViewDepWindIntens", 0x00000001) },
		                { 0x03B, new CMwFieldInfo("SVIdRefBackground", 0x00000001) },
		                { 0x03C, new CMwFieldInfo("SVIdRefGrassRGB", 0x00000001) },
		                { 0x03D, new CMwFieldInfo("SVIdRefLightGenP", 0x00000001) },
		                { 0x03E, new CMwFieldInfo("SVIdRefVehicle", 0x00000001) },
		                { 0x03F, new CMwFieldInfo("SVIdRefHideOnlyDirect", 0x00000001) },
		                { 0x040, new CMwFieldInfo("CustomFog", 0x00000001) },
		                { 0x041, new CMwFieldInfo("FogCustomFarZ", 0x00000024) },
		                { 0x042, new CMwFieldInfo("TreeMipForceLowQ", 0x00000001) },
		                { 0x043, new CMwFieldInfo("RenderShadows", 0x00000001) },
		                { 0x044, new CMwFieldInfo("RenderProjectors", 0x00000001) },
		                { 0x045, new CMwFieldInfo("RenderZoneFogG", 0x00000001) },
		                { 0x046, new CMwFieldInfo("RenderLensFlares", 0x00000001) },
		                { 0x047, new CMwFieldInfo("InvertY", 0x00000001) },
		                { 0x048, new CMwFieldInfo("OnePixBorder", 0x00000001) },
		                { 0x049, new CMwFieldInfo("UseZBuffer", 0x00000001) },
		                { 0x04A, new CMwEnumInfo("TriggerClearRGBA", new string[] { "None", "Once", "EachFrame" }) },
		                { 0x04B, new CMwFieldInfo("ClearRGB", 0x00000009) },
		                { 0x04C, new CMwFieldInfo("ClearAlpha", 0x00000028) },
		                { 0x04D, new CMwFieldInfo("ClearWithFog", 0x00000001) },
		                { 0x04E, new CMwFieldInfo("WriteRed", 0x00000001) },
		                { 0x04F, new CMwFieldInfo("WriteGreen", 0x00000001) },
		                { 0x050, new CMwFieldInfo("WriteBlue", 0x00000001) },
		                { 0x051, new CMwFieldInfo("WriteAlpha", 0x00000001) },
		                { 0x052, new CMwFieldInfo("BitmapClear", 0x00000005) },
		                { 0x053, new CMwEnumInfo("BitmapClearMode", new string[] { "Fixed", "DayTimeRemapInU" }) },
		                { 0x054, new CMwFieldInfo("BitmapClearUV", 0x00000031) },
		                { 0x055, new CMwFieldInfo("BlurTexelCount", 0x0000001F) },
		                { 0x056, new CMwFieldInfo("BlurWRed", 0x00000001) },
		                { 0x057, new CMwFieldInfo("BlurWGreen", 0x00000001) },
		                { 0x058, new CMwFieldInfo("BlurWBlue", 0x00000001) },
		                { 0x059, new CMwFieldInfo("BlurWAlpha", 0x00000001) },
		                { 0x05A, new CMwFieldInfo("GutterTexelCount", 0x0000001F) },
		                { 0x05B, new CMwFieldInfo("RenderMultiLight", 0x00000001) },
		                { 0x05C, new CMwFieldInfo("UpdateForEachCamera", 0x00000001) },
		                { 0x05D, new CMwFieldInfo("RenderSub", 0x00000005) },
		                { 0x05E, new CMwMethodInfo("CleanRenderCache", 0x00000000, null, null) }
	                  })
	                },
	                { 0x087, new CMwClassInfo("CPlugBitmapRenderWater", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwEnumInfo("WaterType", new string[] { "Refraction", "Reflection" }) },
		                { 0x001, new CMwFieldInfo("FogMaxDepth", 0x00000024) },
		                { 0x002, new CMwFieldInfo("FogClampAboveDist", 0x00000024) },
		                { 0x003, new CMwFieldInfo("MaxDistPlaneToAlpha", 0x00000024) },
		                { 0x004, new CMwFieldInfo("MirrorGeom", 0x00000001) },
		                { 0x005, new CMwFieldInfo("MirrorScaleY", 0x00000028) },
		                { 0x006, new CMwFieldInfo("UseClipPlane", 0x00000001) },
		                { 0x007, new CMwFieldInfo("ClipPlaneHeight", 0x00000024) },
		                { 0x008, new CMwFieldInfo("UseFMargin", 0x00000001) },
		                { 0x009, new CMwFieldInfo("FrustumUseHorizon", 0x00000001) },
		                { 0x00A, new CMwFieldInfo("FMargin", 0x00000024) },
		                { 0x00B, new CMwFieldInfo("FHMargin", 0x00000024) },
		                { 0x00C, new CMwFieldInfo("RndLDirSpecInA", 0x00000001) },
		                { 0x00D, new CMwFieldInfo("UseMultiPlanes", 0x00000001) },
		                { 0x00E, new CMwFieldInfo("UseCameraZClip", 0x00000001) },
		                { 0x00F, new CMwFieldInfo("AddWaterFog", 0x00000001) },
		                { 0x010, new CMwFieldInfo("NoSubWaterOptim", 0x00000001) },
		                { 0x011, new CMwFieldInfo("DisableGeomOptim", 0x00000001) },
		                { 0x012, new CMwEnumInfo("ReflectNoGeom", new string[] { "ItemIsBackGnd(TMO/TMS)", "ItemFilterBackGnd(Vsk)" }) },
		                { 0x013, new CMwFieldInfo("DisableConfigQuality", 0x00000001) },
		                { 0x014, new CMwFieldInfo("HqSplitSkyOutDepth", 0x00000001) },
		                { 0x015, new CMwFieldInfo("BitmapSplitSky", 0x00000005) },
		                { 0x016, new CMwFieldInfo("BitmapDepth", 0x00000005) }
	                  })
	                },
	                { 0x088, new CMwClassInfo("CPlugBitmapRenderCubeMap", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwMethodInfo("SaveInTga", 0x00000000, null, null) },
		                { 0x001, new CMwFieldInfo("CubeFaceCount", 0x0000001F) },
		                { 0x002, new CMwFieldInfo("CenterPos", 0x00000035) },
		                { 0x003, new CMwFieldInfo("NearZ", 0x00000024) },
		                { 0x004, new CMwFieldInfo("FarZ", 0x00000024) },
		                { 0x005, new CMwFieldInfo("MinDistToUpdate", 0x00000024) },
		                { 0x006, new CMwFieldInfo("AverageReceiverCenters", 0x00000001) },
		                { 0x007, new CMwEnumInfo("Discard", new string[] { "Corpus", "Shader" }) },
		                { 0x008, new CMwFieldInfo("UseItemShaderFilter", 0x00000001) },
		                { 0x009, new CMwFieldInfo("BitmapImageSpace", 0x00000005) },
		                { 0x00A, new CMwFieldInfo("BitmapImageSpaceDistToCenter", 0x00000024) },
		                { 0x00B, new CMwFieldInfo("BitmapImageSpaceScaleHeight", 0x00000024) }
	                  })
	                },
	                { 0x089, new CMwClassInfo("CPlugBitmapRenderCamera", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("UseCameraDrawRect", 0x00000001) },
		                { 0x001, new CMwFieldInfo("UseCameraScissor", 0x00000001) },
		                { 0x002, new CMwFieldInfo("ForceAlphaToOne", 0x00000001) },
		                { 0x003, new CMwEnumInfo("CameraMode", new string[] { "Image space", "Visual space", "CameraMap" }) },
		                { 0x004, new CMwFieldInfo("CameraToVisual", 0x00000013) },
		                { 0x005, new CMwFieldInfo("ScaleZRange", 0x00000024) },
		                { 0x006, new CMwFieldInfo("Camera", 0x00000005) },
		                { 0x007, new CMwFieldInfo("DepthBias", 0x00000024) },
		                { 0x008, new CMwFieldInfo("DepthBiasSlope", 0x00000024) }
	                  })
	                },
	                { 0x08A, new CMwClassInfo("CPlugBitmapRenderVDepPlaneY", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("ViewDepLocator", 0x00000005) }
	                  })
	                },
	                { 0x08B, new CMwClassInfo("CPlugFileSndGen", new Dictionary<int, CMwMemberInfo>()
	                  {
	                  })
	                },
	                { 0x08C, new CMwClassInfo("CPlugMaterialFxGenCV", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("DeltaYMax", 0x00000024) },
		                { 0x001, new CMwFieldInfo("DeltaYMin", 0x00000024) },
		                { 0x002, new CMwFieldInfo("MaterialToRayCasts", 0x00000006) }
	                  })
	                },
	                { 0x08E, new CMwClassInfo("CPlugSoundEngine", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("Components", 0x00000006) },
		                { 0x001, new CMwFieldInfo("MaxRpm", 0x00000024) },
		                { 0x002, new CMwFieldInfo("Volume_Speed", 0x00000005) },
		                { 0x003, new CMwFieldInfo("Volume_Distance", 0x00000005) },
		                { 0x004, new CMwFieldInfo("Volume_Rpm", 0x00000005) },
		                { 0x005, new CMwFieldInfo("Volume_Accel", 0x00000005) },
		                { 0x006, new CMwFieldInfo("Alpha_Speed", 0x00000005) },
		                { 0x007, new CMwFieldInfo("Alpha_Distance", 0x00000005) },
		                { 0x008, new CMwFieldInfo("Alpha_Rpm", 0x00000005) },
		                { 0x009, new CMwFieldInfo("Alpha_Accel", 0x00000005) }
	                  })
	                },
	                { 0x08F, new CMwClassInfo("CPlugSoundEngineComponent", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("PlugFile", 0x00000005) },
		                { 0x001, new CMwFieldInfo("MinVolume", 0x00000024) },
		                { 0x002, new CMwFieldInfo("MaxVolume", 0x00000024) },
		                { 0x003, new CMwFieldInfo("FadeInStartRpm", 0x00000024) },
		                { 0x004, new CMwFieldInfo("FadeInEndRpm", 0x00000024) },
		                { 0x005, new CMwFieldInfo("FadeOutStartRpm", 0x00000024) },
		                { 0x006, new CMwFieldInfo("FadeOutEndRpm", 0x00000024) },
		                { 0x007, new CMwFieldInfo("MinPitch", 0x00000024) },
		                { 0x008, new CMwFieldInfo("MaxPitch", 0x00000024) },
		                { 0x009, new CMwFieldInfo("PitchShiftStartRpm", 0x00000024) },
		                { 0x00A, new CMwFieldInfo("PitchShiftEndRpm", 0x00000024) }
	                  })
	                },
	                { 0x090, new CMwClassInfo("CPlugBitmapRenderSolid", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwEnumInfo("TriggerBitmap", new string[] { "None", "Once", "EachFrame" }) },
		                { 0x001, new CMwEnumInfo("TriggerShader", new string[] { "None", "Once", "EachFrame" }) },
		                { 0x002, new CMwEnumInfo("TriggerSolid", new string[] { "None", "Once", "EachFrame" }) },
		                { 0x003, new CMwFieldInfo("Bitmap", 0x00000005) },
		                { 0x004, new CMwFieldInfo("Shader", 0x00000005) },
		                { 0x005, new CMwFieldInfo("Solids", 0x00000007) },
		                { 0x006, new CMwFieldInfo("Locations", 0x00000015) }
	                  })
	                },
	                { 0x091, new CMwClassInfo("CPlugBitmapRenderSub", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("ShaderToForce", 0x00000005) }
	                  })
	                },
	                { 0x092, new CMwClassInfo("CPlugModel", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("Origin", 0x00000029) },
		                { 0x001, new CMwFieldInfo("ExportScale", 0x00000024) },
		                { 0x002, new CMwFieldInfo("ModelTree", 0x00000005) },
		                { 0x003, new CMwFieldInfo("VertexPositionsQuantize", 0x00000024) }
	                  })
	                },
	                { 0x093, new CMwClassInfo("CPlugFileGpuFx", new Dictionary<int, CMwMemberInfo>()
	                  {
	                  })
	                },
	                { 0x094, new CMwClassInfo("CPlugFileGpuFxD3d", new Dictionary<int, CMwMemberInfo>()
	                  {
	                  })
	                },
	                { 0x095, new CMwClassInfo("CPlugFileVso", new Dictionary<int, CMwMemberInfo>()
	                  {
	                  })
	                },
	                { 0x096, new CMwClassInfo("CPlugFilePso", new Dictionary<int, CMwMemberInfo>()
	                  {
	                  })
	                },
	                { 0x098, new CMwClassInfo("CPlugFileModel", new Dictionary<int, CMwMemberInfo>()
	                  {
	                  })
	                },
	                { 0x099, new CMwClassInfo("CPlugFileModelObj", new Dictionary<int, CMwMemberInfo>()
	                  {
	                  })
	                },
	                { 0x09A, new CMwClassInfo("CPlugTreeGenSolid", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("MergeInstances", 0x00000001) },
		                { 0x001, new CMwFieldInfo("Solid", 0x00000005) },
		                { 0x002, new CMwFieldInfo("SolidReplaceMaterial", 0x00000005) },
		                { 0x003, new CMwFieldInfo("UseCustomFuncTree", 0x00000001) },
		                { 0x004, new CMwFieldInfo("CustomFuncTreePhase", 0x00000024) },
		                { 0x005, new CMwFieldInfo("CustomFuncTreePeriodScale", 0x00000024) }
	                  })
	                },
	                { 0x09B, new CMwClassInfo("CPlugFileModel3ds", new Dictionary<int, CMwMemberInfo>()
	                  {
	                  })
	                },
	                { 0x09C, new CMwClassInfo("CPlugModelLodMesh", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("OptimSplitGridIsUse", 0x00000001) },
		                { 0x001, new CMwFieldInfo("OptimSplitGridCellSize", 0x00000035) },
		                { 0x002, new CMwFieldInfo("OptimSplitGridOrigin", 0x00000035) },
		                { 0x003, new CMwFieldInfo("OptimGroupId", 0x00000029) },
		                { 0x004, new CMwFieldInfo("OptimIsCreateSubInfluences", 0x00000001) }
	                  })
	                },
	                { 0x09D, new CMwClassInfo("CPlugModelFur", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("RandSeed", 0x0000001F) },
		                { 0x001, new CMwFieldInfo("Enabled", 0x00000001) },
		                { 0x002, new CMwFieldInfo("MaterialName", 0x00000029) },
		                { 0x003, new CMwFieldInfo("Material", 0x00000005) },
		                { 0x004, new CMwFieldInfo("DiffuseMap", 0x00000005) },
		                { 0x005, new CMwFieldInfo("SpecularMap", 0x00000005) },
		                { 0x006, new CMwFieldInfo("DensityMap", 0x00000005) },
		                { 0x007, new CMwFieldInfo("DirAlphaMap", 0x00000005) },
		                { 0x008, new CMwFieldInfo("DirBetaMap", 0x00000005) },
		                { 0x009, new CMwFieldInfo("WidthMap", 0x00000005) },
		                { 0x00A, new CMwFieldInfo("LengthMap", 0x00000005) },
		                { 0x00B, new CMwFieldInfo("CurvatureMap", 0x00000005) },
		                { 0x00C, new CMwFieldInfo("MapAtlasX", 0x0000001F) },
		                { 0x00D, new CMwFieldInfo("MapAtlasY", 0x0000001F) },
		                { 0x00E, new CMwFieldInfo("RandomUDir", 0x00000001) },
		                { 0x00F, new CMwFieldInfo("DirAlphaDeg", 0x00000024) },
		                { 0x010, new CMwFieldInfo("DirAlphaVarDeg", 0x00000024) },
		                { 0x011, new CMwFieldInfo("DirBetaDeg", 0x00000024) },
		                { 0x012, new CMwFieldInfo("DirBetaVarDeg", 0x00000024) },
		                { 0x013, new CMwFieldInfo("CurvatureDeg", 0x00000024) },
		                { 0x014, new CMwFieldInfo("CurvatureVarDeg", 0x00000024) },
		                { 0x015, new CMwFieldInfo("HelixDeg", 0x00000024) },
		                { 0x016, new CMwFieldInfo("HelixVarDeg", 0x00000024) },
		                { 0x017, new CMwFieldInfo("Width", 0x00000024) },
		                { 0x018, new CMwFieldInfo("WidthVar", 0x00000028) },
		                { 0x019, new CMwFieldInfo("Length", 0x00000024) },
		                { 0x01A, new CMwFieldInfo("LengthVar", 0x00000028) },
		                { 0x01B, new CMwFieldInfo("NormalBendAngleX", 0x00000024) },
		                { 0x01C, new CMwFieldInfo("NormalBendAngleY", 0x00000024) },
		                { 0x01D, new CMwFieldInfo("Debug", 0x00000001) },
		                { 0x01E, new CMwFieldInfo("DoubleLayer", 0x00000001) },
		                { 0x01F, new CMwFieldInfo("ModulateWidthByTriSurf", 0x00000001) },
		                { 0x020, new CMwFieldInfo("DensityMax", 0x0000001F) },
		                { 0x021, new CMwFieldInfo("DensityInAtlasY", 0x00000001) },
		                { 0x022, new CMwFieldInfo("DensityRandom", 0x00000024) },
		                { 0x023, new CMwFieldInfo("DensitySampling", 0x0000001F) },
		                { 0x024, new CMwFieldInfo("FluffPosRandom", 0x00000024) },
		                { 0x025, new CMwFieldInfo("DynaEnabled", 0x00000001) },
		                { 0x026, new CMwFieldInfo("DynaK1", 0x00000024) },
		                { 0x027, new CMwFieldInfo("DynaK1Var", 0x00000024) },
		                { 0x028, new CMwFieldInfo("DynaK2", 0x00000024) },
		                { 0x029, new CMwFieldInfo("DynaK2Var", 0x00000024) },
		                { 0x02A, new CMwFieldInfo("DynaAccel", 0x00000024) },
		                { 0x02B, new CMwFieldInfo("DynaAccelVar", 0x00000024) },
		                { 0x02C, new CMwFieldInfo("DynaZeroBeta", 0x00000001) },
		                { 0x02D, new CMwFieldInfo("DynaAlphaMin", 0x00000024) },
		                { 0x02E, new CMwFieldInfo("DynaAlphaMax", 0x00000024) },
		                { 0x02F, new CMwFieldInfo("DynaBetaMin", 0x00000024) },
		                { 0x030, new CMwFieldInfo("DynaBetaMax", 0x00000024) },
		                { 0x031, new CMwFieldInfo("FluffChunkCount", 0x0000001F) },
		                { 0x032, new CMwFieldInfo("ChunkVertCount", 0x0000001F) },
		                { 0x033, new CMwFieldInfo("FluffShapeCircular", 0x00000001) },
		                { 0x034, new CMwFieldInfo("FluffShapeEndWidthCoef", 0x00000024) },
		                { 0x035, new CMwFieldInfo("HairMaxCount", 0x0000001F) }
	                  })
	                },
	                { 0x09E, new CMwClassInfo("CPlugBitmapRenderOverlay", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("Overlays", 0x00000007) }
	                  })
	                },
	                { 0x09F, new CMwClassInfo("CPlugBitmapRenderLightOcc", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("FovY", 0x00000024) },
		                { 0x001, new CMwFieldInfo("Opacity", 0x00000028) },
		                { 0x002, new CMwFieldInfo("FlareThreshold", 0x00000028) },
		                { 0x003, new CMwFieldInfo("BitmapToModulate", 0x00000005) }
	                  })
	                },
	                { 0x0A0, new CMwClassInfo("CPlugViewDepLocator", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("MinY", 0x00000024) },
		                { 0x001, new CMwFieldInfo("MaxY", 0x00000024) },
		                { 0x002, new CMwFieldInfo("UseWaterY", 0x00000001) }
	                  })
	                },
	                { 0x0A1, new CMwClassInfo("CPlugTreeViewDep", new Dictionary<int, CMwMemberInfo>()
	                  {
	                  })
	                },
	                { 0x0A2, new CMwClassInfo("CPlugDecoratorTree", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("TreeId", 0x00000029) },
		                { 0x001, new CMwFieldInfo("Material", 0x00000005) },
		                { 0x002, new CMwFieldInfo("TreeLight", 0x00000005) },
		                { 0x003, new CMwEnumInfo("ExistCond", new string[] { "Never", "LowOnly", "LowAndMedium", "MediumOnly", "MediumAndHigh", "HighOnly", "Always" }) },
		                { 0x004, new CMwEnumInfo("VisibleCond", new string[] { "Never", "LowOnly", "LowAndMedium", "MediumOnly", "MediumAndHigh", "HighOnly", "Always" }) },
		                { 0x005, new CMwFieldInfo("VisibleApplyOnChilds", 0x00000001) },
		                { 0x006, new CMwEnumInfo("ShadowCasterCond", new string[] { "Never", "LowOnly", "LowAndMedium", "MediumOnly", "MediumAndHigh", "HighOnly", "Always" }) },
		                { 0x007, new CMwFieldInfo("ShadowCasterApplyOnChilds", 0x00000001) },
		                { 0x008, new CMwFieldInfo("TransformVisualToSurface", 0x00000001) },
		                { 0x009, new CMwEnumInfo("CollidableCond", new string[] { "Never", "LowOnly", "LowAndMedium", "MediumOnly", "MediumAndHigh", "HighOnly", "Always" }) },
		                { 0x00A, new CMwFieldInfo("NoLocation", 0x00000001) }
	                  })
	                },
	                { 0x0A3, new CMwClassInfo("CPlugDecoratorSolid", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("TreeDecorators", 0x00000007) }
	                  })
	                },
	                { 0x0A4, new CMwClassInfo("CPlugModelFences", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("BlockSizeXZ", 0x00000024) },
		                { 0x001, new CMwFieldInfo("BlockFenceCountXZ", 0x0000001F) },
		                { 0x002, new CMwFieldInfo("FenceRangeY", 0x00000031) },
		                { 0x003, new CMwFieldInfo("LodDist", 0x00000024) },
		                { 0x004, new CMwFieldInfo("RandSeed", 0x0000001F) },
		                { 0x005, new CMwFieldInfo("MaterialFences", 0x00000005) },
		                { 0x006, new CMwFieldInfo("IsOrthos", 0x00000001) },
		                { 0x007, new CMwFieldInfo("IsDiags", 0x00000001) },
		                { 0x008, new CMwFieldInfo("OnlyOnePlaneY", 0x00000001) },
		                { 0x009, new CMwFieldInfo("UseSkinShader", 0x00000001) },
		                { 0x00A, new CMwFieldInfo("Debug", 0x00000001) },
		                { 0x00B, new CMwFieldInfo("DebugRand", 0x00000001) },
		                { 0x00C, new CMwFieldInfo("DebugEdges", 0x00000001) }
	                  })
	                },
	                { 0x0A6, new CMwClassInfo("CPlugFurWind", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("WorldDir", 0x00000035) },
		                { 0x001, new CMwFieldInfo("Intensity", 0x00000024) },
		                { 0x002, new CMwFieldInfo("NoiseFunc", 0x00000005) }
	                  })
	                }
                  })
                },
                { 0x0A, new CMwEngineInfo("Scene", new Dictionary<int, CMwClassInfo>()
                  {
	                { 0x000, new CMwClassInfo("CSceneEngine", new Dictionary<int, CMwMemberInfo>()
	                  {
	                  })
	                },
	                { 0x001, new CMwClassInfo("CScene", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("Mobils", 0x00000007) },
		                { 0x001, new CMwFieldInfo("Cameras", 0x00000007) },
		                { 0x002, new CMwFieldInfo("SceneConfig", 0x00000005) },
		                { 0x003, new CMwFieldInfo("MotionManagers", 0x00000007) },
		                { 0x004, new CMwFieldInfo("MotionManagerModels", 0x00000007) },
		                { 0x005, new CMwMethodInfo("LogSceneStats", 0x00000000, null, null) }
	                  })
	                },
	                { 0x002, new CMwClassInfo("CScene2d", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("Sector", 0x00000005) },
		                { 0x001, new CMwFieldInfo("OverlayMin", 0x00000031) },
		                { 0x002, new CMwFieldInfo("OverlayMax", 0x00000031) },
		                { 0x003, new CMwFieldInfo("Lights", 0x00000007) },
		                { 0x004, new CMwFieldInfo("Sounds", 0x00000007) }
	                  })
	                },
	                { 0x003, new CMwClassInfo("CScene3d", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("Sectors", 0x00000007) },
		                { 0x001, new CMwFieldInfo("Lights", 0x00000007) },
		                { 0x002, new CMwFieldInfo("Gates", 0x00000007) },
		                { 0x003, new CMwFieldInfo("Locations", 0x00000007) },
		                { 0x004, new CMwFieldInfo("Paths", 0x00000007) },
		                { 0x005, new CMwFieldInfo("Sounds", 0x00000007) },
		                { 0x006, new CMwFieldInfo("Fields", 0x00000007) },
		                { 0x007, new CMwFieldInfo("TrafficGraph", 0x00000005) },
		                { 0x008, new CMwFieldInfo("TrafficPaths", 0x00000007) },
		                { 0x009, new CMwFieldInfo("CameraFarZ", 0x00000024) },
		                { 0x00A, new CMwFieldInfo("CameraClearColor", 0x00000009) },
		                { 0x00B, new CMwFieldInfo("SceneFxNod", 0x00000005) }
	                  })
	                },
	                { 0x004, new CMwClassInfo("CSceneSector", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("Zone", 0x00000005) }
	                  })
	                },
	                { 0x005, new CMwClassInfo("CSceneObject", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("Scene", 0x00000005) },
		                { 0x001, new CMwMethodInfo("SetLocation", 0x00000041,
			                new uint[] { 0x00000013, 0x0A004000 },
			                new string[] { "Location", "Sector" })
		                },
		                { 0x002, new CMwFieldInfo("Motion", 0x00000005) }
	                  })
	                },
	                { 0x006, new CMwClassInfo("CSceneGate", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("FirstPortal", 0x00000005) },
		                { 0x001, new CMwFieldInfo("SecondPortal", 0x00000005) }
	                  })
	                },
	                { 0x007, new CMwClassInfo("CSceneLocation", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("Location", 0x00000013) }
	                  })
	                },
	                { 0x008, new CMwClassInfo("CScenePath", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("Paths", 0x00000006) },
		                { 0x001, new CMwFieldInfo("Sectors", 0x00000006) }
	                  })
	                },
	                { 0x009, new CMwClassInfo("CScenePoc", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("HmsPoc", 0x00000005) },
		                { 0x001, new CMwFieldInfo("IsActive", 0x00000001) }
	                  })
	                },
	                { 0x00A, new CMwClassInfo("CSceneCamera", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("Overlays", 0x00000006) },
		                { 0x001, new CMwFieldInfo("iPrecalcRender", 0x0000001F) },
		                { 0x002, new CMwFieldInfo("cPrecalcRender", 0x0000001F) },
		                { 0x003, new CMwFieldInfo("Listener", 0x00000005) }
	                  })
	                },
	                { 0x00B, new CMwClassInfo("CSceneLight", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("Light", 0x00000005) }
	                  })
	                },
	                { 0x00C, new CMwClassInfo("CSceneController", new Dictionary<int, CMwMemberInfo>()
	                  {
	                  })
	                },
	                { 0x00D, new CMwClassInfo("CSceneListener", new Dictionary<int, CMwMemberInfo>()
	                  {
	                  })
	                },
	                { 0x00E, new CMwClassInfo("CSceneSoundSource", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwMethodInfo("Play", 0x00000000, null, null) },
		                { 0x001, new CMwMethodInfo("Stop", 0x00000000, null, null) },
		                { 0x002, new CMwFieldInfo("PlugSound", 0x00000005) }
	                  })
	                },
	                { 0x010, new CMwClassInfo("", new Dictionary<int, CMwMemberInfo>()
	                  {
	                  })
	                },
	                { 0x011, new CMwClassInfo("CSceneMobil", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("Model", 0x00000005) },
		                { 0x001, new CMwFieldInfo("Zombie", 0x00000001) },
		                { 0x002, new CMwFieldInfo("Item", 0x00000005) },
		                { 0x003, new CMwFieldInfo("Solid", 0x00000005) },
		                { 0x004, new CMwFieldInfo("SolidModel", 0x00000005) },
		                { 0x005, new CMwMethodInfo("Show", 0x00000000, null, null) },
		                { 0x006, new CMwMethodInfo("Hide", 0x00000000, null, null) },
		                { 0x007, new CMwFieldInfo("LinkedObject", 0x00000005) },
		                { 0x008, new CMwFieldInfo("Links", 0x00000007) },
		                { 0x009, new CMwFieldInfo("MotionSolid", 0x00000005) },
		                { 0x00A, new CMwFieldInfo("MessageHandler", 0x00000005) },
		                { 0x00B, new CMwFieldInfo("CastedShadows", 0x0000001F) },
		                { 0x00C, new CMwFieldInfo("SelfShadow", 0x00000001) },
		                { 0x00D, new CMwMethodInfo("SolidObjectsRefresh", 0x00000000, null, null) }
	                  })
	                },
	                { 0x012, new CMwClassInfo("CSceneToy", new Dictionary<int, CMwMemberInfo>()
	                  {
	                  })
	                },
	                { 0x014, new CMwClassInfo("CSceneObjectLink", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("IsToSave", 0x00000001) },
		                { 0x001, new CMwFieldInfo("IsFromModel", 0x00000001) },
		                { 0x002, new CMwFieldInfo("UseOnlyMobilPosition", 0x00000001) },
		                { 0x003, new CMwFieldInfo("UseOnlyMobilTreePosition", 0x00000001) },
		                { 0x004, new CMwFieldInfo("RelativeLocation", 0x00000013) },
		                { 0x005, new CMwFieldInfo("Object", 0x00000005) },
		                { 0x006, new CMwFieldInfo("Mobil", 0x00000005) },
		                { 0x007, new CMwFieldInfo("MobilTreeId", 0x00000029) },
		                { 0x008, new CMwFieldInfo("IsActive2", 0x00000001) },
		                { 0x009, new CMwFieldInfo("IsDynamic", 0x00000001) }
	                  })
	                },
	                { 0x015, new CMwClassInfo("", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("", 0x00000021) }
	                  })
	                },
	                { 0x016, new CMwClassInfo("CSceneToyCharacterDesc", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("VisualAnims", 0x00000005) },
		                { 0x001, new CMwFieldInfo("TreeToAnimateId", 0x0000001B) }
	                  })
	                },
	                { 0x017, new CMwClassInfo("CScenePickerManager", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("Picker", 0x00000005) }
	                  })
	                },
	                { 0x01D, new CMwClassInfo("CPlugBitmapRenderScene3d", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("Scene3d", 0x00000005) },
		                { 0x001, new CMwFieldInfo("Location", 0x00000013) },
		                { 0x002, new CMwMethodInfo("CreateCamera", 0x00000000, null, null) },
		                { 0x003, new CMwMethodInfo("CreateOverlayCameraSettings", 0x00000000, null, null) }
	                  })
	                },
	                { 0x01E, new CMwClassInfo("CSceneSoundManager", new Dictionary<int, CMwMemberInfo>()
	                  {
	                  })
	                },
	                { 0x01F, new CMwClassInfo("CSceneMessageHandler", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("OnClickScript", 0x00000005) },
		                { 0x001, new CMwFieldInfo("OnContactScript", 0x00000005) },
		                { 0x002, new CMwFieldInfo("Mobil", 0x00000005) },
		                { 0x003, new CMwFieldInfo("Nod", 0x00000005) },
		                { 0x004, new CMwFieldInfo("Now", 0x0000001F) },
		                { 0x005, new CMwFieldInfo("ContactCorpus", 0x00000005) },
		                { 0x006, new CMwFieldInfo("ContactMobil", 0x00000005) },
		                { 0x007, new CMwFieldInfo("ContactNormal", 0x00000035) },
		                { 0x008, new CMwFieldInfo("ContactPosition", 0x00000035) }
	                  })
	                },
	                { 0x029, new CMwClassInfo("", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("", 0x00000024) },
		                { 0x001, new CMwFieldInfo("", 0x00000024) },
		                { 0x002, new CMwFieldInfo("", 0x00000005) },
		                { 0x003, new CMwFieldInfo("", 0x00000005) },
		                { 0x004, new CMwFieldInfo("", 0x00000024) },
		                { 0x005, new CMwFieldInfo("", 0x0000001F) },
		                { 0x006, new CMwFieldInfo("", 0x00000024) },
		                { 0x007, new CMwFieldInfo("", 0x00000024) },
		                { 0x008, new CMwFieldInfo("", 0x00000024) },
		                { 0x009, new CMwFieldInfo("", 0x00000024) },
		                { 0x00A, new CMwFieldInfo("", 0x00000024) },
		                { 0x00B, new CMwFieldInfo("", 0x00000024) },
		                { 0x00C, new CMwFieldInfo("", 0x00000024) },
		                { 0x00D, new CMwFieldInfo("", 0x00000005) },
		                { 0x00E, new CMwFieldInfo("", 0x0000001F) },
		                { 0x00F, new CMwFieldInfo("", 0x00000024) },
		                { 0x010, new CMwFieldInfo("", 0x00000024) },
		                { 0x011, new CMwFieldInfo("", 0x00000024) },
		                { 0x012, new CMwFieldInfo("", 0x00000024) },
		                { 0x013, new CMwFieldInfo("", 0x0000001F) },
		                { 0x014, new CMwFieldInfo("", 0x00000001) },
		                { 0x015, new CMwFieldInfo("", 0x0000001F) },
		                { 0x016, new CMwFieldInfo("", 0x00000028) },
		                { 0x017, new CMwFieldInfo("", 0x00000005) },
		                { 0x018, new CMwFieldInfo("", 0x00000024) },
		                { 0x019, new CMwFieldInfo("", 0x0000001F) },
		                { 0x01A, new CMwFieldInfo("", 0x0000001F) },
		                { 0x01B, new CMwFieldInfo("", 0x0000001F) },
		                { 0x01C, new CMwFieldInfo("", 0x00000024) },
		                { 0x01D, new CMwFieldInfo("", 0x00000024) },
		                { 0x01E, new CMwFieldInfo("", 0x00000024) },
		                { 0x01F, new CMwFieldInfo("", 0x00000028) },
		                { 0x020, new CMwFieldInfo("", 0x00000005) },
		                { 0x021, new CMwFieldInfo("", 0x00000024) },
		                { 0x022, new CMwFieldInfo("", 0x00000024) },
		                { 0x023, new CMwFieldInfo("", 0x00000005) },
		                { 0x024, new CMwFieldInfo("", 0x00000028) },
		                { 0x025, new CMwFieldInfo("", 0x00000028) },
		                { 0x026, new CMwFieldInfo("", 0x00000005) },
		                { 0x027, new CMwFieldInfo("", 0x00000005) },
		                { 0x028, new CMwFieldInfo("", 0x00000024) },
		                { 0x029, new CMwFieldInfo("", 0x00000024) },
		                { 0x02A, new CMwFieldInfo("", 0x00000028) },
		                { 0x02B, new CMwFieldInfo("", 0x00000028) },
		                { 0x02C, new CMwFieldInfo("", 0x00000028) },
		                { 0x02D, new CMwFieldInfo("", 0x00000028) },
		                { 0x02E, new CMwFieldInfo("", 0x0000001F) },
		                { 0x02F, new CMwFieldInfo("", 0x0000001F) },
		                { 0x030, new CMwFieldInfo("", 0x00000028) },
		                { 0x031, new CMwFieldInfo("", 0x00000024) },
		                { 0x032, new CMwFieldInfo("", 0x0000001F) },
		                { 0x033, new CMwFieldInfo("", 0x00000024) },
		                { 0x034, new CMwFieldInfo("", 0x0000001F) },
		                { 0x035, new CMwFieldInfo("", 0x00000024) },
		                { 0x036, new CMwFieldInfo("", 0x0000001F) },
		                { 0x037, new CMwFieldInfo("", 0x00000005) },
		                { 0x038, new CMwFieldInfo("", 0x00000024) },
		                { 0x039, new CMwFieldInfo("", 0x00000024) },
		                { 0x03A, new CMwFieldInfo("", 0x00000024) },
		                { 0x03B, new CMwFieldInfo("", 0x00000024) },
		                { 0x03C, new CMwFieldInfo("", 0x00000024) },
		                { 0x03D, new CMwFieldInfo("", 0x00000024) },
		                { 0x03E, new CMwFieldInfo("", 0x00000005) },
		                { 0x03F, new CMwFieldInfo("", 0x00000005) },
		                { 0x040, new CMwFieldInfo("", 0x00000005) },
		                { 0x041, new CMwFieldInfo("", 0x00000024) },
		                { 0x042, new CMwFieldInfo("", 0x00000024) },
		                { 0x043, new CMwFieldInfo("", 0x00000024) },
		                { 0x044, new CMwFieldInfo("", 0x00000024) },
		                { 0x045, new CMwFieldInfo("", 0x00000035) },
		                { 0x046, new CMwFieldInfo("", 0x00000028) },
		                { 0x047, new CMwFieldInfo("", 0x00000028) },
		                { 0x048, new CMwFieldInfo("", 0x00000028) },
		                { 0x049, new CMwFieldInfo("", 0x00000028) },
		                { 0x04A, new CMwFieldInfo("", 0x00000024) },
		                { 0x04B, new CMwFieldInfo("", 0x00000024) },
		                { 0x04C, new CMwFieldInfo("", 0x00000024) },
		                { 0x04D, new CMwFieldInfo("", 0x00000024) },
		                { 0x04E, new CMwFieldInfo("", 0x00000024) },
		                { 0x04F, new CMwFieldInfo("", 0x00000024) },
		                { 0x050, new CMwFieldInfo("", 0x00000024) },
		                { 0x051, new CMwFieldInfo("", 0x00000024) },
		                { 0x052, new CMwFieldInfo("", 0x00000024) },
		                { 0x053, new CMwFieldInfo("", 0x00000024) },
		                { 0x054, new CMwFieldInfo("", 0x00000024) },
		                { 0x055, new CMwFieldInfo("", 0x00000024) },
		                { 0x056, new CMwFieldInfo("", 0x00000024) },
		                { 0x057, new CMwFieldInfo("", 0x00000024) },
		                { 0x058, new CMwFieldInfo("", 0x00000024) },
		                { 0x059, new CMwFieldInfo("", 0x00000024) },
		                { 0x05A, new CMwFieldInfo("", 0x00000024) },
		                { 0x05B, new CMwFieldInfo("", 0x00000024) },
		                { 0x05C, new CMwFieldInfo("", 0x00000024) },
		                { 0x05D, new CMwFieldInfo("", 0x00000024) },
		                { 0x05E, new CMwFieldInfo("", 0x00000024) },
		                { 0x05F, new CMwFieldInfo("", 0x00000024) },
		                { 0x060, new CMwFieldInfo("", 0x00000024) },
		                { 0x061, new CMwFieldInfo("", 0x00000024) },
		                { 0x062, new CMwFieldInfo("", 0x00000001) },
		                { 0x063, new CMwEnumInfo("", new string[] { "Demo01", "Demo02", "Demo03" }) },
		                { 0x064, new CMwFieldInfo("", 0x0000001F) },
		                { 0x065, new CMwEnumInfo("", new string[] { "Steer01", "Steer02", "Steer03", "Steer04", "Steer05", "Steer06" }) },
		                { 0x066, new CMwFieldInfo("", 0x00000024) },
		                { 0x067, new CMwFieldInfo("", 0x00000024) },
		                { 0x068, new CMwFieldInfo("", 0x00000024) },
		                { 0x069, new CMwFieldInfo("", 0x00000024) },
		                { 0x06A, new CMwFieldInfo("", 0x00000005) },
		                { 0x06B, new CMwFieldInfo("", 0x00000024) },
		                { 0x06C, new CMwFieldInfo("", 0x00000005) },
		                { 0x06D, new CMwFieldInfo("", 0x00000024) },
		                { 0x06E, new CMwFieldInfo("", 0x00000028) },
		                { 0x06F, new CMwFieldInfo("", 0x00000028) },
		                { 0x070, new CMwFieldInfo("", 0x00000028) },
		                { 0x071, new CMwFieldInfo("", 0x00000028) },
		                { 0x072, new CMwFieldInfo("", 0x00000024) },
		                { 0x073, new CMwFieldInfo("", 0x00000024) },
		                { 0x074, new CMwFieldInfo("", 0x00000024) },
		                { 0x075, new CMwFieldInfo("", 0x00000024) },
		                { 0x076, new CMwFieldInfo("", 0x00000024) },
		                { 0x077, new CMwFieldInfo("", 0x00000024) },
		                { 0x078, new CMwFieldInfo("", 0x00000024) },
		                { 0x079, new CMwFieldInfo("", 0x00000024) },
		                { 0x07A, new CMwFieldInfo("", 0x00000024) },
		                { 0x07B, new CMwFieldInfo("", 0x00000024) },
		                { 0x07C, new CMwFieldInfo("", 0x00000024) },
		                { 0x07D, new CMwFieldInfo("", 0x00000005) },
		                { 0x07E, new CMwFieldInfo("", 0x00000028) },
		                { 0x07F, new CMwFieldInfo("", 0x00000005) },
		                { 0x080, new CMwFieldInfo("", 0x00000028) },
		                { 0x081, new CMwFieldInfo("", 0x00000005) },
		                { 0x082, new CMwFieldInfo("", 0x00000028) },
		                { 0x083, new CMwFieldInfo("", 0x00000024) },
		                { 0x084, new CMwFieldInfo("", 0x00000024) },
		                { 0x085, new CMwFieldInfo("", 0x00000024) },
		                { 0x086, new CMwFieldInfo("", 0x00000024) },
		                { 0x087, new CMwFieldInfo("", 0x00000024) },
		                { 0x088, new CMwFieldInfo("", 0x00000005) },
		                { 0x089, new CMwFieldInfo("", 0x00000024) },
		                { 0x08A, new CMwFieldInfo("", 0x00000005) },
		                { 0x08B, new CMwFieldInfo("", 0x00000024) },
		                { 0x08C, new CMwFieldInfo("", 0x00000024) },
		                { 0x08D, new CMwFieldInfo("", 0x00000024) },
		                { 0x08E, new CMwFieldInfo("", 0x00000024) },
		                { 0x08F, new CMwFieldInfo("", 0x00000024) },
		                { 0x090, new CMwFieldInfo("", 0x00000024) },
		                { 0x091, new CMwFieldInfo("", 0x00000024) },
		                { 0x092, new CMwFieldInfo("", 0x00000024) },
		                { 0x093, new CMwFieldInfo("", 0x00000024) },
		                { 0x094, new CMwFieldInfo("", 0x00000005) },
		                { 0x095, new CMwFieldInfo("", 0x00000024) },
		                { 0x096, new CMwFieldInfo("", 0x00000024) },
		                { 0x097, new CMwFieldInfo("", 0x00000024) },
		                { 0x098, new CMwFieldInfo("", 0x00000024) },
		                { 0x099, new CMwFieldInfo("", 0x00000005) },
		                { 0x09A, new CMwFieldInfo("", 0x00000024) },
		                { 0x09B, new CMwFieldInfo("", 0x00000005) },
		                { 0x09C, new CMwFieldInfo("", 0x00000024) },
		                { 0x09D, new CMwFieldInfo("", 0x00000024) },
		                { 0x09E, new CMwFieldInfo("", 0x00000024) },
		                { 0x09F, new CMwFieldInfo("", 0x00000024) },
		                { 0x0A0, new CMwFieldInfo("", 0x00000024) },
		                { 0x0A1, new CMwFieldInfo("", 0x00000024) },
		                { 0x0A2, new CMwFieldInfo("", 0x00000024) },
		                { 0x0A3, new CMwFieldInfo("", 0x00000024) },
		                { 0x0A4, new CMwFieldInfo("", 0x00000005) },
		                { 0x0A5, new CMwFieldInfo("", 0x00000024) },
		                { 0x0A6, new CMwFieldInfo("", 0x0000001F) },
		                { 0x0A7, new CMwFieldInfo("", 0x00000024) },
		                { 0x0A8, new CMwFieldInfo("", 0x00000024) },
		                { 0x0A9, new CMwFieldInfo("", 0x00000005) },
		                { 0x0AA, new CMwFieldInfo("", 0x00000024) },
		                { 0x0AB, new CMwFieldInfo("", 0x00000024) },
		                { 0x0AC, new CMwFieldInfo("", 0x00000024) },
		                { 0x0AD, new CMwFieldInfo("", 0x0000001F) },
		                { 0x0AE, new CMwFieldInfo("", 0x00000024) },
		                { 0x0AF, new CMwFieldInfo("", 0x00000024) },
		                { 0x0B0, new CMwFieldInfo("", 0x00000024) },
		                { 0x0B1, new CMwFieldInfo("", 0x00000026) },
		                { 0x0B2, new CMwFieldInfo("", 0x00000026) },
		                { 0x0B3, new CMwFieldInfo("", 0x00000026) },
		                { 0x0B4, new CMwFieldInfo("", 0x00000026) },
		                { 0x0B5, new CMwFieldInfo("", 0x00000026) },
		                { 0x0B6, new CMwFieldInfo("", 0x00000026) },
		                { 0x0B7, new CMwFieldInfo("", 0x00000024) },
		                { 0x0B8, new CMwFieldInfo("", 0x00000024) },
		                { 0x0B9, new CMwFieldInfo("", 0x00000024) },
		                { 0x0BA, new CMwFieldInfo("", 0x00000024) },
		                { 0x0BB, new CMwFieldInfo("", 0x00000024) },
		                { 0x0BC, new CMwFieldInfo("", 0x00000024) },
		                { 0x0BD, new CMwFieldInfo("", 0x00000024) },
		                { 0x0BE, new CMwFieldInfo("", 0x00000024) },
		                { 0x0BF, new CMwFieldInfo("", 0x00000024) },
		                { 0x0C0, new CMwFieldInfo("", 0x00000024) },
		                { 0x0C1, new CMwFieldInfo("", 0x00000024) },
		                { 0x0C2, new CMwFieldInfo("", 0x00000024) },
		                { 0x0C3, new CMwFieldInfo("", 0x00000024) },
		                { 0x0C4, new CMwFieldInfo("", 0x00000024) },
		                { 0x0C5, new CMwFieldInfo("", 0x00000024) },
		                { 0x0C6, new CMwFieldInfo("", 0x00000024) },
		                { 0x0C7, new CMwFieldInfo("", 0x00000005) },
		                { 0x0C8, new CMwFieldInfo("", 0x00000005) }
	                  })
	                },
	                { 0x02A, new CMwClassInfo("", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("", 0x00000024) },
		                { 0x001, new CMwFieldInfo("", 0x00000024) },
		                { 0x002, new CMwFieldInfo("", 0x00000005) }
	                  })
	                },
	                { 0x02B, new CMwClassInfo("", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("", 0x00000028) },
		                { 0x001, new CMwFieldInfo("", 0x0000000E) },
		                { 0x002, new CMwFieldInfo("", 0x00000001) },
		                { 0x003, new CMwFieldInfo("", 0x00000001) },
		                { 0x004, new CMwFieldInfo("", 0x00000024) },
		                { 0x005, new CMwFieldInfo("", 0x00000024) },
		                { 0x006, new CMwFieldInfo("", 0x00000024) },
		                { 0x007, new CMwFieldInfo("", 0x00000024) },
		                { 0x008, new CMwFieldInfo("", 0x00000024) },
		                { 0x009, new CMwFieldInfo("", 0x00000024) },
		                { 0x00A, new CMwFieldInfo("", 0x00000024) },
		                { 0x00B, new CMwFieldInfo("", 0x00000024) },
		                { 0x00C, new CMwFieldInfo("", 0x00000028) },
		                { 0x00D, new CMwFieldInfo("", 0x00000024) },
		                { 0x00E, new CMwEnumInfo("", new string[] { "None", "Jump", "Gliding", "Turbo" }) },
		                { 0x00F, new CMwEnumInfo("", new string[] { "None", "Burnout", "Donut", "AfterBurnout" }) },
		                { 0x010, new CMwFieldInfo("", 0x00000024) },
		                { 0x011, new CMwMethodInfo("", 0x00000000, null, null) },
		                { 0x012, new CMwFieldInfo("", 0x0000001F) }
	                  })
	                },
	                { 0x02C, new CMwClassInfo("CSceneMobilClouds", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("AutoSizeFarZ", 0x00000001) },
		                { 0x001, new CMwFieldInfo("InstCountX", 0x0000001F) },
		                { 0x002, new CMwFieldInfo("InstCountZ", 0x0000001F) },
		                { 0x003, new CMwFieldInfo("WindSpeed", 0x00000024) },
		                { 0x004, new CMwFieldInfo("WindDir", 0x00000028) },
		                { 0x005, new CMwFieldInfo("IsViewDep", 0x00000001) },
		                { 0x006, new CMwFieldInfo("FadeAlpha", 0x00000001) },
		                { 0x007, new CMwFieldInfo("ForceSize", 0x00000001) },
		                { 0x008, new CMwFieldInfo("GridSizeXZ", 0x00000031) },
		                { 0x009, new CMwFieldInfo("Solids", 0x00000007) }
	                  })
	                },
	                { 0x02D, new CMwClassInfo("CSceneToyBroomstick", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("InputBrake", 0x00000001) },
		                { 0x001, new CMwFieldInfo("InputTurbo", 0x00000001) },
		                { 0x002, new CMwFieldInfo("InputLeftRight", 0x00000028) },
		                { 0x003, new CMwFieldInfo("InputUpDown", 0x00000028) },
		                { 0x004, new CMwFieldInfo("AsyncScript", 0x00000005) },
		                { 0x005, new CMwFieldInfo("SyncScript", 0x00000005) },
		                { 0x006, new CMwFieldInfo("Propulsion", 0x00000024) },
		                { 0x007, new CMwFieldInfo("UpDownSpeed", 0x00000024) },
		                { 0x008, new CMwFieldInfo("LeftRightSpeed", 0x00000024) },
		                { 0x009, new CMwFieldInfo("Now", 0x0000001F) },
		                { 0x00A, new CMwFieldInfo("LinearSpeed", 0x00000035) },
		                { 0x00B, new CMwFieldInfo("AngSpeed", 0x00000035) },
		                { 0x00C, new CMwFieldInfo("Pos", 0x00000035) },
		                { 0x00D, new CMwFieldInfo("Rot", 0x00000013) },
		                { 0x00E, new CMwFieldInfo("CollisionTime", 0x0000001F) },
		                { 0x00F, new CMwFieldInfo("CollisionNormal", 0x00000035) },
		                { 0x010, new CMwFieldInfo("Game", 0x00000005) },
		                { 0x011, new CMwFieldInfo("Force", 0x00000035) }
	                  })
	                },
	                { 0x02E, new CMwClassInfo("", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("", 0x00000024) },
		                { 0x001, new CMwFieldInfo("", 0x00000024) },
		                { 0x002, new CMwFieldInfo("", 0x00000024) },
		                { 0x003, new CMwFieldInfo("", 0x00000005) }
	                  })
	                },
	                { 0x030, new CMwClassInfo("", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("", 0x00000029) },
		                { 0x001, new CMwFieldInfo("", 0x00000007) }
	                  })
	                },
	                { 0x031, new CMwClassInfo("", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwEnumInfo("", new string[] {  }) },
		                { 0x001, new CMwFieldInfo("", 0x00000028) },
		                { 0x002, new CMwFieldInfo("", 0x00000028) },
		                { 0x003, new CMwFieldInfo("", 0x00000028) },
		                { 0x004, new CMwFieldInfo("", 0x00000028) },
		                { 0x005, new CMwFieldInfo("", 0x00000024) },
		                { 0x006, new CMwFieldInfo("", 0x00000024) },
		                { 0x007, new CMwFieldInfo("", 0x00000005) },
		                { 0x008, new CMwFieldInfo("", 0x00000031) },
		                { 0x009, new CMwFieldInfo("", 0x00000024) },
		                { 0x00A, new CMwFieldInfo("", 0x00000024) }
	                  })
	                },
	                { 0x032, new CMwClassInfo("", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("", 0x00000024) },
		                { 0x001, new CMwFieldInfo("", 0x00000024) },
		                { 0x002, new CMwFieldInfo("", 0x00000024) },
		                { 0x003, new CMwFieldInfo("", 0x00000024) },
		                { 0x004, new CMwFieldInfo("", 0x00000024) },
		                { 0x005, new CMwFieldInfo("", 0x00000024) },
		                { 0x006, new CMwFieldInfo("", 0x00000024) },
		                { 0x007, new CMwFieldInfo("", 0x00000024) },
		                { 0x008, new CMwFieldInfo("", 0x00000024) },
		                { 0x009, new CMwFieldInfo("", 0x00000024) },
		                { 0x00A, new CMwFieldInfo("", 0x00000024) },
		                { 0x00B, new CMwFieldInfo("", 0x00000024) },
		                { 0x00C, new CMwFieldInfo("", 0x00000024) },
		                { 0x00D, new CMwFieldInfo("", 0x00000024) },
		                { 0x00E, new CMwFieldInfo("", 0x00000024) },
		                { 0x00F, new CMwFieldInfo("", 0x00000024) },
		                { 0x010, new CMwFieldInfo("", 0x00000024) },
		                { 0x011, new CMwFieldInfo("", 0x00000024) },
		                { 0x012, new CMwFieldInfo("", 0x00000024) },
		                { 0x013, new CMwFieldInfo("", 0x00000024) },
		                { 0x014, new CMwFieldInfo("", 0x00000024) },
		                { 0x015, new CMwFieldInfo("", 0x00000024) },
		                { 0x016, new CMwFieldInfo("", 0x00000024) },
		                { 0x017, new CMwFieldInfo("", 0x00000024) },
		                { 0x018, new CMwFieldInfo("", 0x00000024) },
		                { 0x019, new CMwFieldInfo("", 0x00000024) },
		                { 0x01A, new CMwFieldInfo("", 0x00000024) },
		                { 0x01B, new CMwFieldInfo("", 0x00000024) },
		                { 0x01C, new CMwFieldInfo("", 0x00000024) },
		                { 0x01D, new CMwFieldInfo("", 0x00000024) },
		                { 0x01E, new CMwFieldInfo("", 0x00000024) },
		                { 0x01F, new CMwFieldInfo("", 0x00000035) },
		                { 0x020, new CMwFieldInfo("", 0x00000035) },
		                { 0x021, new CMwFieldInfo("", 0x00000035) },
		                { 0x022, new CMwFieldInfo("", 0x00000035) },
		                { 0x023, new CMwFieldInfo("", 0x00000035) },
		                { 0x024, new CMwFieldInfo("", 0x00000035) },
		                { 0x025, new CMwFieldInfo("", 0x00000035) },
		                { 0x026, new CMwFieldInfo("", 0x00000035) },
		                { 0x027, new CMwFieldInfo("", 0x00000035) },
		                { 0x028, new CMwFieldInfo("", 0x00000035) },
		                { 0x029, new CMwFieldInfo("", 0x00000035) },
		                { 0x02A, new CMwFieldInfo("", 0x00000035) },
		                { 0x02B, new CMwFieldInfo("", 0x00000035) }
	                  })
	                },
	                { 0x033, new CMwClassInfo("", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("", 0x00000001) },
		                { 0x001, new CMwFieldInfo("", 0x00000006) }
	                  })
	                },
	                { 0x034, new CMwClassInfo("CSceneFxColors", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("ParamInverseRGB", 0x00000028) },
		                { 0x001, new CMwFieldInfo("ParamHue", 0x00000028) },
		                { 0x002, new CMwFieldInfo("ParamSaturation", 0x00000028) },
		                { 0x003, new CMwFieldInfo("ParamBrightness", 0x00000028) },
		                { 0x004, new CMwFieldInfo("ParamContrast", 0x00000028) },
		                { 0x005, new CMwFieldInfo("ParamModulateRGB", 0x00000009) },
		                { 0x006, new CMwFieldInfo("ParamModulateR", 0x00000028) },
		                { 0x007, new CMwFieldInfo("ParamModulateG", 0x00000028) },
		                { 0x008, new CMwFieldInfo("ParamModulateB", 0x00000028) },
		                { 0x009, new CMwFieldInfo("ParamBlendRGB", 0x00000009) },
		                { 0x00A, new CMwFieldInfo("ParamBlendAlpha", 0x00000028) },
		                { 0x00B, new CMwFieldInfo("ParamUserEnable", 0x00000001) },
		                { 0x00C, new CMwFieldInfo("UserIntensity", 0x00000028) },
		                { 0x00D, new CMwFieldInfo("CloudsIntensity", 0x00000028) },
		                { 0x00E, new CMwFieldInfo("ZFarIntensity", 0x00000028) },
		                { 0x00F, new CMwFieldInfo("ParamZFar_StartZ", 0x00000024) },
		                { 0x010, new CMwFieldInfo("ParamZFar_StopZ", 0x00000024) },
		                { 0x011, new CMwFieldInfo("ParamZFarInverseRGB", 0x00000028) },
		                { 0x012, new CMwFieldInfo("ParamZFarHue", 0x00000028) },
		                { 0x013, new CMwFieldInfo("ParamZFarSaturation", 0x00000028) },
		                { 0x014, new CMwFieldInfo("ParamZFarBrightness", 0x00000028) },
		                { 0x015, new CMwFieldInfo("ParamZFarContrast", 0x00000028) },
		                { 0x016, new CMwFieldInfo("ParamZFarModulateRGB", 0x00000009) },
		                { 0x017, new CMwFieldInfo("ParamZFarModulateR", 0x00000028) },
		                { 0x018, new CMwFieldInfo("ParamZFarModulateG", 0x00000028) },
		                { 0x019, new CMwFieldInfo("ParamZFarModulateB", 0x00000028) },
		                { 0x01A, new CMwFieldInfo("ParamZFarBlendRGB", 0x00000009) },
		                { 0x01B, new CMwFieldInfo("ParamZFarBlendAlpha", 0x00000028) },
		                { 0x01C, new CMwMethodInfo("HdrLabel", 0x00000000, null, null) },
		                { 0x01D, new CMwEnumInfo("HdrOutput", new string[] { "Dynamic", "Min", "Max" }) },
		                { 0x01E, new CMwFieldInfo("HdrSunFactor", 0x00000028) },
		                { 0x01F, new CMwFieldInfo("HdrSunSkyFactor", 0x00000028) },
		                { 0x020, new CMwFieldInfo("HdrSkyFactor", 0x00000028) },
		                { 0x021, new CMwFieldInfo("HdrScene", 0x00000024) },
		                { 0x022, new CMwFieldInfo("HdrEye", 0x00000024) },
		                { 0x023, new CMwFieldInfo("HdrVertigo", 0x00000028) },
		                { 0x024, new CMwFieldInfo("HdrSunTimeRise", 0x00000024) },
		                { 0x025, new CMwFieldInfo("HdrSunTimeFall", 0x00000024) },
		                { 0x026, new CMwFieldInfo("HdrInputSun", 0x00000024) },
		                { 0x027, new CMwFieldInfo("HdrSkyTimeRise", 0x00000024) },
		                { 0x028, new CMwFieldInfo("HdrSkyTimeFall", 0x00000024) },
		                { 0x029, new CMwFieldInfo("HdrInputSky", 0x00000024) },
		                { 0x02A, new CMwFieldInfo("HdrInputSunSkyDotExp", 0x00000024) },
		                { 0x02B, new CMwFieldInfo("HdrInputSunSky", 0x00000024) },
		                { 0x02C, new CMwFieldInfo("HdrTimeRise", 0x00000024) },
		                { 0x02D, new CMwFieldInfo("HdrTimeFall", 0x00000024) },
		                { 0x02E, new CMwFieldInfo("HdrVertigoExp", 0x00000024) },
		                { 0x02F, new CMwMethodInfo("HdrMinLabel", 0x00000000, null, null) },
		                { 0x030, new CMwFieldInfo("ParamHdrMinInverseRGB", 0x00000028) },
		                { 0x031, new CMwFieldInfo("ParamHdrMinHue", 0x00000028) },
		                { 0x032, new CMwFieldInfo("ParamHdrMinSaturation", 0x00000028) },
		                { 0x033, new CMwFieldInfo("ParamHdrMinBrightness", 0x00000028) },
		                { 0x034, new CMwFieldInfo("ParamHdrMinContrast", 0x00000028) },
		                { 0x035, new CMwFieldInfo("ParamHdrMinModulateRGB", 0x00000009) },
		                { 0x036, new CMwFieldInfo("ParamHdrMinModulateR", 0x00000028) },
		                { 0x037, new CMwFieldInfo("ParamHdrMinModulateG", 0x00000028) },
		                { 0x038, new CMwFieldInfo("ParamHdrMinModulateB", 0x00000028) },
		                { 0x039, new CMwFieldInfo("ParamHdrMinBlendRGB", 0x00000009) },
		                { 0x03A, new CMwFieldInfo("ParamHdrMinBlendAlpha", 0x00000028) },
		                { 0x03B, new CMwMethodInfo("HdrMaxLabel", 0x00000000, null, null) },
		                { 0x03C, new CMwFieldInfo("ParamHdrMaxInverseRGB", 0x00000028) },
		                { 0x03D, new CMwFieldInfo("ParamHdrMaxHue", 0x00000028) },
		                { 0x03E, new CMwFieldInfo("ParamHdrMaxSaturation", 0x00000028) },
		                { 0x03F, new CMwFieldInfo("ParamHdrMaxBrightness", 0x00000028) },
		                { 0x040, new CMwFieldInfo("ParamHdrMaxContrast", 0x00000028) },
		                { 0x041, new CMwFieldInfo("ParamHdrMaxModulateRGB", 0x00000009) },
		                { 0x042, new CMwFieldInfo("ParamHdrMaxModulateR", 0x00000028) },
		                { 0x043, new CMwFieldInfo("ParamHdrMaxModulateG", 0x00000028) },
		                { 0x044, new CMwFieldInfo("ParamHdrMaxModulateB", 0x00000028) },
		                { 0x045, new CMwFieldInfo("ParamHdrMaxBlendRGB", 0x00000009) },
		                { 0x046, new CMwFieldInfo("ParamHdrMaxBlendAlpha", 0x00000028) },
		                { 0x047, new CMwFieldInfo("MaterialColors", 0x00000005) },
		                { 0x048, new CMwFieldInfo("MaterialSky", 0x00000005) },
		                { 0x049, new CMwFieldInfo("BitmapSbch", 0x00000005) },
		                { 0x04A, new CMwFieldInfo("BitmapTcScale", 0x00000031) },
		                { 0x04B, new CMwFieldInfo("BitmapTcPeriod", 0x00000031) },
		                { 0x04C, new CMwFieldInfo("BitmapRangeR", 0x00000031) },
		                { 0x04D, new CMwFieldInfo("BitmapRangeG", 0x00000031) },
		                { 0x04E, new CMwFieldInfo("BitmapRangeB", 0x00000031) },
		                { 0x04F, new CMwFieldInfo("BitmapRangeA", 0x00000031) }
	                  })
	                },
	                { 0x035, new CMwClassInfo("CSceneFxSuperSample", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("SampleCountX", 0x0000001F) },
		                { 0x001, new CMwFieldInfo("SampleCountY", 0x0000001F) },
		                { 0x002, new CMwFieldInfo("Use32bAccum", 0x00000001) },
		                { 0x003, new CMwFieldInfo("SoftShadows", 0x00000001) },
		                { 0x004, new CMwFieldInfo("AmbientOcc", 0x00000001) },
		                { 0x005, new CMwFieldInfo("TweakTestAlpha", 0x00000001) },
		                { 0x006, new CMwFieldInfo("GlobalScaleIsAuto", 0x00000001) },
		                { 0x007, new CMwFieldInfo("GlobalScale", 0x00000024) },
		                { 0x008, new CMwFieldInfo("CanBlendOnAccum", 0x00000001) },
		                { 0x009, new CMwFieldInfo("PointsInSphereOpt", 0x00000005) }
	                  })
	                },
	                { 0x036, new CMwClassInfo("CSceneLocationCamera", new Dictionary<int, CMwMemberInfo>()
	                  {
	                  })
	                },
	                { 0x037, new CMwClassInfo("CSceneToyRock", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("RockMG", 0x00000024) },
		                { 0x001, new CMwFieldInfo("ViscoAir", 0x00000024) },
		                { 0x002, new CMwFieldInfo("ViscoWater", 0x00000024) },
		                { 0x003, new CMwMethodInfo("Reset", 0x00000000, null, null) }
	                  })
	                },
	                { 0x038, new CMwClassInfo("CSceneFxFlares", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("FlarePerBlock", 0x0000001F) }
	                  })
	                },
	                { 0x039, new CMwClassInfo("", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("", 0x0000001F) },
		                { 0x001, new CMwFieldInfo("", 0x00000005) },
		                { 0x002, new CMwFieldInfo("", 0x00000005) },
		                { 0x003, new CMwFieldInfo("", 0x00000005) }
	                  })
	                },
	                { 0x03A, new CMwClassInfo("CSceneFxNod", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("IsActive", 0x00000001) },
		                { 0x001, new CMwFieldInfo("Fx", 0x00000005) },
		                { 0x002, new CMwFieldInfo("NodInputs", 0x00000007) }
	                  })
	                },
	                { 0x03B, new CMwClassInfo("CSceneFxBloom", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("BlurSize", 0x0000001F) },
		                { 0x001, new CMwFieldInfo("Intensity", 0x00000028) },
		                { 0x002, new CMwFieldInfo("DataBlend", 0x00000028) },
		                { 0x003, new CMwFieldInfo("DualData", 0x00000001) },
		                { 0x004, new CMwFieldInfo("Datas", 0x00000007) },
		                { 0x005, new CMwEnumInfo("RadialInput", new string[] { "None", "2d", "3d" }) },
		                { 0x006, new CMwFieldInfo("RadialIntens", 0x00000028) },
		                { 0x007, new CMwFieldInfo("m_RadialProjQuality", 0x0000001F) },
		                { 0x008, new CMwFieldInfo("RadialRadius2d", 0x00000024) },
		                { 0x009, new CMwFieldInfo("RadialRadius3d", 0x00000024) },
		                { 0x00A, new CMwFieldInfo("RadialOffset2d", 0x00000031) },
		                { 0x00B, new CMwFieldInfo("RadialOffset3d", 0x00000035) }
	                  })
	                },
	                { 0x03C, new CMwClassInfo("", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("", 0x00000024) },
		                { 0x001, new CMwFieldInfo("", 0x00000024) },
		                { 0x002, new CMwFieldInfo("", 0x00000005) }
	                  })
	                },
	                { 0x03D, new CMwClassInfo("CSceneFxToneMapping", new Dictionary<int, CMwMemberInfo>()
	                  {
	                  })
	                },
	                { 0x03E, new CMwClassInfo("CBoatSailState", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("SailTree", 0x00000005) },
		                { 0x001, new CMwFieldInfo("BoomTree", 0x00000005) },
		                { 0x002, new CMwFieldInfo("BoomLowTree", 0x00000005) },
		                { 0x003, new CMwFieldInfo("SailVisualBase", 0x00000005) },
		                { 0x004, new CMwFieldInfo("SailVisualFlat", 0x00000005) },
		                { 0x005, new CMwEnumInfo("SailState", new string[] { "PrepareHaulUp", "HaulUp", "Up", "PrepareHaulDown", "HaulDown", "Down" }) },
		                { 0x006, new CMwFieldInfo("SailStateCoef", 0x00000028) },
		                { 0x007, new CMwFieldInfo("AutomaticSheetTargetSpeedEnable", 0x00000001) }
	                  })
	                },
	                { 0x03F, new CMwClassInfo("CSceneFxBloomData", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("HighInvExponent", 0x00000028) },
		                { 0x001, new CMwFieldInfo("FakeHdrExponent", 0x00000028) },
		                { 0x002, new CMwFieldInfo("FakeHdrMin", 0x00000028) }
	                  })
	                },
	                { 0x040, new CMwClassInfo("CSceneConfig", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("VisionPC0", 0x00000005) },
		                { 0x001, new CMwFieldInfo("VisionPC1", 0x00000005) },
		                { 0x002, new CMwFieldInfo("VisionPC2", 0x00000005) },
		                { 0x003, new CMwFieldInfo("VisionPC3", 0x00000005) }
	                  })
	                },
	                { 0x041, new CMwClassInfo("CSceneConfigVision", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("HmsConfig", 0x00000005) }
	                  })
	                },
	                { 0x042, new CMwClassInfo("CSceneMood", new Dictionary<int, CMwMemberInfo>()
	                  {
	                  })
	                },
	                { 0x043, new CMwClassInfo("CSceneFxStereoscopy", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwEnumInfo("Output", new string[] { "Left Right", "Right Left", "Red Cyan", "Top Bottom", "Bottom Top", "Line Even Odd", "Line Odd Even", "DbgBlend", "DbgLeft", "DbgRight" }) },
		                { 0x001, new CMwFieldInfo("ExternalControl", 0x00000001) },
		                { 0x002, new CMwFieldInfo("Separation", 0x00000028) },
		                { 0x003, new CMwFieldInfo("ScreenDist", 0x00000028) },
		                { 0x004, new CMwFieldInfo("MarginPixelCount", 0x0000001F) },
		                { 0x005, new CMwEnumInfo("SplitRatio", new string[] { "Screen", "Split" }) },
		                { 0x006, new CMwEnumInfo("AnaglyphColor", new string[] { "Gray->Full", "Gray->Half" }) },
		                { 0x007, new CMwFieldInfo("AnaglyphColorFactor", 0x00000028) },
		                { 0x008, new CMwFieldInfo("Fid_PHlsl_AnaglyphFullColor", 0x00000005) },
		                { 0x009, new CMwFieldInfo("Fid_PHlsl_AnaglyphHalfColor", 0x00000005) }
	                  })
	                },
	                { 0x044, new CMwClassInfo("CSceneFxHeadTrack", new Dictionary<int, CMwMemberInfo>()
	                  {
	                  })
	                },
	                { 0x04F, new CMwClassInfo("CSceneToyDisplayProgress", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("Progress", 0x00000028) },
		                { 0x001, new CMwFieldInfo("ProgressFuncShader", 0x00000005) },
		                { 0x002, new CMwFieldInfo("ProgressTreeId", 0x00000029) }
	                  })
	                },
	                { 0x050, new CMwClassInfo("CSceneToyDisplayHistogram", new Dictionary<int, CMwMemberInfo>()
	                  {
	                  })
	                },
	                { 0x051, new CMwClassInfo("CSceneToyDisplayGraph", new Dictionary<int, CMwMemberInfo>()
	                  {
	                  })
	                },
	                { 0x059, new CMwClassInfo("CSceneField", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwMethodInfo("CreateFieldBall", 0x00000000, null, null) },
		                { 0x001, new CMwMethodInfo("CreateFieldUniform", 0x00000000, null, null) }
	                  })
	                },
	                { 0x05D, new CMwClassInfo("CSceneMoods", new Dictionary<int, CMwMemberInfo>()
	                  {
	                  })
	                },
	                { 0x05E, new CMwClassInfo("CSceneMobilLeaves", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("LeafMaxCount", 0x0000001F) },
		                { 0x001, new CMwFieldInfo("LeafEmitterMaxCount", 0x0000001F) },
		                { 0x002, new CMwFieldInfo("LeafShader", 0x00000005) },
		                { 0x003, new CMwMethodInfo("ResetLeaves", 0x00000000, null, null) },
		                { 0x004, new CMwFieldInfo("LeafRadiusBase", 0x00000024) },
		                { 0x005, new CMwFieldInfo("LeafRadiusRandom", 0x00000024) },
		                { 0x006, new CMwFieldInfo("Wind", 0x00000035) },
		                { 0x007, new CMwFieldInfo("LeafOscillationAmplitudeBase", 0x00000024) },
		                { 0x008, new CMwFieldInfo("LeafOscillationAmplitudeRandom", 0x00000024) },
		                { 0x009, new CMwFieldInfo("LeafOscillationPeriodBase", 0x00000024) },
		                { 0x00A, new CMwFieldInfo("LeafOscillationPeriodRandom", 0x00000024) },
		                { 0x00B, new CMwFieldInfo("LeafFallingSpeedBase", 0x00000024) },
		                { 0x00C, new CMwFieldInfo("LeafFallingSpeedRandom", 0x00000024) },
		                { 0x00D, new CMwFieldInfo("LeafAlphaSpeedMax", 0x00000024) },
		                { 0x00E, new CMwFieldInfo("LeafBetaSpeedlMax", 0x00000024) },
		                { 0x00F, new CMwFieldInfo("RespawnPeriod", 0x00000024) },
		                { 0x010, new CMwFieldInfo("FarZ", 0x00000024) },
		                { 0x011, new CMwFieldInfo("Curvature", 0x00000028) }
	                  })
	                },
	                { 0x05F, new CMwClassInfo("CSceneToyMotorbike", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("MaxSpeed", 0x00000024) },
		                { 0x001, new CMwFieldInfo("Acceleration", 0x00000028) },
		                { 0x002, new CMwFieldInfo("Brake", 0x00000028) },
		                { 0x003, new CMwFieldInfo("Steer", 0x00000028) },
		                { 0x004, new CMwFieldInfo("AirControl", 0x00000028) },
		                { 0x005, new CMwFieldInfo("BumpSlowing", 0x00000028) },
		                { 0x006, new CMwFieldInfo("BumpSlowingDuration", 0x0000001F) },
		                { 0x007, new CMwFieldInfo("KJump", 0x00000024) },
		                { 0x008, new CMwFieldInfo("AirSlowing", 0x00000024) },
		                { 0x009, new CMwFieldInfo("KeepContactDuration", 0x0000001F) },
		                { 0x00A, new CMwFieldInfo("EnvMaterials", 0x00000006) },
		                { 0x00B, new CMwFieldInfo("RPMMax", 0x00000024) },
		                { 0x00C, new CMwFieldInfo("RPMCoef", 0x00000024) },
		                { 0x00D, new CMwFieldInfo("RPMDecDuration", 0x00000024) },
		                { 0x00E, new CMwFieldInfo("RPMDecWantedVal", 0x00000024) },
		                { 0x00F, new CMwFieldInfo("InputSteerReachMaxDuration", 0x0000001F) },
		                { 0x010, new CMwFieldInfo("SteerRadiusMin", 0x00000024) },
		                { 0x011, new CMwFieldInfo("SteerRadiusMax", 0x00000024) },
		                { 0x012, new CMwFieldInfo("ConstrainedSteerRadius", 0x00000001) },
		                { 0x013, new CMwFieldInfo("WheelingSteerCoef", 0x00000028) },
		                { 0x014, new CMwFieldInfo("WheelingMaxSpeedCoef", 0x00000028) },
		                { 0x015, new CMwFieldInfo("SteerPivot", 0x00000024) },
		                { 0x016, new CMwFieldInfo("MaxXAxisSpeed", 0x00000024) },
		                { 0x017, new CMwFieldInfo("RoadSteerCoef", 0x00000024) },
		                { 0x018, new CMwFieldInfo("InputMegaGrip", 0x00000001) },
		                { 0x019, new CMwFieldInfo("MegaGripForce", 0x00000001) },
		                { 0x01A, new CMwFieldInfo("WheelingThreshold", 0x00000024) },
		                { 0x01B, new CMwFieldInfo("WheelingSpeedThreshold", 0x00000024) },
		                { 0x01C, new CMwFieldInfo("RotationSpeedThreshold", 0x00000024) },
		                { 0x01D, new CMwFieldInfo("GroundToleranceAngle", 0x00000024) },
		                { 0x01E, new CMwFieldInfo("DeclutchingBrakeValue", 0x00000024) },
		                { 0x01F, new CMwFieldInfo("FollowNormalKi", 0x00000024) },
		                { 0x020, new CMwFieldInfo("FollowNormalKa", 0x00000024) },
		                { 0x021, new CMwFieldInfo("KInclination", 0x00000024) },
		                { 0x022, new CMwEnumInfo("FallCondition", new string[] { "Never", "Gas", "SpeedAndGas", "SpeedAndAngle" }) },
		                { 0x023, new CMwFieldInfo("FallSpeedThreshold", 0x00000024) },
		                { 0x024, new CMwFieldInfo("FallAngleThreshold", 0x00000024) },
		                { 0x025, new CMwFieldInfo("FallImpulseSpeedBase", 0x00000024) },
		                { 0x026, new CMwFieldInfo("FallImpulseSpeedCoef", 0x00000024) },
		                { 0x027, new CMwFieldInfo("FallImpulseDirUp", 0x00000024) },
		                { 0x028, new CMwFieldInfo("FallFluidFrictionCoef", 0x00000024) },
		                { 0x029, new CMwFieldInfo("IsRiderAnimation", 0x00000001) },
		                { 0x02A, new CMwFieldInfo("InputGas", 0x00000024) },
		                { 0x02B, new CMwFieldInfo("InputBrake", 0x00000024) },
		                { 0x02C, new CMwFieldInfo("InputSteer", 0x00000024) },
		                { 0x02D, new CMwFieldInfo("InputDriverLeftRight", 0x00000024) },
		                { 0x02E, new CMwFieldInfo("InputDriverBackFront", 0x00000024) },
		                { 0x02F, new CMwFieldInfo("InputTrick1", 0x00000001) },
		                { 0x030, new CMwFieldInfo("CurrentTrickIndex", 0x0000001F) },
		                { 0x031, new CMwEnumInfo("GamePlay", new string[] { "Base", "MotoRacer" }) },
		                { 0x032, new CMwEnumInfo("MotoVersion", new string[] { "MotoV1", "MotoV2", "MotoV3" }) },
		                { 0x033, new CMwFieldInfo("IsWallInEnv", 0x00000001) },
		                { 0x034, new CMwFieldInfo("IsLowQuality", 0x00000001) },
		                { 0x035, new CMwFieldInfo("ImpactMin", 0x0000001F) },
		                { 0x036, new CMwFieldInfo("ImpactMax", 0x0000001F) },
		                { 0x037, new CMwFieldInfo("StandardPose", 0x00000005) },
		                { 0x038, new CMwFieldInfo("FullLeftPose", 0x00000005) },
		                { 0x039, new CMwFieldInfo("FullRightPose", 0x00000005) },
		                { 0x03A, new CMwFieldInfo("FullStandPose", 0x00000005) },
		                { 0x03B, new CMwFieldInfo("TricksKeys", 0x00000007) },
		                { 0x03C, new CMwFieldInfo("EngineLoudness", 0x0000001F) },
		                { 0x03D, new CMwFieldInfo("SoundEngineMinFreq", 0x0000001F) },
		                { 0x03E, new CMwFieldInfo("SoundEngineMaxFreq", 0x0000001F) },
		                { 0x03F, new CMwFieldInfo("ImpactLoudness", 0x0000001F) },
		                { 0x040, new CMwFieldInfo("SoundEngine", 0x00000005) },
		                { 0x041, new CMwFieldInfo("SoundJumping", 0x00000005) },
		                { 0x042, new CMwFieldInfo("SoundLanding", 0x00000005) },
		                { 0x043, new CMwFieldInfo("SoundImpact", 0x00000005) },
		                { 0x044, new CMwFieldInfo("SoundShot", 0x00000005) },
		                { 0x045, new CMwFieldInfo("SoundShock", 0x00000005) },
		                { 0x046, new CMwFieldInfo("SoundDefaultGround", 0x00000005) },
		                { 0x047, new CMwFieldInfo("SoundDefaultSkidding", 0x00000005) },
		                { 0x048, new CMwFieldInfo("SoundDefaultChafing", 0x00000005) }
	                  })
	                },
	                { 0x060, new CMwClassInfo("", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("", 0x00000005) },
		                { 0x001, new CMwFieldInfo("", 0x00000005) },
		                { 0x002, new CMwMethodInfo("", 0x00000000, null, null) },
		                { 0x003, new CMwFieldInfo("", 0x00000028) },
		                { 0x004, new CMwFieldInfo("", 0x00000028) },
		                { 0x005, new CMwFieldInfo("", 0x00000028) },
		                { 0x006, new CMwFieldInfo("", 0x00000005) },
		                { 0x007, new CMwFieldInfo("", 0x00000005) },
		                { 0x008, new CMwFieldInfo("", 0x00000005) },
		                { 0x009, new CMwFieldInfo("", 0x00000001) },
		                { 0x00A, new CMwFieldInfo("", 0x00000001) },
		                { 0x00B, new CMwMethodInfo("", 0x00000000, null, null) }
	                  })
	                },
	                { 0x061, new CMwClassInfo("", new Dictionary<int, CMwMemberInfo>()
	                  {
	                  })
	                },
	                { 0x062, new CMwClassInfo("CSceneTrafficGraph", new Dictionary<int, CMwMemberInfo>()
	                  {
	                  })
	                },
	                { 0x063, new CMwClassInfo("CSceneTrafficPath", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("DropVehicles", 0x00000005) },
		                { 0x001, new CMwFieldInfo("Name", 0x00000029) },
		                { 0x002, new CMwFieldInfo("Length", 0x0000001F) },
		                { 0x003, new CMwFieldInfo("SpeedLimits", 0x00000025) },
		                { 0x004, new CMwFieldInfo("ScenePaths", 0x00000006) }
	                  })
	                },
	                { 0x067, new CMwClassInfo("CSceneToyFilaments", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("Alpha", 0x00000024) },
		                { 0x001, new CMwFieldInfo("k1", 0x00000024) },
		                { 0x002, new CMwFieldInfo("k2", 0x00000024) },
		                { 0x003, new CMwFieldInfo("k3", 0x00000024) },
		                { 0x004, new CMwFieldInfo("MaxSpeed", 0x00000024) },
		                { 0x005, new CMwFieldInfo("Inertia", 0x00000024) },
		                { 0x006, new CMwFieldInfo("NoiseAmp", 0x00000024) },
		                { 0x007, new CMwFieldInfo("NoiseFreq", 0x00000024) },
		                { 0x008, new CMwFieldInfo("AddNoise", 0x00000001) },
		                { 0x009, new CMwFieldInfo("ShowGradient", 0x00000001) }
	                  })
	                },
	                { 0x068, new CMwClassInfo("CSceneMobilTraffic", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwEnumInfo("Kind", new string[] { "Smooth turns", "Sharp turns" }) },
		                { 0x001, new CMwEnumInfo("Speed", new string[] { "Slow", "Average", "Fast" }) },
		                { 0x002, new CMwFieldInfo("TrafficPath", 0x00000005) },
		                { 0x003, new CMwFieldInfo("TrafficPathTimeOffset", 0x00000024) }
	                  })
	                },
	                { 0x069, new CMwClassInfo("CSceneMobilFlockAttractor", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("Radius", 0x00000024) },
		                { 0x001, new CMwFieldInfo("Power", 0x00000024) },
		                { 0x002, new CMwFieldInfo("SpawnCount", 0x0000001F) },
		                { 0x003, new CMwFieldInfo("IsRepulsor", 0x00000001) }
	                  })
	                },
	                { 0x06A, new CMwClassInfo("CSceneExtraFlocking", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwMethodInfo("Start", 0x00000000, null, null) },
		                { 0x001, new CMwMethodInfo("Stop", 0x00000000, null, null) },
		                { 0x002, new CMwFieldInfo("BirdModel", 0x00000005) },
		                { 0x003, new CMwFieldInfo("Sound", 0x00000005) },
		                { 0x004, new CMwFieldInfo("Attractors", 0x00000007) },
		                { 0x005, new CMwFieldInfo("Volatility", 0x00000028) },
		                { 0x006, new CMwFieldInfo("Range", 0x00000024) },
		                { 0x007, new CMwFieldInfo("CosViewAngle", 0x00000024) },
		                { 0x008, new CMwFieldInfo("MinSpeed", 0x00000024) },
		                { 0x009, new CMwFieldInfo("MaxSpeed", 0x00000024) },
		                { 0x00A, new CMwFieldInfo("UpdateFrequency", 0x0000001F) },
		                { 0x00B, new CMwFieldInfo("Variance", 0x00000024) },
		                { 0x00C, new CMwFieldInfo("vAvoidance", 0x00000024) },
		                { 0x00D, new CMwFieldInfo("kAvoidance", 0x00000024) },
		                { 0x00E, new CMwFieldInfo("vGrouping", 0x00000024) },
		                { 0x00F, new CMwFieldInfo("kGrouping", 0x00000024) },
		                { 0x010, new CMwFieldInfo("vMatching", 0x00000024) },
		                { 0x011, new CMwFieldInfo("kMatching", 0x00000024) },
		                { 0x012, new CMwFieldInfo("vGroundAvoid", 0x00000024) },
		                { 0x013, new CMwFieldInfo("kGroundAvoid", 0x00000024) },
		                { 0x014, new CMwFieldInfo("GroundAltitude", 0x00000024) },
		                { 0x015, new CMwFieldInfo("StandingDuration", 0x0000001F) },
		                { 0x016, new CMwFieldInfo("AnimPeriod", 0x0000001F) }
	                  })
	                },
	                { 0x06B, new CMwClassInfo("CSceneMotorbikeEnvMaterial", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("Grip", 0x00000024) },
		                { 0x001, new CMwFieldInfo("Bump", 0x00000024) },
		                { 0x002, new CMwFieldInfo("Slowing", 0x00000024) },
		                { 0x003, new CMwFieldInfo("SoundGround", 0x00000005) },
		                { 0x004, new CMwFieldInfo("SoundSkidding", 0x00000005) },
		                { 0x005, new CMwFieldInfo("SoundChafing", 0x00000005) }
	                  })
	                },
	                { 0x06C, new CMwClassInfo("CSceneExtraFlockingCharacters", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwMethodInfo("Start", 0x00000000, null, null) },
		                { 0x001, new CMwMethodInfo("Stop", 0x00000000, null, null) },
		                { 0x002, new CMwMethodInfo("Kill", 0x00000000, null, null) },
		                { 0x003, new CMwFieldInfo("Model", 0x00000005) },
		                { 0x004, new CMwFieldInfo("Attractors", 0x00000007) },
		                { 0x005, new CMwFieldInfo("Volatility", 0x00000028) },
		                { 0x006, new CMwFieldInfo("Range", 0x00000024) },
		                { 0x007, new CMwFieldInfo("CosViewAngle", 0x00000024) },
		                { 0x008, new CMwFieldInfo("MinSpeed", 0x00000024) },
		                { 0x009, new CMwFieldInfo("MaxSpeed", 0x00000024) },
		                { 0x00A, new CMwFieldInfo("UpdateFrequency", 0x0000001F) },
		                { 0x00B, new CMwFieldInfo("Variance", 0x00000024) },
		                { 0x00C, new CMwFieldInfo("vAvoidance", 0x00000024) },
		                { 0x00D, new CMwFieldInfo("kAvoidance", 0x00000024) },
		                { 0x00E, new CMwFieldInfo("vGrouping", 0x00000024) },
		                { 0x00F, new CMwFieldInfo("kGrouping", 0x00000024) },
		                { 0x010, new CMwFieldInfo("vMatching", 0x00000024) },
		                { 0x011, new CMwFieldInfo("kMatching", 0x00000024) },
		                { 0x012, new CMwFieldInfo("SpawnColCount", 0x0000001F) },
		                { 0x013, new CMwFieldInfo("SpawnRowCount", 0x0000001F) }
	                  })
	                },
	                { 0x06D, new CMwClassInfo("CSceneVehicleSpeedBoat", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("IsContact", 0x00000001) }
	                  })
	                },
	                { 0x06E, new CMwClassInfo("CSceneToySubway", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("Path", 0x00000005) },
		                { 0x001, new CMwFieldInfo("SubwayPeriod", 0x00000024) },
		                { 0x002, new CMwFieldInfo("SubwaySpeed", 0x00000024) },
		                { 0x003, new CMwMethodInfo("SwitchOn", 0x00000000, null, null) },
		                { 0x004, new CMwMethodInfo("SwitchOff", 0x00000000, null, null) },
		                { 0x005, new CMwMethodInfo("CreateDefaultPathFromCurrentLocation", 0x00000000, null, null) },
		                { 0x006, new CMwFieldInfo("DefaultPathLength", 0x00000024) },
		                { 0x007, new CMwFieldInfo("LineCount", 0x0000001F) },
		                { 0x008, new CMwFieldInfo("CubeCount", 0x0000001F) },
		                { 0x009, new CMwFieldInfo("Height", 0x00000024) },
		                { 0x00A, new CMwFieldInfo("Width", 0x00000024) },
		                { 0x00B, new CMwFieldInfo("Length", 0x00000024) },
		                { 0x00C, new CMwFieldInfo("SubwaySound", 0x00000005) },
		                { 0x00D, new CMwFieldInfo("Font", 0x00000005) },
		                { 0x00E, new CMwFieldInfo("LineHeight", 0x00000024) },
		                { 0x00F, new CMwFieldInfo("LetterScaleX", 0x00000024) },
		                { 0x010, new CMwFieldInfo("LetterScaleY", 0x00000024) },
		                { 0x011, new CMwFieldInfo("LetterScalePeriod", 0x00000024) },
		                { 0x012, new CMwFieldInfo("LetterColor", 0x00000009) },
		                { 0x013, new CMwFieldInfo("CubePeriod", 0x00000024) },
		                { 0x014, new CMwFieldInfo("CubeSize", 0x00000024) },
		                { 0x015, new CMwFieldInfo("CubeColor", 0x00000009) },
		                { 0x016, new CMwFieldInfo("IsRootRotated", 0x00000001) },
		                { 0x017, new CMwFieldInfo("UseCubeFiles", 0x00000001) }
	                  })
	                },
	                { 0x06F, new CMwClassInfo("CSceneToyBird", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwMethodInfo("MakeFlockMaster", 0x00000000, null, null) },
		                { 0x001, new CMwMethodInfo("LeaveFlock", 0x00000000, null, null) },
		                { 0x002, new CMwFieldInfo("SphereLocation", 0x00000005) },
		                { 0x003, new CMwFieldInfo("Volatility", 0x00000028) },
		                { 0x004, new CMwFieldInfo("Range", 0x00000024) },
		                { 0x005, new CMwFieldInfo("CosViewAngle", 0x00000024) },
		                { 0x006, new CMwFieldInfo("MinSpeed", 0x00000024) },
		                { 0x007, new CMwFieldInfo("MaxSpeed", 0x00000024) },
		                { 0x008, new CMwFieldInfo("UpdateFrequency", 0x0000001F) },
		                { 0x009, new CMwFieldInfo("Variance", 0x00000024) },
		                { 0x00A, new CMwFieldInfo("MaxFlockSize", 0x00000024) },
		                { 0x00B, new CMwFieldInfo("vAvoidance", 0x00000024) },
		                { 0x00C, new CMwFieldInfo("kAvoidance", 0x00000024) },
		                { 0x00D, new CMwFieldInfo("vGrouping", 0x00000024) },
		                { 0x00E, new CMwFieldInfo("kGrouping", 0x00000024) },
		                { 0x00F, new CMwFieldInfo("vMatching", 0x00000024) },
		                { 0x010, new CMwFieldInfo("kMatching", 0x00000024) },
		                { 0x011, new CMwFieldInfo("vSphereAvoidance", 0x00000024) },
		                { 0x012, new CMwFieldInfo("kSphereAvoidance", 0x00000024) }
	                  })
	                },
	                { 0x070, new CMwClassInfo("CSceneMobilSnow", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwEnumInfo("Kind", new string[] { "Snow", "Rain", "Suspension", "Kanji", "Pollen", "Stars" }) },
		                { 0x001, new CMwFieldInfo("Intensity", 0x00000028) },
		                { 0x002, new CMwFieldInfo("FarZ", 0x00000024) },
		                { 0x003, new CMwFieldInfo("LifeSpan", 0x0000001F) },
		                { 0x004, new CMwFieldInfo("Diam", 0x00000024) },
		                { 0x005, new CMwFieldInfo("XYRatio", 0x00000024) },
		                { 0x006, new CMwFieldInfo("WindRotate", 0x00000001) },
		                { 0x007, new CMwFieldInfo("Param1", 0x00000024) },
		                { 0x008, new CMwEnumInfo("EditKind", new string[] { "Snow", "Rain", "Suspension", "Kanji", "Pollen", "Stars" }) },
		                { 0x009, new CMwFieldInfo("EditBitmap", 0x00000005) },
		                { 0x00A, new CMwFieldInfo("EditParticleCount", 0x0000001F) },
		                { 0x00B, new CMwMethodInfo("EditInit", 0x00000000, null, null) }
	                  })
	                },
	                { 0x071, new CMwClassInfo("CSceneToyLeash", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("", 0x00000007) },
		                { 0x001, new CMwFieldInfo("", 0x00000028) },
		                { 0x002, new CMwFieldInfo("", 0x00000028) },
		                { 0x003, new CMwMethodInfo("", 0x00000041,
			                new uint[] { 0x0A011000, 0x0000001F },
			                new string[] { "Mobil", "Index" })
		                },
		                { 0x004, new CMwMethodInfo("", 0x00000041,
			                new uint[] { 0x0A011000 },
			                new string[] { "Mobil" })
		                }
	                  })
	                },
	                { 0x072, new CMwClassInfo("CSceneFx", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("Intensity", 0x00000028) },
		                { 0x001, new CMwFieldInfo("WantPreLoad", 0x00000001) }
	                  })
	                },
	                { 0x073, new CMwClassInfo("CSceneFxVisionK", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("DurationStart", 0x0000001F) },
		                { 0x001, new CMwFieldInfo("DurationEnd", 0x0000001F) },
		                { 0x002, new CMwFieldInfo("GrayFactor", 0x00000028) },
		                { 0x003, new CMwFieldInfo("GrayRGB", 0x00000009) },
		                { 0x004, new CMwFieldInfo("GlowNewG", 0x00000028) },
		                { 0x005, new CMwFieldInfo("GlowGain", 0x00000028) },
		                { 0x006, new CMwFieldInfo("GlowRecur", 0x00000028) },
		                { 0x007, new CMwFieldInfo("GlowLerp", 0x00000028) },
		                { 0x008, new CMwFieldInfo("BASizeRatio", 0x0000001F) },
		                { 0x009, new CMwEnumInfo("BlurMethod", new string[] { "FastBilinear", "Gaussian" }) },
		                { 0x00A, new CMwFieldInfo("BlurSize", 0x0000001F) },
		                { 0x00B, new CMwFieldInfo("BlurRatio", 0x00000024) },
		                { 0x00C, new CMwFieldInfo("BlurAmp", 0x00000024) },
		                { 0x00D, new CMwMethodInfo("NewRandomSeed", 0x00000000, null, null) },
		                { 0x00E, new CMwFieldInfo("Arc0RGB", 0x00000009) },
		                { 0x00F, new CMwFieldInfo("Arc1RGB", 0x00000009) },
		                { 0x010, new CMwFieldInfo("Arc2RGB", 0x00000009) },
		                { 0x011, new CMwFieldInfo("Arc3RGB", 0x00000009) },
		                { 0x012, new CMwFieldInfo("ArcMaxStartPts", 0x0000001F) },
		                { 0x013, new CMwFieldInfo("ArcStepFreq", 0x00000028) },
		                { 0x014, new CMwFieldInfo("ArcStepSize", 0x00000024) },
		                { 0x015, new CMwFieldInfo("ArcStepAngle", 0x00000028) },
		                { 0x016, new CMwFieldInfo("ArcStepILoss", 0x00000028) },
		                { 0x017, new CMwFieldInfo("ArcStepAttract", 0x00000028) },
		                { 0x018, new CMwFieldInfo("ArcStepKill", 0x00000028) },
		                { 0x019, new CMwFieldInfo("ArcDivideStep", 0x0000001F) },
		                { 0x01A, new CMwFieldInfo("ArcDivideAngle", 0x00000028) },
		                { 0x01B, new CMwFieldInfo("ArcDivideILoss", 0x00000028) },
		                { 0x01C, new CMwEnumInfo("BranchArcBlendSrc", new string[] { "0", "1", "SrcColor", "1-SrcColor", "SrcAlpha", "1-SrcAlpha", "DstColor", "1-DstColor", "DstAlpha", "1-DstAlpha", "SrcAlphaSat", "Constant", "1-Constant" }) },
		                { 0x01D, new CMwEnumInfo("BranchArcBlendDst", new string[] { "0", "1", "SrcColor", "1-SrcColor", "SrcAlpha", "1-SrcAlpha", "DstColor", "1-DstColor", "DstAlpha", "1-DstAlpha", "SrcAlphaSat", "Constant", "1-Constant" }) }
	                  })
	                },
	                { 0x074, new CMwClassInfo("CSceneFxOverlay", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("Shader", 0x00000005) }
	                  })
	                },
	                { 0x076, new CMwClassInfo("CSceneFxCompo", new Dictionary<int, CMwMemberInfo>()
	                  {
	                  })
	                },
	                { 0x077, new CMwClassInfo("CSceneFxDepthOfField", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwEnumInfo("Technique", new string[] { "BlendLayersZ", "BlurAtDepth(NV30)", "MultiRT" }) },
		                { 0x001, new CMwFieldInfo("ForceFocus", 0x00000001) },
		                { 0x002, new CMwFieldInfo("FocusZ", 0x00000028) },
		                { 0x003, new CMwFieldInfo("LensSize", 0x00000024) },
		                { 0x004, new CMwFieldInfo("FocusRange", 0x00000028) },
		                { 0x005, new CMwFieldInfo("Intensity", 0x00000028) },
		                { 0x006, new CMwFieldInfo("TexSizeN", 0x00000023) },
		                { 0x007, new CMwFieldInfo("BlurSizeN", 0x00000023) },
		                { 0x008, new CMwFieldInfo("TexSizeF", 0x00000023) },
		                { 0x009, new CMwFieldInfo("BlurSizeF", 0x00000023) },
		                { 0x00A, new CMwEnumInfo("BlendLayer", new string[] { "Stencil", "Alpha" }) },
		                { 0x00B, new CMwFieldInfo("OverlapF", 0x00000028) },
		                { 0x00C, new CMwFieldInfo("IsAlphaInHomo", 0x00000001) },
		                { 0x00D, new CMwFieldInfo("BlurRatio", 0x00000028) },
		                { 0x00E, new CMwFieldInfo("BlurAmp", 0x00000028) },
		                { 0x00F, new CMwFieldInfo("BlurSize", 0x00000023) },
		                { 0x010, new CMwFieldInfo("BlurIs2PassHV", 0x00000001) },
		                { 0x011, new CMwFieldInfo("FidMrtDOF_BlurAtDepth", 0x00000005) }
	                  })
	                },
	                { 0x078, new CMwClassInfo("CSceneFxMotionBlur", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwEnumInfo("Technique", new string[] { "BlendFrames", "AccumFrames", "ExtraRT", "ExtraRT LengthCmp" }) },
		                { 0x001, new CMwFieldInfo("FrameCount", 0x00000023) },
		                { 0x002, new CMwFieldInfo("DurationMs", 0x0000001F) },
		                { 0x003, new CMwFieldInfo("RecurLoss", 0x00000028) },
		                { 0x004, new CMwFieldInfo("MotionScale", 0x00000024) },
		                { 0x005, new CMwFieldInfo("MotionMaxLen", 0x00000024) },
		                { 0x006, new CMwFieldInfo("DelayShadows", 0x00000001) },
		                { 0x007, new CMwFieldInfo("FidERT_PHlsl_MotionLenCmp", 0x00000005) }
	                  })
	                },
	                { 0x079, new CMwClassInfo("CSceneFxCameraBlend", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("BlendFactor", 0x00000028) }
	                  })
	                },
	                { 0x07A, new CMwClassInfo("CSceneFxGrayAccum", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("GrayFactor", 0x00000028) },
		                { 0x001, new CMwFieldInfo("GrayRGB", 0x00000009) },
		                { 0x002, new CMwFieldInfo("GlowNewG", 0x00000028) },
		                { 0x003, new CMwFieldInfo("GlowGain", 0x00000028) },
		                { 0x004, new CMwFieldInfo("GlowRecur", 0x00000028) },
		                { 0x005, new CMwFieldInfo("BASizeRatio", 0x0000001F) },
		                { 0x006, new CMwEnumInfo("BlurMethod", new string[] { "FastBilinear", "Gaussian" }) }
	                  })
	                },
	                { 0x07B, new CMwClassInfo("CSceneFxDistor2d", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("BitmapDistor", 0x00000005) },
		                { 0x001, new CMwFieldInfo("DistorScale", 0x00000024) }
	                  })
	                },
	                { 0x07C, new CMwClassInfo("", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("", 0x00000024) },
		                { 0x001, new CMwFieldInfo("", 0x00000024) },
		                { 0x002, new CMwFieldInfo("", 0x00000024) },
		                { 0x003, new CMwFieldInfo("", 0x00000024) },
		                { 0x004, new CMwFieldInfo("", 0x00000024) },
		                { 0x005, new CMwFieldInfo("", 0x00000024) },
		                { 0x006, new CMwFieldInfo("", 0x00000024) },
		                { 0x007, new CMwFieldInfo("", 0x00000024) },
		                { 0x008, new CMwEnumInfo("", new string[] { "Simple", "Charged" }) },
		                { 0x009, new CMwFieldInfo("", 0x00000024) },
		                { 0x00A, new CMwFieldInfo("", 0x00000024) },
		                { 0x00B, new CMwFieldInfo("", 0x0000001F) },
		                { 0x00C, new CMwFieldInfo("", 0x0000001F) },
		                { 0x00D, new CMwEnumInfo("", new string[] { "None", "Acceleration" }) },
		                { 0x00E, new CMwFieldInfo("", 0x0000001F) },
		                { 0x00F, new CMwFieldInfo("", 0x00000024) },
		                { 0x010, new CMwFieldInfo("", 0x00000024) }
	                  })
	                },
	                { 0x07D, new CMwClassInfo("", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("", 0x00000029) },
		                { 0x001, new CMwFieldInfo("", 0x00000007) }
	                  })
	                },
	                { 0x07E, new CMwClassInfo("CSceneFxOccZCmp", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("UsePointsInSphere", 0x00000001) },
		                { 0x001, new CMwFieldInfo("ImageRadius", 0x00000028) },
		                { 0x002, new CMwFieldInfo("BlurTexelCount", 0x0000001F) },
		                { 0x003, new CMwFieldInfo("Shader", 0x00000005) },
		                { 0x004, new CMwFieldInfo("PointsInSphere", 0x00000005) }
	                  })
	                },
	                { 0x080, new CMwClassInfo("", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("", 0x00000024) },
		                { 0x001, new CMwFieldInfo("", 0x00000024) },
		                { 0x002, new CMwFieldInfo("", 0x00000024) },
		                { 0x003, new CMwFieldInfo("", 0x00000024) },
		                { 0x004, new CMwFieldInfo("", 0x00000024) },
		                { 0x005, new CMwFieldInfo("", 0x00000024) },
		                { 0x006, new CMwFieldInfo("", 0x00000024) },
		                { 0x007, new CMwFieldInfo("", 0x00000024) },
		                { 0x008, new CMwFieldInfo("", 0x00000024) },
		                { 0x009, new CMwFieldInfo("", 0x00000024) },
		                { 0x00A, new CMwFieldInfo("", 0x00000024) },
		                { 0x00B, new CMwFieldInfo("", 0x00000024) },
		                { 0x00C, new CMwFieldInfo("", 0x00000024) },
		                { 0x00D, new CMwFieldInfo("", 0x00000024) },
		                { 0x00E, new CMwFieldInfo("", 0x00000024) }
	                  })
	                },
	                { 0x090, new CMwClassInfo("CSceneToyTrain", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("Path", 0x00000005) },
		                { 0x001, new CMwFieldInfo("Wagon", 0x00000005) },
		                { 0x002, new CMwFieldInfo("Loco", 0x00000005) },
		                { 0x003, new CMwFieldInfo("WheelPlacement", 0x00000028) },
		                { 0x004, new CMwFieldInfo("DistanceBetweenCars", 0x00000024) },
		                { 0x005, new CMwFieldInfo("DistanceLocoWagon", 0x00000024) },
		                { 0x006, new CMwFieldInfo("NbCars", 0x0000001F) },
		                { 0x007, new CMwFieldInfo("UseLocoAsTail", 0x00000001) },
		                { 0x008, new CMwFieldInfo("Speed", 0x00000024) },
		                { 0x009, new CMwFieldInfo("FrictionBack", 0x00000024) },
		                { 0x00A, new CMwFieldInfo("FrictionFront", 0x00000024) },
		                { 0x00B, new CMwFieldInfo("Attraction", 0x00000024) },
		                { 0x00C, new CMwFieldInfo("CurrentX", 0x00000024) },
		                { 0x00D, new CMwFieldInfo("ContactCarNumber", 0x0000001F) },
		                { 0x00E, new CMwFieldInfo("ContactCarSpeed", 0x00000035) }
	                  })
	                },
	                { 0x100, new CMwClassInfo("CSceneToyBoat", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("HdgDeg", 0x00000024) },
		                { 0x001, new CMwFieldInfo("BSKnot", 0x00000024) },
		                { 0x002, new CMwFieldInfo("BSAvgKnot", 0x00000024) },
		                { 0x003, new CMwFieldInfo("BSTheoricalKnot", 0x00000024) },
		                { 0x004, new CMwFieldInfo("TWDWorldDeg", 0x00000024) },
		                { 0x005, new CMwFieldInfo("TWSWorldKnot", 0x00000024) },
		                { 0x006, new CMwFieldInfo("TWDDeg", 0x00000024) },
		                { 0x007, new CMwFieldInfo("TWSKnot", 0x00000024) },
		                { 0x008, new CMwFieldInfo("TWADeg", 0x00000024) },
		                { 0x009, new CMwFieldInfo("AWADeg", 0x00000024) },
		                { 0x00A, new CMwFieldInfo("AWSKnot", 0x00000024) },
		                { 0x00B, new CMwFieldInfo("HeelDeg", 0x00000024) },
		                { 0x00C, new CMwFieldInfo("COGAvgDeg", 0x00000024) },
		                { 0x00D, new CMwFieldInfo("SOGAvgKnot", 0x00000024) },
		                { 0x00E, new CMwFieldInfo("StreamSpeedKnot", 0x00000024) },
		                { 0x00F, new CMwFieldInfo("StreamDirectionDeg", 0x00000024) },
		                { 0x010, new CMwMethodInfo("ResetLocation", 0x00000000, null, null) },
		                { 0x011, new CMwFieldInfo("Params", 0x00000005) },
		                { 0x012, new CMwFieldInfo("RealBoatWidth", 0x00000024) },
		                { 0x013, new CMwFieldInfo("RealBoatLength", 0x00000024) },
		                { 0x014, new CMwFieldInfo("WindShadow", 0x00000005) },
		                { 0x015, new CMwMethodInfo("RetrieveSounds", 0x00000000, null, null) },
		                { 0x016, new CMwMethodInfo("EdBoatCreate", 0x00000000, null, null) },
		                { 0x017, new CMwMethodInfo("ChangeSail", 0x00000000, null, null) },
		                { 0x018, new CMwFieldInfo("TillerTargetAngle", 0x00000028) },
		                { 0x019, new CMwFieldInfo("SplashEmitterModel", 0x00000005) },
		                { 0x01A, new CMwFieldInfo("TeamMatesVisible", 0x00000001) },
		                { 0x01B, new CMwFieldInfo("StemWavesVisible", 0x00000001) },
		                { 0x01C, new CMwFieldInfo("WindIndicatorVisible", 0x00000001) },
		                { 0x01D, new CMwFieldInfo("SailStates", 0x00000007) },
		                { 0x01E, new CMwFieldInfo("ContactForceFluidFrictionCoef", 0x00000024) },
		                { 0x01F, new CMwFieldInfo("ReplacementStepLength", 0x00000024) },
		                { 0x020, new CMwFieldInfo("ContactRotationImpulseCoef", 0x00000024) },
		                { 0x021, new CMwFieldInfo("ContactRelSpeedMultCoef", 0x00000024) },
		                { 0x022, new CMwFieldInfo("GamePlayCoef_BSLevel", 0x00000024) },
		                { 0x023, new CMwFieldInfo("GamePlayCoef_BSCatchBack", 0x00000024) },
		                { 0x024, new CMwFieldInfo("GamePlayCoef_BSSpi", 0x00000024) },
		                { 0x025, new CMwFieldInfo("GamePlayCoef_BSNoSpi", 0x00000024) },
		                { 0x026, new CMwFieldInfo("GamePlayCoef_BSGeneral", 0x00000024) },
		                { 0x027, new CMwFieldInfo("GamePlayCoef_BSManoeuvre", 0x00000024) }
	                  })
	                },
	                { 0x101, new CMwClassInfo("CSceneToySea", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("Houles", 0x00000005) },
		                { 0x001, new CMwFieldInfo("CoefMovTexture", 0x00000024) },
		                { 0x002, new CMwFieldInfo("MinTextureView", 0x00000024) },
		                { 0x003, new CMwFieldInfo("MaxTextureView", 0x00000024) },
		                { 0x004, new CMwFieldInfo("CoefMinView", 0x00000024) },
		                { 0x005, new CMwFieldInfo("Valtest1", 0x00000024) },
		                { 0x006, new CMwFieldInfo("Valtest2", 0x00000024) },
		                { 0x007, new CMwFieldInfo("HouleTable3", 0x00000005) },
		                { 0x008, new CMwFieldInfo("HouleTable4", 0x00000005) },
		                { 0x009, new CMwFieldInfo("HouleTable5", 0x00000005) },
		                { 0x00A, new CMwFieldInfo("HouleTable6", 0x00000005) },
		                { 0x00B, new CMwFieldInfo("HouleTable7", 0x00000005) },
		                { 0x00C, new CMwFieldInfo("UpdateGeomTimeMs", 0x00000024) }
	                  })
	                },
	                { 0x104, new CMwClassInfo("CSceneToyStem", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("IsActive", 0x00000001) },
		                { 0x001, new CMwFieldInfo("UseWaveProjection", 0x00000001) },
		                { 0x002, new CMwFieldInfo("UseSeaElevation", 0x00000001) },
		                { 0x003, new CMwFieldInfo("OnlyUnderSea", 0x00000001) },
		                { 0x004, new CMwFieldInfo("SeaLevel", 0x00000024) },
		                { 0x005, new CMwFieldInfo("Sea", 0x00000005) },
		                { 0x006, new CMwFieldInfo("NbPointZ", 0x0000001F) },
		                { 0x007, new CMwFieldInfo("Gravity", 0x00000035) },
		                { 0x008, new CMwFieldInfo("InitialSpeedDir", 0x00000035) },
		                { 0x009, new CMwFieldInfo("MaxCoefSize", 0x00000024) },
		                { 0x00A, new CMwFieldInfo("BoatSpeedRef", 0x00000024) },
		                { 0x00B, new CMwFieldInfo("EjectSpeedScale", 0x00000024) },
		                { 0x00C, new CMwFieldInfo("SplashCoef", 0x00000024) },
		                { 0x00D, new CMwFieldInfo("StemDuration", 0x0000001F) },
		                { 0x00E, new CMwFieldInfo("RelativePos", 0x00000035) },
		                { 0x00F, new CMwFieldInfo("UseRelativePos2", 0x00000001) },
		                { 0x010, new CMwFieldInfo("RelativePos2", 0x00000035) },
		                { 0x011, new CMwFieldInfo("StartWidth", 0x00000024) },
		                { 0x012, new CMwFieldInfo("EndWidth", 0x00000024) },
		                { 0x013, new CMwFieldInfo("TextureSizeInMeter", 0x00000024) },
		                { 0x014, new CMwFieldInfo("ValTest1", 0x00000024) },
		                { 0x015, new CMwFieldInfo("StemColor", 0x00000009) },
		                { 0x016, new CMwFieldInfo("MaterialStem", 0x00000005) }
	                  })
	                },
	                { 0x105, new CMwClassInfo("CBoatTeamDesc", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("TeamMateIds", 0x0000001D) },
		                { 0x001, new CMwFieldInfo("TeamActionDescs", 0x00000005) },
		                { 0x002, new CMwFieldInfo("TeamMateLocationDescs", 0x00000005) },
		                { 0x003, new CMwFieldInfo("TeamMateVisualAnims", 0x00000005) },
		                { 0x004, new CMwFieldInfo("AnimWalkId", 0x0000001B) },
		                { 0x005, new CMwMethodInfo("UpdateCacheData", 0x00000000, null, null) }
	                  })
	                },
	                { 0x106, new CMwClassInfo("CBoatTeamActionDesc", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("TeamActionId", 0x0000001B) },
		                { 0x001, new CMwFieldInfo("TeamMateActionDescs", 0x00000007) }
	                  })
	                },
	                { 0x107, new CMwClassInfo("CBoatTeamMateActionDesc", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("TeamMateId", 0x0000001B) },
		                { 0x001, new CMwFieldInfo("DestLocationId", 0x0000001B) },
		                { 0x002, new CMwFieldInfo("WalkSpeed", 0x00000024) },
		                { 0x003, new CMwFieldInfo("AnimBeforeId", 0x0000001B) },
		                { 0x004, new CMwFieldInfo("AnimAfterId", 0x0000001B) },
		                { 0x005, new CMwFieldInfo("AnimAfterIsLooping", 0x00000001) },
		                { 0x006, new CMwFieldInfo("AnimAfterIsTimeStop", 0x00000001) }
	                  })
	                },
	                { 0x108, new CMwClassInfo("CManoeuvre", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("Duration", 0x00000024) },
		                { 0x001, new CMwFieldInfo("SlowDownCoef", 0x00000024) },
		                { 0x002, new CMwFieldInfo("TeamActionId", 0x0000001B) },
		                { 0x003, new CMwFieldInfo("Sound1Id", 0x0000001B) },
		                { 0x004, new CMwFieldInfo("Sound1Time", 0x00000024) },
		                { 0x005, new CMwFieldInfo("Sound2Id", 0x0000001B) },
		                { 0x006, new CMwFieldInfo("Sound2Time", 0x00000024) },
		                { 0x007, new CMwFieldInfo("OldTeamActionIndex", 0x0000001F) }
	                  })
	                },
	                { 0x109, new CMwClassInfo("CBoatTeamMateLocationDesc", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("LocationId", 0x0000001B) },
		                { 0x001, new CMwFieldInfo("Translation", 0x00000035) },
		                { 0x002, new CMwFieldInfo("RotationYDeg", 0x00000024) }
	                  })
	                },
	                { 0x10A, new CMwClassInfo("CSceneToySeaHoule", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("UsedForAssiette", 0x00000001) },
		                { 0x001, new CMwFieldInfo("TextureCPU", 0x00000005) },
		                { 0x002, new CMwFieldInfo("RepresentedX", 0x00000024) },
		                { 0x003, new CMwFieldInfo("RepresentedZ", 0x00000024) },
		                { 0x004, new CMwFieldInfo("Amplitude", 0x00000024) },
		                { 0x005, new CMwFieldInfo("Speed", 0x00000024) },
		                { 0x006, new CMwFieldInfo("AngleWindDeltaInDegree", 0x00000028) },
		                { 0x007, new CMwFieldInfo("DistorFactorXZ", 0x00000028) },
		                { 0x008, new CMwFieldInfo("FoamMin", 0x00000028) },
		                { 0x009, new CMwFieldInfo("FoamScale", 0x00000028) }
	                  })
	                },
	                { 0x10B, new CMwClassInfo("CBoatSail", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwEnumInfo("SailType", new string[] { "MainSail", "Genoa", "Spi", "Code0", "StaySail", "SpiAsym", "None" }) },
		                { 0x001, new CMwFieldInfo("VppCurves", 0x00000005) },
		                { 0x002, new CMwFieldInfo("BSCoefCurves", 0x00000005) },
		                { 0x003, new CMwFieldInfo("OptimalSailAngleCurves", 0x00000005) },
		                { 0x004, new CMwFieldInfo("OptimalSailAngleCurvesArray", 0x00000007) },
		                { 0x005, new CMwFieldInfo("HeelAngleCurves", 0x00000005) },
		                { 0x006, new CMwFieldInfo("HeelAngleCoefCurves", 0x00000005) },
		                { 0x007, new CMwFieldInfo("LuffAngleSpeedCurves", 0x00000005) },
		                { 0x008, new CMwFieldInfo("RevolveAngleSpeedCurves", 0x00000005) },
		                { 0x009, new CMwFieldInfo("AccelerationCurves", 0x00000005) },
		                { 0x00A, new CMwFieldInfo("ShiverAngleCurve", 0x00000005) },
		                { 0x00B, new CMwFieldInfo("BoomAngleCurves", 0x00000005) },
		                { 0x00C, new CMwFieldInfo("SheetAngleMaxCurve", 0x00000005) },
		                { 0x00D, new CMwFieldInfo("SheetAngleMin", 0x00000024) },
		                { 0x00E, new CMwFieldInfo("SheetAngleMax", 0x00000024) },
		                { 0x00F, new CMwFieldInfo("SailSpeedCoefSheet", 0x00000024) },
		                { 0x010, new CMwFieldInfo("SailSpeedCoefEaseOut", 0x00000024) },
		                { 0x011, new CMwFieldInfo("AutomaticSheetTargetSpeed", 0x00000024) },
		                { 0x012, new CMwFieldInfo("ManoeuvreHaulUp", 0x00000005) },
		                { 0x013, new CMwFieldInfo("ManoeuvreHaulDown", 0x00000005) },
		                { 0x014, new CMwFieldInfo("ManoeuvrePrepareHaulUp", 0x00000005) },
		                { 0x015, new CMwFieldInfo("ManoeuvrePrepareHaulDown", 0x00000005) },
		                { 0x016, new CMwFieldInfo("BulgeIntens", 0x00000024) },
		                { 0x017, new CMwFieldInfo("BulgeSpeed", 0x00000024) },
		                { 0x018, new CMwFieldInfo("BoomSpeed", 0x00000024) },
		                { 0x019, new CMwFieldInfo("SailId", 0x00000029) },
		                { 0x01A, new CMwFieldInfo("SailFlatId", 0x00000029) },
		                { 0x01B, new CMwFieldInfo("BoomId", 0x00000029) },
		                { 0x01C, new CMwFieldInfo("IsReefingGear", 0x00000001) },
		                { 0x01D, new CMwFieldInfo("VisualHaulDelta", 0x00000024) },
		                { 0x01E, new CMwFieldInfo("VisualShiverAmplitude", 0x00000024) },
		                { 0x01F, new CMwFieldInfo("VisualShiverSpeedCoef", 0x00000024) },
		                { 0x020, new CMwFieldInfo("VisualShiverSpeedMax", 0x00000024) },
		                { 0x021, new CMwFieldInfo("ShiverWindFree", 0x00000001) },
		                { 0x022, new CMwFieldInfo("OldLuffBlockadeWindAngle", 0x00000024) },
		                { 0x023, new CMwFieldInfo("OldLuffBlockadeBsSlowDown", 0x00000024) },
		                { 0x024, new CMwFieldInfo("OldLuffLimitWindAngle", 0x00000024) },
		                { 0x025, new CMwFieldInfo("OldLuffLimitBsSlowDown", 0x00000024) },
		                { 0x026, new CMwFieldInfo("OldHeelCoef", 0x00000024) }
	                  })
	                },
	                { 0x10C, new CMwClassInfo("CSceneToySeaHouleTable", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("SeaBumpScale", 0x00000024) },
		                { 0x001, new CMwFieldInfo("SeaBumpScaleUV", 0x00000031) },
		                { 0x002, new CMwFieldInfo("FoamExponant", 0x00000024) },
		                { 0x003, new CMwFieldInfo("FoamScaleWorldXZ", 0x00000028) },
		                { 0x004, new CMwMethodInfo("AddHoule", 0x00000000, null, null) },
		                { 0x005, new CMwFieldInfo("Houles", 0x00000006) },
		                { 0x006, new CMwFieldInfo("FieldHouleUncompress", 0x00000001) },
		                { 0x007, new CMwFieldInfo("HouleGlobalScale", 0x00000024) }
	                  })
	                },
	                { 0x10D, new CMwClassInfo("CSceneToySeaHouleFixe", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("CoefFoam", 0x00000024) },
		                { 0x001, new CMwFieldInfo("ExposantFoam", 0x00000024) }
	                  })
	                },
	                { 0x10E, new CMwClassInfo("CBoatParam", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwEnumInfo("BoatType", new string[] { "MonoCoque", "Catamaran", "Trimaran" }) },
		                { 0x001, new CMwFieldInfo("BoatWidth", 0x00000024) },
		                { 0x002, new CMwFieldInfo("ExtraHullHeightDelta", 0x00000024) },
		                { 0x003, new CMwFieldInfo("DecelerationConstant", 0x00000024) },
		                { 0x004, new CMwFieldInfo("DecelerationDynamic", 0x00000024) },
		                { 0x005, new CMwFieldInfo("DecelerationFromTillerCurves", 0x00000005) },
		                { 0x006, new CMwFieldInfo("DecelerationFromTillerCurvesBearAway", 0x00000005) },
		                { 0x007, new CMwFieldInfo("RotationRadius", 0x00000024) },
		                { 0x008, new CMwFieldInfo("TillerAngleMax", 0x00000024) },
		                { 0x009, new CMwFieldInfo("TillerInertia", 0x00000024) },
		                { 0x00A, new CMwFieldInfo("BSCoefFromHeelCurve", 0x00000005) },
		                { 0x00B, new CMwFieldInfo("IsNewSailPhysics", 0x00000001) },
		                { 0x00C, new CMwFieldInfo("Sails", 0x00000007) },
		                { 0x00D, new CMwFieldInfo("WindShadow", 0x00000005) },
		                { 0x00E, new CMwFieldInfo("Surf", 0x00000024) },
		                { 0x00F, new CMwFieldInfo("Floating", 0x00000024) },
		                { 0x010, new CMwFieldInfo("WaterLineHeightDelta", 0x00000024) },
		                { 0x011, new CMwFieldInfo("RollCoef", 0x00000024) },
		                { 0x012, new CMwFieldInfo("PitchSpeed", 0x00000024) },
		                { 0x013, new CMwFieldInfo("PitchInertia", 0x00000024) },
		                { 0x014, new CMwFieldInfo("PitchOscillation", 0x00000024) },
		                { 0x015, new CMwFieldInfo("PitchOscillationSpeed", 0x00000024) },
		                { 0x016, new CMwFieldInfo("HeelSpeed", 0x00000024) },
		                { 0x017, new CMwFieldInfo("HeelInertia", 0x00000024) },
		                { 0x018, new CMwFieldInfo("HeelOscillation", 0x00000024) },
		                { 0x019, new CMwFieldInfo("HeelOscillationSpeed", 0x00000024) },
		                { 0x01A, new CMwFieldInfo("HeelMax", 0x00000024) },
		                { 0x01B, new CMwFieldInfo("IsNewSeaPhysics", 0x00000001) },
		                { 0x01C, new CMwFieldInfo("LateralForceFromLateralSpeedSq", 0x00000024) },
		                { 0x01D, new CMwFieldInfo("FloatingForceFromImmersionNormed", 0x00000024) },
		                { 0x01E, new CMwFieldInfo("FloatingForceFromImmersionNormedSq", 0x00000024) },
		                { 0x01F, new CMwFieldInfo("FloatingForceFullImmersionDist", 0x00000024) },
		                { 0x020, new CMwFieldInfo("Gravity", 0x00000024) },
		                { 0x021, new CMwFieldInfo("FluidFrictionY", 0x00000024) },
		                { 0x022, new CMwFieldInfo("FloatingDeltaY", 0x00000024) },
		                { 0x023, new CMwFieldInfo("DynamicAcceleration", 0x00000024) },
		                { 0x024, new CMwFieldInfo("FluidFrictionFront", 0x00000024) },
		                { 0x025, new CMwFieldInfo("CameraInsideE", 0x00000035) },
		                { 0x026, new CMwFieldInfo("CameraInsideB", 0x00000035) },
		                { 0x027, new CMwFieldInfo("CameraInsideE_Amplifior", 0x00000035) },
		                { 0x028, new CMwFieldInfo("WindIndicatorDistance", 0x00000035) },
		                { 0x029, new CMwFieldInfo("TeamMateModelFid", 0x00000005) },
		                { 0x02A, new CMwFieldInfo("TeamMateTreeToFollowId", 0x0000001B) },
		                { 0x02B, new CMwFieldInfo("m_CollisionSoundBoatSpeedDeltaThreshold", 0x00000024) },
		                { 0x02C, new CMwFieldInfo("StemWaves", 0x00000007) },
		                { 0x02D, new CMwFieldInfo("TeamDesc", 0x00000005) },
		                { 0x02E, new CMwFieldInfo("OldAccelerationConstant", 0x00000024) },
		                { 0x02F, new CMwFieldInfo("OldWreckAngle", 0x00000024) },
		                { 0x030, new CMwFieldInfo("OldHeelLuff", 0x00000024) },
		                { 0x031, new CMwFieldInfo("OldHeelCoefCurve", 0x00000005) },
		                { 0x032, new CMwFieldInfo("OldAnimList", 0x00000005) },
		                { 0x033, new CMwFieldInfo("OldFuncPathMesh", 0x00000005) },
		                { 0x034, new CMwFieldInfo("OldTeamManagerModel", 0x00000005) }
	                  })
	                },
	                { 0x401, new CMwClassInfo("CSceneToyCharacter", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwMethodInfo("UpdateParamsFromTuning", 0x00000000, null, null) },
		                { 0x001, new CMwFieldInfo("Desc", 0x00000005) },
		                { 0x002, new CMwFieldInfo("CurAnim", 0x0000001B) },
		                { 0x003, new CMwFieldInfo("Tunings", 0x00000005) },
		                { 0x004, new CMwFieldInfo("IsOnGround", 0x00000001) },
		                { 0x005, new CMwFieldInfo("IsInWater", 0x00000001) },
		                { 0x006, new CMwFieldInfo("BonusJumpMultiplier", 0x00000024) },
		                { 0x007, new CMwFieldInfo("BonusSpeedOnGroundMultiplier", 0x00000024) },
		                { 0x008, new CMwFieldInfo("BonusSpeedInAirMultiplier", 0x00000024) },
		                { 0x009, new CMwFieldInfo("BonusAirControlTimeMultiplier", 0x00000024) }
	                  })
	                }
                  })
                },
                { 0x0B, new CMwEngineInfo("System", new Dictionary<int, CMwClassInfo>()
                  {
	                { 0x000, new CMwClassInfo("CSystemEngine", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwMethodInfo("SynchUpdate", 0x00000000, null, null) },
		                { 0x001, new CMwFieldInfo("AnalyzerHasReadDuringPreviousFrame", 0x00000001) },
		                { 0x002, new CMwFieldInfo("AnalyzerHasWrittenDuringPreviousFrame", 0x00000001) }
	                  })
	                },
	                { 0x001, new CMwClassInfo("CSystemMouse", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("CoordX", 0x00000028) },
		                { 0x001, new CMwFieldInfo("CoordY", 0x00000028) },
		                { 0x002, new CMwFieldInfo("IsDownLeft", 0x00000001) },
		                { 0x003, new CMwFieldInfo("IsDownMiddle", 0x00000001) },
		                { 0x004, new CMwFieldInfo("IsDownRight", 0x00000001) },
		                { 0x005, new CMwFieldInfo("DeltaWheel", 0x00000024) },
		                { 0x006, new CMwFieldInfo("TotalWheel", 0x00000024) }
	                  })
	                },
	                { 0x002, new CMwClassInfo("CSystemKeyboard", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("IsDownEnter", 0x00000001) },
		                { 0x001, new CMwFieldInfo("IsStrokeEnter", 0x00000001) },
		                { 0x002, new CMwFieldInfo("IsDownSpace", 0x00000001) },
		                { 0x003, new CMwFieldInfo("IsStrokeSpace", 0x00000001) },
		                { 0x004, new CMwFieldInfo("IsDownShift", 0x00000001) },
		                { 0x005, new CMwFieldInfo("IsStrokeShift", 0x00000001) },
		                { 0x006, new CMwFieldInfo("IsDownEscape", 0x00000001) },
		                { 0x007, new CMwFieldInfo("IsStrokeEscape", 0x00000001) },
		                { 0x008, new CMwFieldInfo("IsDownUp", 0x00000001) },
		                { 0x009, new CMwFieldInfo("IsStrokeUp", 0x00000001) },
		                { 0x00A, new CMwFieldInfo("IsDownRight", 0x00000001) },
		                { 0x00B, new CMwFieldInfo("IsStrokeRight", 0x00000001) },
		                { 0x00C, new CMwFieldInfo("IsDownLeft", 0x00000001) },
		                { 0x00D, new CMwFieldInfo("IsStrokeLeft", 0x00000001) },
		                { 0x00E, new CMwFieldInfo("IsDownDown", 0x00000001) },
		                { 0x00F, new CMwFieldInfo("IsStrokeDown", 0x00000001) },
		                { 0x010, new CMwFieldInfo("IsDownControl", 0x00000001) },
		                { 0x011, new CMwFieldInfo("IsStrokeControl", 0x00000001) },
		                { 0x012, new CMwFieldInfo("IsDownAlt", 0x00000001) },
		                { 0x013, new CMwFieldInfo("IsStrokeAlt", 0x00000001) },
		                { 0x014, new CMwFieldInfo("IsDownPageUp", 0x00000001) },
		                { 0x015, new CMwFieldInfo("IsStrokePageUp", 0x00000001) },
		                { 0x016, new CMwFieldInfo("IsDownPageDown", 0x00000001) },
		                { 0x017, new CMwFieldInfo("IsStrokePageDown", 0x00000001) },
		                { 0x018, new CMwFieldInfo("IsDownTab", 0x00000001) },
		                { 0x019, new CMwFieldInfo("IsStrokeTab", 0x00000001) },
		                { 0x01A, new CMwFieldInfo("IsDownNumpad0", 0x00000001) },
		                { 0x01B, new CMwFieldInfo("IsStrokeNumpad0", 0x00000001) },
		                { 0x01C, new CMwFieldInfo("IsDownNumpad1", 0x00000001) },
		                { 0x01D, new CMwFieldInfo("IsStrokeNumpad1", 0x00000001) },
		                { 0x01E, new CMwFieldInfo("IsDownNumpad2", 0x00000001) },
		                { 0x01F, new CMwFieldInfo("IsStrokeNumpad2", 0x00000001) },
		                { 0x020, new CMwFieldInfo("IsDownNumpad3", 0x00000001) },
		                { 0x021, new CMwFieldInfo("IsStrokeNumpad3", 0x00000001) },
		                { 0x022, new CMwFieldInfo("IsDownNumpad4", 0x00000001) },
		                { 0x023, new CMwFieldInfo("IsStrokeNumpad4", 0x00000001) },
		                { 0x024, new CMwFieldInfo("IsDownNumpad5", 0x00000001) },
		                { 0x025, new CMwFieldInfo("IsStrokeNumpad5", 0x00000001) },
		                { 0x026, new CMwFieldInfo("IsDownNumpad6", 0x00000001) },
		                { 0x027, new CMwFieldInfo("IsStrokeNumpad6", 0x00000001) },
		                { 0x028, new CMwFieldInfo("IsDownNumpad7", 0x00000001) },
		                { 0x029, new CMwFieldInfo("IsStrokeNumpad7", 0x00000001) },
		                { 0x02A, new CMwFieldInfo("IsDownNumpad8", 0x00000001) },
		                { 0x02B, new CMwFieldInfo("IsStrokeNumpad8", 0x00000001) },
		                { 0x02C, new CMwFieldInfo("IsDownNumpad9", 0x00000001) },
		                { 0x02D, new CMwFieldInfo("IsStrokeNumpad9", 0x00000001) },
		                { 0x02E, new CMwFieldInfo("IsDownAdd", 0x00000001) },
		                { 0x02F, new CMwFieldInfo("IsStrokeAdd", 0x00000001) },
		                { 0x030, new CMwFieldInfo("IsDownSub", 0x00000001) },
		                { 0x031, new CMwFieldInfo("IsStrokeSub", 0x00000001) },
		                { 0x032, new CMwFieldInfo("IsDownMul", 0x00000001) },
		                { 0x033, new CMwFieldInfo("IsStrokeMul", 0x00000001) },
		                { 0x034, new CMwFieldInfo("IsDownDiv", 0x00000001) },
		                { 0x035, new CMwFieldInfo("IsStrokeDiv", 0x00000001) },
		                { 0x036, new CMwFieldInfo("IsDownDecimal", 0x00000001) },
		                { 0x037, new CMwFieldInfo("IsStrokeDecimal", 0x00000001) },
		                { 0x038, new CMwFieldInfo("IsDownA", 0x00000001) },
		                { 0x039, new CMwFieldInfo("IsStrokeA", 0x00000001) },
		                { 0x03A, new CMwFieldInfo("IsDownB", 0x00000001) },
		                { 0x03B, new CMwFieldInfo("IsStrokeB", 0x00000001) },
		                { 0x03C, new CMwFieldInfo("IsDownC", 0x00000001) },
		                { 0x03D, new CMwFieldInfo("IsStrokeC", 0x00000001) },
		                { 0x03E, new CMwFieldInfo("IsDownD", 0x00000001) },
		                { 0x03F, new CMwFieldInfo("IsStrokeD", 0x00000001) },
		                { 0x040, new CMwFieldInfo("IsDownE", 0x00000001) },
		                { 0x041, new CMwFieldInfo("IsStrokeE", 0x00000001) },
		                { 0x042, new CMwFieldInfo("IsDownF", 0x00000001) },
		                { 0x043, new CMwFieldInfo("IsStrokeF", 0x00000001) },
		                { 0x044, new CMwFieldInfo("IsDownG", 0x00000001) },
		                { 0x045, new CMwFieldInfo("IsStrokeG", 0x00000001) },
		                { 0x046, new CMwFieldInfo("IsDownH", 0x00000001) },
		                { 0x047, new CMwFieldInfo("IsStrokeH", 0x00000001) },
		                { 0x048, new CMwFieldInfo("IsDownI", 0x00000001) },
		                { 0x049, new CMwFieldInfo("IsStrokeI", 0x00000001) },
		                { 0x04A, new CMwFieldInfo("IsDownJ", 0x00000001) },
		                { 0x04B, new CMwFieldInfo("IsStrokeJ", 0x00000001) },
		                { 0x04C, new CMwFieldInfo("IsDownK", 0x00000001) },
		                { 0x04D, new CMwFieldInfo("IsStrokeK", 0x00000001) },
		                { 0x04E, new CMwFieldInfo("IsDownL", 0x00000001) },
		                { 0x04F, new CMwFieldInfo("IsStrokeL", 0x00000001) },
		                { 0x050, new CMwFieldInfo("IsDownM", 0x00000001) },
		                { 0x051, new CMwFieldInfo("IsStrokeM", 0x00000001) },
		                { 0x052, new CMwFieldInfo("IsDownN", 0x00000001) },
		                { 0x053, new CMwFieldInfo("IsStrokeN", 0x00000001) },
		                { 0x054, new CMwFieldInfo("IsDownO", 0x00000001) },
		                { 0x055, new CMwFieldInfo("IsStrokeO", 0x00000001) },
		                { 0x056, new CMwFieldInfo("IsDownP", 0x00000001) },
		                { 0x057, new CMwFieldInfo("IsStrokeP", 0x00000001) },
		                { 0x058, new CMwFieldInfo("IsDownQ", 0x00000001) },
		                { 0x059, new CMwFieldInfo("IsStrokeQ", 0x00000001) },
		                { 0x05A, new CMwFieldInfo("IsDownR", 0x00000001) },
		                { 0x05B, new CMwFieldInfo("IsStrokeR", 0x00000001) },
		                { 0x05C, new CMwFieldInfo("IsDownS", 0x00000001) },
		                { 0x05D, new CMwFieldInfo("IsStrokeS", 0x00000001) },
		                { 0x05E, new CMwFieldInfo("IsDownT", 0x00000001) },
		                { 0x05F, new CMwFieldInfo("IsStrokeT", 0x00000001) },
		                { 0x060, new CMwFieldInfo("IsDownU", 0x00000001) },
		                { 0x061, new CMwFieldInfo("IsStrokeU", 0x00000001) },
		                { 0x062, new CMwFieldInfo("IsDownV", 0x00000001) },
		                { 0x063, new CMwFieldInfo("IsStrokeV", 0x00000001) },
		                { 0x064, new CMwFieldInfo("IsDownW", 0x00000001) },
		                { 0x065, new CMwFieldInfo("IsStrokeW", 0x00000001) },
		                { 0x066, new CMwFieldInfo("IsDownX", 0x00000001) },
		                { 0x067, new CMwFieldInfo("IsStrokeX", 0x00000001) },
		                { 0x068, new CMwFieldInfo("IsDownY", 0x00000001) },
		                { 0x069, new CMwFieldInfo("IsStrokeY", 0x00000001) },
		                { 0x06A, new CMwFieldInfo("IsDownZ", 0x00000001) },
		                { 0x06B, new CMwFieldInfo("IsStrokeZ", 0x00000001) },
		                { 0x06C, new CMwFieldInfo("IsDownF1", 0x00000001) },
		                { 0x06D, new CMwFieldInfo("IsStrokeF1", 0x00000001) },
		                { 0x06E, new CMwFieldInfo("IsDownF2", 0x00000001) },
		                { 0x06F, new CMwFieldInfo("IsStrokeF2", 0x00000001) },
		                { 0x070, new CMwFieldInfo("IsDownF3", 0x00000001) },
		                { 0x071, new CMwFieldInfo("IsStrokeF3", 0x00000001) },
		                { 0x072, new CMwFieldInfo("IsDownF4", 0x00000001) },
		                { 0x073, new CMwFieldInfo("IsStrokeF4", 0x00000001) },
		                { 0x074, new CMwFieldInfo("IsDownF5", 0x00000001) },
		                { 0x075, new CMwFieldInfo("IsStrokeF5", 0x00000001) },
		                { 0x076, new CMwFieldInfo("IsDownF6", 0x00000001) },
		                { 0x077, new CMwFieldInfo("IsStrokeF6", 0x00000001) },
		                { 0x078, new CMwFieldInfo("IsDownF7", 0x00000001) },
		                { 0x079, new CMwFieldInfo("IsStrokeF7", 0x00000001) },
		                { 0x07A, new CMwFieldInfo("IsDownF8", 0x00000001) },
		                { 0x07B, new CMwFieldInfo("IsStrokeF8", 0x00000001) },
		                { 0x07C, new CMwFieldInfo("IsDownF9", 0x00000001) },
		                { 0x07D, new CMwFieldInfo("IsStrokeF9", 0x00000001) },
		                { 0x07E, new CMwFieldInfo("IsDownF10", 0x00000001) },
		                { 0x07F, new CMwFieldInfo("IsStrokeF10", 0x00000001) },
		                { 0x080, new CMwFieldInfo("IsDownF11", 0x00000001) },
		                { 0x081, new CMwFieldInfo("IsStrokeF11", 0x00000001) },
		                { 0x082, new CMwFieldInfo("IsDownF12", 0x00000001) },
		                { 0x083, new CMwFieldInfo("IsStrokeF12", 0x00000001) },
		                { 0x084, new CMwFieldInfo("IsDown0", 0x00000001) },
		                { 0x085, new CMwFieldInfo("IsStroke0", 0x00000001) },
		                { 0x086, new CMwFieldInfo("IsDown1", 0x00000001) },
		                { 0x087, new CMwFieldInfo("IsStroke1", 0x00000001) },
		                { 0x088, new CMwFieldInfo("IsDown2", 0x00000001) },
		                { 0x089, new CMwFieldInfo("IsStroke2", 0x00000001) },
		                { 0x08A, new CMwFieldInfo("IsDown3", 0x00000001) },
		                { 0x08B, new CMwFieldInfo("IsStroke3", 0x00000001) },
		                { 0x08C, new CMwFieldInfo("IsDown4", 0x00000001) },
		                { 0x08D, new CMwFieldInfo("IsStroke4", 0x00000001) },
		                { 0x08E, new CMwFieldInfo("IsDown5", 0x00000001) },
		                { 0x08F, new CMwFieldInfo("IsStroke5", 0x00000001) },
		                { 0x090, new CMwFieldInfo("IsDown6", 0x00000001) },
		                { 0x091, new CMwFieldInfo("IsStroke6", 0x00000001) },
		                { 0x092, new CMwFieldInfo("IsDown7", 0x00000001) },
		                { 0x093, new CMwFieldInfo("IsStroke7", 0x00000001) },
		                { 0x094, new CMwFieldInfo("IsDown8", 0x00000001) },
		                { 0x095, new CMwFieldInfo("IsStroke8", 0x00000001) },
		                { 0x096, new CMwFieldInfo("IsDown9", 0x00000001) },
		                { 0x097, new CMwFieldInfo("IsStroke9", 0x00000001) },
		                { 0x098, new CMwFieldInfo("IsDownBackspace", 0x00000001) },
		                { 0x099, new CMwFieldInfo("IsStrokeBackspace", 0x00000001) },
		                { 0x09A, new CMwFieldInfo("IsDownDelete", 0x00000001) },
		                { 0x09B, new CMwFieldInfo("IsStrokeDelete", 0x00000001) },
		                { 0x09C, new CMwFieldInfo("IsDownHome", 0x00000001) },
		                { 0x09D, new CMwFieldInfo("IsStrokeHome", 0x00000001) },
		                { 0x09E, new CMwFieldInfo("IsDownEnd", 0x00000001) },
		                { 0x09F, new CMwFieldInfo("IsStrokeEnd", 0x00000001) },
		                { 0x0A0, new CMwFieldInfo("IsDownInsert", 0x00000001) },
		                { 0x0A1, new CMwFieldInfo("IsStrokeInsert", 0x00000001) },
		                { 0x0A2, new CMwFieldInfo("IsDownCompare", 0x00000001) },
		                { 0x0A3, new CMwFieldInfo("IsStrokeCompare", 0x00000001) },
		                { 0x0A4, new CMwFieldInfo("IsDownLeftPar", 0x00000001) },
		                { 0x0A5, new CMwFieldInfo("IsStrokeLeftPar", 0x00000001) },
		                { 0x0A6, new CMwFieldInfo("IsDownEqual", 0x00000001) },
		                { 0x0A7, new CMwFieldInfo("IsStrokeEqual", 0x00000001) },
		                { 0x0A8, new CMwFieldInfo("IsDownCirc", 0x00000001) },
		                { 0x0A9, new CMwFieldInfo("IsStrokeCirc", 0x00000001) },
		                { 0x0AA, new CMwFieldInfo("IsDownDollar", 0x00000001) },
		                { 0x0AB, new CMwFieldInfo("IsStrokeDollar", 0x00000001) },
		                { 0x0AC, new CMwFieldInfo("IsDownPercent", 0x00000001) },
		                { 0x0AD, new CMwFieldInfo("IsStrokePercent", 0x00000001) },
		                { 0x0AE, new CMwFieldInfo("IsDownMuMult", 0x00000001) },
		                { 0x0AF, new CMwFieldInfo("IsStrokeMuMult", 0x00000001) },
		                { 0x0B0, new CMwFieldInfo("IsDownComma", 0x00000001) },
		                { 0x0B1, new CMwFieldInfo("IsStrokeComma", 0x00000001) },
		                { 0x0B2, new CMwFieldInfo("IsDownSemicolon", 0x00000001) },
		                { 0x0B3, new CMwFieldInfo("IsStrokeSemicolon", 0x00000001) },
		                { 0x0B4, new CMwFieldInfo("IsDownTwoPoint", 0x00000001) },
		                { 0x0B5, new CMwFieldInfo("IsStrokeTwoPoint", 0x00000001) },
		                { 0x0B6, new CMwFieldInfo("IsDownExclamation", 0x00000001) },
		                { 0x0B7, new CMwFieldInfo("IsStrokeExclamation", 0x00000001) },
		                { 0x0B8, new CMwFieldInfo("IsDownSquare", 0x00000001) },
		                { 0x0B9, new CMwFieldInfo("IsStrokeSquare", 0x00000001) },
		                { 0x0BA, new CMwFieldInfo("IsOnNumlock", 0x00000001) },
		                { 0x0BB, new CMwFieldInfo("IsOnCapslock", 0x00000001) }
	                  })
	                },
	                { 0x003, new CMwClassInfo("CSystemWindow", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("SizeX", 0x00000023) },
		                { 0x001, new CMwFieldInfo("SizeY", 0x00000023) },
		                { 0x002, new CMwFieldInfo("PosX", 0x00000023) },
		                { 0x003, new CMwFieldInfo("PosY", 0x00000023) },
		                { 0x004, new CMwFieldInfo("Ratio", 0x00000024) },
		                { 0x005, new CMwFieldInfo("HasSizeChanged", 0x00000001) },
		                { 0x006, new CMwFieldInfo("HasPosChanged", 0x00000001) },
		                { 0x007, new CMwFieldInfo("StatusString", 0x00000029) }
	                  })
	                },
	                { 0x005, new CMwClassInfo("CSystemConfig", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("IsSafeMode", 0x00000001) },
		                { 0x001, new CMwFieldInfo("Display", 0x00000005) },
		                { 0x002, new CMwFieldInfo("DisplaySafe", 0x00000005) },
		                { 0x003, new CMwFieldInfo("NetworkUseProxy", 0x00000001) },
		                { 0x004, new CMwFieldInfo("NetworkProxyLogin", 0x00000029) },
		                { 0x005, new CMwFieldInfo("NetworkProxyPassword", 0x00000029) },
		                { 0x006, new CMwFieldInfo("NetworkServerPort", 0x0000001F) },
		                { 0x007, new CMwFieldInfo("NetworkP2PServerPort", 0x0000001F) },
		                { 0x008, new CMwFieldInfo("NetworkClientPort", 0x0000001F) },
		                { 0x009, new CMwFieldInfo("NetworkDownloadRate", 0x0000001F) },
		                { 0x00A, new CMwFieldInfo("NetworkUploadRate", 0x0000001F) },
		                { 0x00B, new CMwFieldInfo("NetworkForceUseLocalAddress", 0x00000001) },
		                { 0x00C, new CMwFieldInfo("NetworkForceServerAddress", 0x00000029) },
		                { 0x00D, new CMwFieldInfo("NetworkServerBroadcastLength", 0x0000001F) },
		                { 0x00E, new CMwFieldInfo("NetworkUseNatUPnP", 0x00000001) },
		                { 0x00F, new CMwFieldInfo("FileTransferEnableDownload", 0x00000001) },
		                { 0x010, new CMwFieldInfo("FileTransferEnableUpload", 0x00000001) },
		                { 0x011, new CMwFieldInfo("FileTransferMaxCacheSize", 0x0000001F) },
		                { 0x012, new CMwFieldInfo("EnableLocators", 0x00000001) },
		                { 0x013, new CMwFieldInfo("AutoUpdateFromLocator", 0x00000001) },
		                { 0x014, new CMwFieldInfo("AutoUpdateLocatorDBUrl", 0x00000029) },
		                { 0x015, new CMwFieldInfo("MenuSkin", 0x0000002D) },
		                { 0x016, new CMwFieldInfo("IsIgnorePlayerSkins", 0x00000001) },
		                { 0x017, new CMwFieldInfo("IsSkipRollingDemo", 0x00000001) },
		                { 0x018, new CMwFieldInfo("ProfileEnableMulti", 0x00000001) },
		                { 0x019, new CMwFieldInfo("ProfileName", 0x00000029) },
		                { 0x01A, new CMwMethodInfo("SetProfileNameUnassigned", 0x00000000, null, null) },
		                { 0x01B, new CMwFieldInfo("PlayerInfoDisplaySize", 0x00000023) },
		                { 0x01C, new CMwEnumInfo("PlayerInfoDisplayType", new string[] { "Name", "Avatar", "Avatar and Name" }) },
		                { 0x01D, new CMwEnumInfo("Vsk3SeaQuality", new string[] { "|SeaQuality|Low", "|SeaQuality|Medium", "|SeaQuality|High", "|SeaQuality|Very High" }) },
		                { 0x01E, new CMwEnumInfo("Vsk3BoatQuality", new string[] { "|BoatQuality|All Low", "|BoatQuality|Low Opponents", "|BoatQuality|All High" }) },
		                { 0x01F, new CMwEnumInfo("Vsk3TeamMate", new string[] { "|TeamMates|None", "|TeamMates|My Boat", "|TeamMates|All Boats" }) },
		                { 0x020, new CMwEnumInfo("Vsk3Stem", new string[] { "|TeamMates|None", "|TeamMates|My Boat", "|TeamMates|All Boats" }) },
		                { 0x021, new CMwEnumInfo("TmCarQuality", new string[] { "|CarQuality|All Low", "|CarQuality|Medium, Low opponents", "|CarQuality|High, Medium opponents", "|CarQuality|All High" }) },
		                { 0x022, new CMwEnumInfo("TmCarParticlesQuality", new string[] { "|CarParticlesQuality|All Low", "|CarParticlesQuality|All Medium", "|CarParticlesQuality|High,Medium opponents", "|CarParticlesQuality|All High" }) },
		                { 0x023, new CMwEnumInfo("TmCarProjector", new string[] { "|CarProjectors|None", "|CarProjectors|My Car", "|CarProjectors|All Cars" }) },
		                { 0x024, new CMwEnumInfo("TmOpponents", new string[] { "|Opponents|Always Visible", "|Opponents|Hide Too Close" }) },
		                { 0x025, new CMwFieldInfo("TmOppShadows", 0x00000001) },
		                { 0x026, new CMwFieldInfo("TmMaxOpponents", 0x0000001F) },
		                { 0x027, new CMwEnumInfo("TmBackgroundQuality", new string[] { "|BackgroundQuality|Low", "|BackgroundQuality|Medium", "|BackgroundQuality|High" }) },
		                { 0x028, new CMwFieldInfo("AudioEnabled", 0x00000001) },
		                { 0x029, new CMwFieldInfo("AudioSoundVolume", 0x00000028) },
		                { 0x02A, new CMwFieldInfo("AudioMusicVolume", 0x00000028) },
		                { 0x02B, new CMwEnumInfo("AudioGlobalQuality", new string[] { "|AudioQuality|Low", "|AudioQuality|Normal", "|AudioQuality|High" }) },
		                { 0x02C, new CMwFieldInfo("AudioDevice_Oal", 0x00000029) },
		                { 0x02D, new CMwEnumInfo("AudioAcceleration_Dx9", new string[] { "|Audio|Auto", "|Audio|HardwareOnly", "|Audio|SoftwareOnly" }) },
		                { 0x02E, new CMwEnumInfo("AudioQuality3d_Dx9", new string[] { "|Audio|NoHrtf", "|Audio|HrtfLight", "|Audio|HrtfFull" }) },
		                { 0x02F, new CMwFieldInfo("AudioUseEAX", 0x00000001) },
		                { 0x030, new CMwFieldInfo("AudioDisableDoppler", 0x00000001) },
		                { 0x031, new CMwEnumInfo("AudioSpeakerConfig", new string[] { "|Speaker|Use system config", "|Speaker|Mono", "|Speaker|Headphone", "|Speaker|Stereo min", "|Speaker|Stereo narrow", "|Speaker|Stereo wide", "|Speaker|Stereo max", "|Speaker|Quad", "|Speaker|Surround", "|Speaker|5.1", "|Speaker|7.1", "|Speaker|No speakers" }) },
		                { 0x032, new CMwFieldInfo("InputsAlternateMethod", 0x00000001) },
		                { 0x033, new CMwFieldInfo("InputsCaptureKeyboard", 0x00000001) },
		                { 0x034, new CMwFieldInfo("InputsFreezeUnusedAxes", 0x00000001) },
		                { 0x035, new CMwFieldInfo("InputsEnableRumble", 0x00000001) },
		                { 0x036, new CMwEnumInfo("Advertising_Enabled", new string[] { "Disabled", "Configurable", "Forced" }) },
		                { 0x037, new CMwFieldInfo("Advertising_TunningCoef", 0x00000028) },
		                { 0x038, new CMwFieldInfo("Advertising_DisabledByUser", 0x00000001) },
		                { 0x039, new CMwFieldInfo("EnableCrashLogUpload", 0x00000001) }
	                  })
	                },
	                { 0x007, new CMwClassInfo("CNodSystem", new Dictionary<int, CMwMemberInfo>()
	                  {
	                  })
	                },
	                { 0x008, new CMwClassInfo("CSystemFid", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("Nod", 0x00000005) },
		                { 0x001, new CMwFieldInfo("Text", 0x00000001) },
		                { 0x002, new CMwFieldInfo("Compressed", 0x00000001) }
	                  })
	                },
	                { 0x009, new CMwClassInfo("CSystemFids", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("Trees", 0x00000007) },
		                { 0x001, new CMwFieldInfo("Leaves", 0x00000007) }
	                  })
	                },
	                { 0x00A, new CMwClassInfo("CSystemFidFile", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("ByteSize", 0x0000001F) },
		                { 0x001, new CMwFieldInfo("ByteSizeEd", 0x0000001F) },
		                { 0x002, new CMwFieldInfo("IsReadOnly", 0x00000001) },
		                { 0x003, new CMwFieldInfo("FileName", 0x0000002D) },
		                { 0x004, new CMwFieldInfo("FullFileName", 0x0000002D) },
		                { 0x005, new CMwFieldInfo("ShortFileName", 0x0000002D) },
		                { 0x006, new CMwMethodInfo("CopyToFileRelative", 0x00000041,
			                new uint[] { 0x00000029, 0x00000001 },
			                new string[] { "RelFileName", "FailIfExists" })
		                }
	                  })
	                },
	                { 0x00B, new CMwClassInfo("CSystemFidsFolder", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("ByteSize", 0x0000001F) },
		                { 0x001, new CMwFieldInfo("ByteSizeEd", 0x0000001F) },
		                { 0x002, new CMwFieldInfo("DirName", 0x0000002D) },
		                { 0x003, new CMwFieldInfo("FullDirName", 0x0000002D) }
	                  })
	                },
	                { 0x00C, new CMwClassInfo("CSystemFidsDrive", new Dictionary<int, CMwMemberInfo>()
	                  {
	                  })
	                },
	                { 0x00D, new CMwClassInfo("CSystemCmdLoadNod", new Dictionary<int, CMwMemberInfo>()
	                  {
	                  })
	                },
	                { 0x00E, new CMwClassInfo("CSystemFidMemory", new Dictionary<int, CMwMemberInfo>()
	                  {
	                  })
	                },
	                { 0x00F, new CMwClassInfo("CSystemCmdDuplicateNod", new Dictionary<int, CMwMemberInfo>()
	                  {
	                  })
	                },
	                { 0x010, new CMwClassInfo("CSystemCmdExec", new Dictionary<int, CMwMemberInfo>()
	                  {
	                  })
	                },
	                { 0x011, new CMwClassInfo("CSystemCmdAssert", new Dictionary<int, CMwMemberInfo>()
	                  {
	                  })
	                },
	                { 0x013, new CMwClassInfo("CSystemConfigDisplay", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("FullScreen", 0x00000001) },
		                { 0x001, new CMwFieldInfo("StereoscopyByDefault", 0x00000001) },
		                { 0x002, new CMwFieldInfo("StereoscopyAdvanced", 0x00000001) },
		                { 0x003, new CMwFieldInfo("RefreshRate", 0x0000001F) },
		                { 0x004, new CMwFieldInfo("VSync", 0x00000001) },
		                { 0x005, new CMwEnumInfo("AutoScale", new string[] { "|GfxAuto|Faster", "|GfxAuto|Normal", "|GfxAuto|Nicer" }) },
		                { 0x006, new CMwFieldInfo("Customize", 0x00000001) },
		                { 0x007, new CMwEnumInfo("Preset", new string[] { "|DisplayPreset|None", "|DisplayPreset|Minimum Quality", "|DisplayPreset|Low Quality", "|DisplayPreset|Medium Quality", "|DisplayPreset|High Quality", "|DisplayPreset|VeryHigh Quality" }) },
		                { 0x008, new CMwEnumInfo("ColorDepth", new string[] { "|ColorDepth|16 bits", "|ColorDepth|32 bits" }) },
		                { 0x009, new CMwEnumInfo("Antialiasing", new string[] { "|Antialiasing|None", "|Antialiasing| 2 samples", "|Antialiasing| 4 samples", "|Antialiasing| 6 samples", "|Antialiasing| 8 samples", "|Antialiasing|16 samples" }) },
		                { 0x00A, new CMwEnumInfo("ShaderQuality", new string[] { "PC0", "PC1", "PC2", "PC3 Low", "PC3 High" }) },
		                { 0x00B, new CMwEnumInfo("TexturesQuality", new string[] { "|TexturesQuality|VeryLow", "|TexturesQuality|Low", "|TexturesQuality|Medium", "|TexturesQuality|High" }) },
		                { 0x00C, new CMwEnumInfo("FilterAnisoQ", new string[] { "|MaxFiltering|Bilinear", "|MaxFiltering|Trilinear", "|MaxFiltering|Anisotropic  2x", "|MaxFiltering|Anisotropic  4x", "|MaxFiltering|Anisotropic  8x", "|MaxFiltering|Anisotropic 16x" }) },
		                { 0x00D, new CMwEnumInfo("ZClip", new string[] { "|ZClip|Disable", "|ZClip|Auto", "|ZClip|Fixed" }) },
		                { 0x00E, new CMwEnumInfo("ZClipAuto", new string[] { "|ZClipAuto|Near", "|ZClipAuto|Medium", "|ZClipAuto|Far" }) },
		                { 0x00F, new CMwFieldInfo("ZClipNbBlock", 0x0000001F) },
		                { 0x010, new CMwFieldInfo("GeomLodScaleZ", 0x00000024) },
		                { 0x011, new CMwFieldInfo("WaterGeom", 0x00000001) },
		                { 0x012, new CMwFieldInfo("WaterGeomStadium", 0x00000001) },
		                { 0x013, new CMwFieldInfo("TreeAlwaysHq", 0x00000001) },
		                { 0x014, new CMwFieldInfo("PostFxEnable", 0x00000001) },
		                { 0x015, new CMwFieldInfo("ForceFxColors", 0x00000001) },
		                { 0x016, new CMwFieldInfo("ForceFxMotionBlur", 0x00000001) },
		                { 0x017, new CMwFieldInfo("ForceFxBloom", 0x00000001) },
		                { 0x018, new CMwEnumInfo("LightMapQuality", new string[] { "|LightMapQuality|None", "|LightMapQuality|2k^2", "|LightMapQuality|4k^2", "|LightMapQuality|8k^2" }) },
		                { 0x019, new CMwEnumInfo("Shadows", new string[] { "|Shadows|None", "|Shadows|Minimum", "|Shadows|Medium", "|Shadows|High", "|Shadows|VeryHigh", "|Shadows|Complex" }) },
		                { 0x01A, new CMwEnumInfo("GpuSync", new string[] { "|GpuSync|None", "|GpuSync|3 Frames", "|GpuSync|2 Frames & Half", "|GpuSync|2 Frames", "|GpuSync|1 Frame & Half", "|GpuSync|1 Frame", "|GpuSync|Half a frame", "|GpuSync|Immediate" }) },
		                { 0x01B, new CMwFieldInfo("GpuSyncTimeOut", 0x0000001F) },
		                { 0x01C, new CMwEnumInfo("VertexProcess", new string[] { "|VertexProcess|Hardware", "|VertexProcess|SoftwareVertexShader", "|VertexProcess|Software" }) },
		                { 0x01D, new CMwFieldInfo("DisableShadowBuffer", 0x00000001) },
		                { 0x01E, new CMwFieldInfo("EmulateCursorGDI", 0x00000001) },
		                { 0x01F, new CMwFieldInfo("OptimizePartDyna", 0x00000001) },
		                { 0x020, new CMwFieldInfo("IgnoreDriverCrashes", 0x00000001) },
		                { 0x021, new CMwFieldInfo("DisableColorWMask", 0x00000001) },
		                { 0x022, new CMwFieldInfo("DisableZBufferRange", 0x00000001) },
		                { 0x023, new CMwFieldInfo("DisableWindowedAntiAlias", 0x00000001) },
		                { 0x024, new CMwFieldInfo("EnableFullscreenGDI", 0x00000001) },
		                { 0x025, new CMwEnumInfo("LightMapCompute", new string[] { "|LightMapCompute|NoShadows", "|LightMapCompute|SafeMode", "|LightMapCompute|Normal" }) },
		                { 0x026, new CMwEnumInfo("LightFromMap", new string[] { "|LightFromMap|None", "|LightFromMap|MyVehicle", "|LightFromMap|AllVehicles" }) },
		                { 0x027, new CMwFieldInfo("EnableCheckLags", 0x00000001) },
		                { 0x028, new CMwFieldInfo("EnableRenderReadBack", 0x00000001) },
		                { 0x029, new CMwFieldInfo("AgpUseFactor", 0x00000028) },
		                { 0x02A, new CMwFieldInfo("MultiThreadEnable", 0x00000001) },
		                { 0x02B, new CMwFieldInfo("MultiThreadCountMax", 0x0000001F) }
	                  })
	                },
	                { 0x014, new CMwClassInfo("CSystemPackManager", new Dictionary<int, CMwMemberInfo>()
	                  {
	                  })
	                },
	                { 0x015, new CMwClassInfo("CSystemPackDesc", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("Name", 0x0000002D) },
		                { 0x001, new CMwFieldInfo("Checksum", 0x00000029) },
		                { 0x002, new CMwFieldInfo("FileName", 0x0000002D) },
		                { 0x003, new CMwFieldInfo("Size", 0x00000029) },
		                { 0x004, new CMwFieldInfo("LocatorFileName", 0x0000002D) },
		                { 0x005, new CMwFieldInfo("Url", 0x00000029) },
		                { 0x006, new CMwFieldInfo("AutoUpdate", 0x00000001) }
	                  })
	                },
	                { 0x016, new CMwClassInfo("CSystemFidBuffer", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("Buffer", 0x00000007) }
	                  })
	                },
	                { 0x017, new CMwClassInfo("CSystemNodWrapper", new Dictionary<int, CMwMemberInfo>()
	                  {
	                  })
	                },
	                { 0x018, new CMwClassInfo("CSystemData", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("Url", 0x00000029) },
		                { 0x001, new CMwFieldInfo("PackDesc", 0x00000005) },
		                { 0x002, new CMwFieldInfo("Data", 0x00000005) }
	                  })
	                }
                  })
                },
                { 0x0C, new CMwEngineInfo("Vision", new Dictionary<int, CMwClassInfo>()
                  {
	                { 0x001, new CMwClassInfo("CVisionViewport", new Dictionary<int, CMwMemberInfo>()
	                  {
	                  })
	                },
	                { 0x00A, new CMwClassInfo("CVisionViewportDx9", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("PointSizeMin", 0x00000024) },
		                { 0x001, new CMwFieldInfo("PointSizeMax", 0x00000024) },
		                { 0x002, new CMwEnumInfo("CpuExtSupported", new string[] { "None", "SSE", "SSE2" }) },
		                { 0x003, new CMwFieldInfo("ShadowDepthBiasFactor", 0x00000024) },
		                { 0x004, new CMwFieldInfo("ShadowDepthSlopeFactor", 0x00000024) },
		                { 0x005, new CMwEnumInfo("ShadowDepthFilter", new string[] { "Point", "Bilinear", "Trilinear", "Anisotropic" }) },
		                { 0x006, new CMwFieldInfo("ShadowPower", 0x00000024) },
		                { 0x007, new CMwFieldInfo("DeviceCaps", 0x00000005) },
		                { 0x008, new CMwMethodInfo("GetPerformance", 0x00000000, null, null) },
		                { 0x009, new CMwFieldInfo("BenchShader", 0x00000005) },
		                { 0x00A, new CMwMethodInfo("RemoveAllResourceAndReset", 0x00000000, null, null) }
	                  })
	                },
	                { 0x00B, new CMwClassInfo("CDx9DeviceCaps", new Dictionary<int, CMwMemberInfo>()
	                  {
	                  })
	                },
	                { 0x012, new CMwClassInfo("CVisionResourceFile", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("PhlslBlurHV_DepthTest", 0x00000005) },
		                { 0x001, new CMwFieldInfo("BlurDepthTestMaxDist", 0x00000024) },
		                { 0x002, new CMwFieldInfo("ShaderDeferredOccZCmp", 0x00000005) },
		                { 0x003, new CMwFieldInfo("ShaderDeferredShadowPssm", 0x00000005) },
		                { 0x004, new CMwFieldInfo("BitmapDeferredZ", 0x00000005) },
		                { 0x005, new CMwFieldInfo("BitmapDeferredFaceNormalInC", 0x00000005) }
	                  })
	                }
                  })
                },
                { 0x10, new CMwEngineInfo("Audio", new Dictionary<int, CMwClassInfo>()
                  {
	                { 0x001, new CMwClassInfo("CAudioPort", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("IsEnabled", 0x00000001) },
		                { 0x001, new CMwFieldInfo("Sounds", 0x00000007) },
		                { 0x002, new CMwFieldInfo("BufferKeepers", 0x00000007) },
		                { 0x003, new CMwFieldInfo("HmsListener", 0x00000005) },
		                { 0x004, new CMwFieldInfo("SoundVolume", 0x00000028) },
		                { 0x005, new CMwFieldInfo("MusicVolume", 0x00000028) },
		                { 0x006, new CMwFieldInfo("HmsSoundVolume", 0x00000028) },
		                { 0x007, new CMwFieldInfo("HmsSoundVolume2", 0x00000028) },
		                { 0x008, new CMwEnumInfo("SettingQuality", new string[] { "|AudioQuality|Low", "|AudioQuality|Normal", "|AudioQuality|High" }) },
		                { 0x009, new CMwFieldInfo("SettingDisableDoppler", 0x00000001) },
		                { 0x00A, new CMwFieldInfo("SettingUpdatePeriod", 0x00000023) },
		                { 0x00B, new CMwFieldInfo("SettingSoundsPerUpdate", 0x0000001F) },
		                { 0x00C, new CMwFieldInfo("SettingMaxSimultaneousSounds", 0x0000001F) },
		                { 0x00D, new CMwFieldInfo("NbMaxSounds", 0x0000001F) },
		                { 0x00E, new CMwFieldInfo("IsCapturing", 0x00000001) },
		                { 0x00F, new CMwFieldInfo("CapturedFileSnds", 0x00000007) },
		                { 0x010, new CMwFieldInfo("MuteAllSoundsExpectThis", 0x00000005) },
		                { 0x011, new CMwMethodInfo("LoadExternalSoundParam", 0x00000000, null, null) },
		                { 0x012, new CMwMethodInfo("SaveExternalSoundParam", 0x00000000, null, null) },
		                { 0x013, new CMwFieldInfo("AnalyzerAudioTimePerSec", 0x00000024) },
		                { 0x014, new CMwFieldInfo("AnalyzerAudioTimePerSecUpdate", 0x00000024) },
		                { 0x015, new CMwFieldInfo("AnalyzerAudioTimePerSecStream", 0x00000024) },
		                { 0x016, new CMwFieldInfo("AnalyzerStreamBytesPerSec", 0x0000001F) },
		                { 0x017, new CMwFieldInfo("AnalyzerAudioLongestSlice", 0x0000001F) },
		                { 0x018, new CMwFieldInfo("AnalyzerTotalKeepersMem", 0x0000001F) },
		                { 0x019, new CMwFieldInfo("AnalyzerNbFadingSounds", 0x0000001F) },
		                { 0x01A, new CMwFieldInfo("AnalyzerNbAutoBalancedSounds", 0x0000001F) }
	                  })
	                },
	                { 0x002, new CMwClassInfo("CAudioBufferKeeper", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("PlugFileSnd", 0x00000005) },
		                { 0x001, new CMwFieldInfo("AllocSize", 0x0000001F) },
		                { 0x002, new CMwFieldInfo("IsStreaming", 0x00000001) },
		                { 0x003, new CMwFieldInfo("PreBufferingSize", 0x0000001F) }
	                  })
	                },
	                { 0x003, new CMwClassInfo("CAudioSound", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("IsPlaying", 0x00000001) },
		                { 0x001, new CMwFieldInfo("IsActuallyPlaying", 0x00000001) },
		                { 0x002, new CMwMethodInfo("Play", 0x00000000, null, null) },
		                { 0x003, new CMwMethodInfo("Stop", 0x00000000, null, null) },
		                { 0x004, new CMwMethodInfo("RefreshStaticProperties", 0x00000000, null, null) },
		                { 0x005, new CMwFieldInfo("PlugSound", 0x00000005) },
		                { 0x006, new CMwFieldInfo("UseLowQuality", 0x00000001) },
		                { 0x007, new CMwFieldInfo("PriorityAdjustement", 0x00000024) },
		                { 0x008, new CMwFieldInfo("PlayCursor", 0x00000024) },
		                { 0x009, new CMwFieldInfo("PlayCursorUi", 0x00000028) },
		                { 0x00A, new CMwFieldInfo("Pan", 0x00000028) },
		                { 0x00B, new CMwFieldInfo("Volume", 0x00000028) },
		                { 0x00C, new CMwFieldInfo("Pitch", 0x00000028) },
		                { 0x00D, new CMwFieldInfo("Position", 0x00000035) },
		                { 0x00E, new CMwFieldInfo("Direction", 0x00000035) },
		                { 0x00F, new CMwFieldInfo("Velocity", 0x00000035) },
		                { 0x010, new CMwEnumInfo("InternalBalanceGroup", new string[] { "Sound", "Music" }) }
	                  })
	                },
	                { 0x004, new CMwClassInfo("CAudioMusic", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("TracksEnabled", 0x00000002) }
	                  })
	                },
	                { 0x005, new CMwClassInfo("CAudioSoundEngine", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("RpmNormalised", 0x00000028) },
		                { 0x001, new CMwFieldInfo("Rpm", 0x00000024) },
		                { 0x002, new CMwFieldInfo("Accel", 0x00000028) },
		                { 0x003, new CMwFieldInfo("VehicleSpeed", 0x00000024) },
		                { 0x004, new CMwFieldInfo("Alpha", 0x00000028) },
		                { 0x005, new CMwFieldInfo("TestMode", 0x00000001) }
	                  })
	                },
	                { 0x006, new CMwClassInfo("CAudioSoundSurface", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwEnumInfo("SurfaceId", new string[] { "Concrete", "Pavement", "Grass", "Ice", "Metal", "Sand", "Dirt", "Turbo", "DirtRoad", "Rubber", "SlidingRubber", "Test", "Rock", "Water", "Wood", "Danger", "Asphalt", "WetDirtRoad", "WetAsphalt", "WetPavement", "WetGrass", "Snow", "ResonantMetal", "GolfBall", "GolfWall", "GolfGround", "Turbo2", "Bumper", "NotCollidable", "FreeWheeling", "TurboRoulette" }) },
		                { 0x001, new CMwFieldInfo("Impact", 0x0000001F) },
		                { 0x002, new CMwFieldInfo("SpeedNormalised", 0x00000028) },
		                { 0x003, new CMwFieldInfo("Speed", 0x00000024) },
		                { 0x004, new CMwFieldInfo("SkidIntensity", 0x00000028) }
	                  })
	                },
	                { 0x007, new CMwClassInfo("CAudioSoundMulti", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("Variant", 0x0000001F) }
	                  })
	                },
	                { 0x008, new CMwClassInfo("CAudioPortNull", new Dictionary<int, CMwMemberInfo>()
	                  {
	                  })
	                },
	                { 0x020, new CMwClassInfo("COalAudioPort", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("DeviceName", 0x00000029) },
		                { 0x001, new CMwFieldInfo("SettingUseEFX", 0x00000001) },
		                { 0x002, new CMwMethodInfo("EnumerateDevices", 0x00000000, null, null) },
		                { 0x003, new CMwMethodInfo("EnumerateDevices_WriteToLog", 0x00000000, null, null) },
		                { 0x004, new CMwFieldInfo("OalDevices", 0x00000007) },
		                { 0x005, new CMwMethodInfo("RefreshAllSounds", 0x00000000, null, null) },
		                { 0x006, new CMwFieldInfo("EFXEnabled", 0x00000001) },
		                { 0x007, new CMwFieldInfo("Manager_MonoSources", 0x0000001F) },
		                { 0x008, new CMwFieldInfo("Manager_StereoSources", 0x0000001F) },
		                { 0x009, new CMwFieldInfo("Manager_AvailableSources", 0x0000001F) },
		                { 0x00A, new CMwFieldInfo("CurrentEnvironment", 0x00000005) },
		                { 0x00B, new CMwFieldInfo("AnalyzerNbSoundsStr", 0x00000029) }
	                  })
	                },
	                { 0x021, new CMwClassInfo("COalAudioBufferKeeper", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("NbUses", 0x0000001F) }
	                  })
	                },
	                { 0x022, new CMwClassInfo("COalDevice", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("DeviceSpecifier", 0x00000029) },
		                { 0x001, new CMwFieldInfo("OtherNames", 0x00000029) },
		                { 0x002, new CMwFieldInfo("CanCapture", 0x00000001) },
		                { 0x003, new CMwFieldInfo("AlExtensions", 0x00000029) },
		                { 0x004, new CMwFieldInfo("AlcExtensions", 0x00000029) },
		                { 0x005, new CMwFieldInfo("VersionMajor", 0x0000001F) },
		                { 0x006, new CMwFieldInfo("VersionMinor", 0x0000001F) },
		                { 0x007, new CMwFieldInfo("IsSync", 0x00000001) },
		                { 0x008, new CMwFieldInfo("RefreshPeriod", 0x0000001F) },
		                { 0x009, new CMwFieldInfo("MixFrequency", 0x0000001F) },
		                { 0x00A, new CMwFieldInfo("NbMonoSources", 0x0000001F) },
		                { 0x00B, new CMwFieldInfo("NbStereoSources", 0x0000001F) },
		                { 0x00C, new CMwFieldInfo("XRAMFree", 0x0000001F) },
		                { 0x00D, new CMwFieldInfo("XRAMSize", 0x0000001F) },
		                { 0x00E, new CMwFieldInfo("EFX_VersionMajor", 0x0000001F) },
		                { 0x00F, new CMwFieldInfo("EFX_VersionMinor", 0x0000001F) },
		                { 0x010, new CMwFieldInfo("EFX_MaxAuxSends", 0x0000001F) }
	                  })
	                }
                  })
                },
                { 0x12, new CMwEngineInfo("Net", new Dictionary<int, CMwClassInfo>()
                  {
	                { 0x001, new CMwClassInfo("CNetNod", new Dictionary<int, CMwMemberInfo>()
	                  {
	                  })
	                },
	                { 0x002, new CMwClassInfo("CNetServerInfo", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("SessionName", 0x00000029) },
		                { 0x001, new CMwFieldInfo("GameID", 0x00000029) },
		                { 0x002, new CMwFieldInfo("GameVersion", 0x00000029) },
		                { 0x003, new CMwFieldInfo("HostName", 0x00000029) },
		                { 0x004, new CMwFieldInfo("LocalIP", 0x00000029) },
		                { 0x005, new CMwFieldInfo("LocalUDPPort", 0x0000001F) },
		                { 0x006, new CMwFieldInfo("LocalTCPPort", 0x0000001F) },
		                { 0x007, new CMwFieldInfo("RemoteIP", 0x00000029) },
		                { 0x008, new CMwFieldInfo("RemoteUDPPort", 0x0000001F) },
		                { 0x009, new CMwFieldInfo("RemoteTCPPort", 0x0000001F) }
	                  })
	                },
	                { 0x003, new CMwClassInfo("CNetClientInfo", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("GameID", 0x00000029) },
		                { 0x001, new CMwFieldInfo("GameVersion", 0x00000029) },
		                { 0x002, new CMwFieldInfo("HostName", 0x00000029) },
		                { 0x003, new CMwFieldInfo("LocalIP", 0x00000029) },
		                { 0x004, new CMwFieldInfo("RemoteIP", 0x00000029) }
	                  })
	                },
	                { 0x004, new CMwClassInfo("CNetFormTimed", new Dictionary<int, CMwMemberInfo>()
	                  {
	                  })
	                },
	                { 0x007, new CMwClassInfo("CNetFormQuerrySessions", new Dictionary<int, CMwMemberInfo>()
	                  {
	                  })
	                },
	                { 0x008, new CMwClassInfo("CNetFormEnumSessions", new Dictionary<int, CMwMemberInfo>()
	                  {
	                  })
	                },
	                { 0x009, new CMwClassInfo("CNetFormPing", new Dictionary<int, CMwMemberInfo>()
	                  {
	                  })
	                },
	                { 0x00C, new CMwClassInfo("CNetServer", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("P2PPort", 0x0000001F) },
		                { 0x001, new CMwFieldInfo("AcceptConnections", 0x00000001) },
		                { 0x002, new CMwFieldInfo("NbrNewConnections", 0x0000001F) },
		                { 0x003, new CMwFieldInfo("NbrConnections", 0x0000001F) },
		                { 0x004, new CMwFieldInfo("NbrConnectionsDone", 0x0000001F) },
		                { 0x005, new CMwFieldInfo("NbrConnectionsPending", 0x0000001F) },
		                { 0x006, new CMwFieldInfo("NbrConnectionsDisconnecting", 0x0000001F) },
		                { 0x007, new CMwFieldInfo("SendingDataRate", 0x0000001F) },
		                { 0x008, new CMwFieldInfo("TCPSendingDataRate", 0x0000001F) },
		                { 0x009, new CMwFieldInfo("UDPSendingDataRate", 0x0000001F) },
		                { 0x00A, new CMwFieldInfo("ReceivingDataRate", 0x0000001F) },
		                { 0x00B, new CMwFieldInfo("TCPReceivingDataRate", 0x0000001F) },
		                { 0x00C, new CMwFieldInfo("UDPReceivingDataRate", 0x0000001F) },
		                { 0x00D, new CMwFieldInfo("ReceptionPacketTotal", 0x0000001F) },
		                { 0x00E, new CMwFieldInfo("UDPReceptionPacketTotal", 0x0000001F) },
		                { 0x00F, new CMwFieldInfo("TCPReceptionPacketTotal", 0x0000001F) },
		                { 0x010, new CMwFieldInfo("SendingPacketTotal", 0x0000001F) },
		                { 0x011, new CMwFieldInfo("UDPSendingPacketTotal", 0x0000001F) },
		                { 0x012, new CMwFieldInfo("TCPSendingPacketTotal", 0x0000001F) },
		                { 0x013, new CMwFieldInfo("ReceptionNodTotal", 0x0000001F) },
		                { 0x014, new CMwFieldInfo("UDPReceptionNodTotal", 0x0000001F) },
		                { 0x015, new CMwFieldInfo("TCPReceptionNodTotal", 0x0000001F) },
		                { 0x016, new CMwFieldInfo("SendingNodTotal", 0x0000001F) },
		                { 0x017, new CMwFieldInfo("UDPSendingNodTotal", 0x0000001F) },
		                { 0x018, new CMwFieldInfo("TCPSendingNodTotal", 0x0000001F) }
	                  })
	                },
	                { 0x00D, new CMwClassInfo("CNetClient", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("Connections", 0x00000007) },
		                { 0x001, new CMwFieldInfo("NbrNewConnections", 0x0000001F) },
		                { 0x002, new CMwFieldInfo("NbrConnections", 0x0000001F) },
		                { 0x003, new CMwFieldInfo("NbrConnectionsInProgress", 0x0000001F) },
		                { 0x004, new CMwFieldInfo("NbrConnectionsDone", 0x0000001F) },
		                { 0x005, new CMwFieldInfo("NbrConnectionsDisconnecting", 0x0000001F) },
		                { 0x006, new CMwFieldInfo("SendingDataRate", 0x0000001F) },
		                { 0x007, new CMwFieldInfo("TCPSendingDataRate", 0x0000001F) },
		                { 0x008, new CMwFieldInfo("UDPSendingDataRate", 0x0000001F) },
		                { 0x009, new CMwFieldInfo("ReceivingDataRate", 0x0000001F) },
		                { 0x00A, new CMwFieldInfo("TCPReceivingDataRate", 0x0000001F) },
		                { 0x00B, new CMwFieldInfo("UDPReceivingDataRate", 0x0000001F) },
		                { 0x00C, new CMwFieldInfo("ReceptionPacketTotal", 0x0000001F) },
		                { 0x00D, new CMwFieldInfo("UDPReceptionPacketTotal", 0x0000001F) },
		                { 0x00E, new CMwFieldInfo("TCPReceptionPacketTotal", 0x0000001F) },
		                { 0x00F, new CMwFieldInfo("SendingPacketTotal", 0x0000001F) },
		                { 0x010, new CMwFieldInfo("UDPSendingPacketTotal", 0x0000001F) },
		                { 0x011, new CMwFieldInfo("TCPSendingPacketTotal", 0x0000001F) },
		                { 0x012, new CMwFieldInfo("ReceptionNodTotal", 0x0000001F) },
		                { 0x013, new CMwFieldInfo("UDPReceptionNodTotal", 0x0000001F) },
		                { 0x014, new CMwFieldInfo("TCPReceptionNodTotal", 0x0000001F) },
		                { 0x015, new CMwFieldInfo("SendingNodTotal", 0x0000001F) },
		                { 0x016, new CMwFieldInfo("UDPSendingNodTotal", 0x0000001F) },
		                { 0x017, new CMwFieldInfo("TCPSendingNodTotal", 0x0000001F) },
		                { 0x018, new CMwFieldInfo("TimeLatestPing", 0x0000001F) },
		                { 0x019, new CMwFieldInfo("TimeLatestEpsilon", 0x00000024) },
		                { 0x01A, new CMwFieldInfo("TimeSmoothedEpsilon", 0x00000024) },
		                { 0x01B, new CMwFieldInfo("DiscontinuityLastEpsilon", 0x00000024) },
		                { 0x01C, new CMwFieldInfo("LatestTimeSynchronization", 0x0000001F) },
		                { 0x01D, new CMwFieldInfo("TimeNotifyDiscontinuity", 0x00000001) },
		                { 0x01E, new CMwFieldInfo("PrevDiscontinuityTime", 0x0000001F) },
		                { 0x01F, new CMwFieldInfo("TimeCorrectionWeight", 0x00000024) },
		                { 0x020, new CMwFieldInfo("TimeThreshold", 0x00000024) },
		                { 0x021, new CMwFieldInfo("TimeSmoothing", 0x00000024) },
		                { 0x022, new CMwFieldInfo("TimeLookahead", 0x0000001F) }
	                  })
	                },
	                { 0x00F, new CMwClassInfo("CNetConnection", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("ClientToServer", 0x00000001) },
		                { 0x001, new CMwFieldInfo("Info", 0x00000005) },
		                { 0x002, new CMwFieldInfo("TCPPort", 0x0000001F) },
		                { 0x003, new CMwFieldInfo("UDPPort", 0x0000001F) },
		                { 0x004, new CMwFieldInfo("State", 0x0000001F) },
		                { 0x005, new CMwFieldInfo("Broken", 0x00000001) },
		                { 0x006, new CMwFieldInfo("ConnectionTCP", 0x00000001) },
		                { 0x007, new CMwFieldInfo("ConnectionWaiting", 0x00000001) },
		                { 0x008, new CMwFieldInfo("ConnectionRequest", 0x00000001) },
		                { 0x009, new CMwFieldInfo("TestingUDP", 0x00000001) },
		                { 0x00A, new CMwFieldInfo("Synchronisation", 0x00000001) },
		                { 0x00B, new CMwFieldInfo("Connected", 0x00000001) },
		                { 0x00C, new CMwFieldInfo("CanReceiveTCP", 0x00000001) },
		                { 0x00D, new CMwFieldInfo("CanSendTCP", 0x00000001) },
		                { 0x00E, new CMwFieldInfo("CanReceiveUDP", 0x00000001) },
		                { 0x00F, new CMwFieldInfo("CanSendUDP", 0x00000001) },
		                { 0x010, new CMwFieldInfo("TCPEmissionQueue", 0x0000001F) },
		                { 0x011, new CMwFieldInfo("WasUDPPacketDropped", 0x00000001) },
		                { 0x012, new CMwFieldInfo("IsTCPSaturated", 0x00000001) },
		                { 0x013, new CMwFieldInfo("LatestNetworkActivity", 0x0000001F) },
		                { 0x014, new CMwFieldInfo("LatestUDPActivity", 0x0000001F) },
		                { 0x015, new CMwFieldInfo("ReceptionPacketTotal", 0x0000001F) },
		                { 0x016, new CMwFieldInfo("UDPReceptionPacketTotal", 0x0000001F) },
		                { 0x017, new CMwFieldInfo("TCPReceptionPacketTotal", 0x0000001F) },
		                { 0x018, new CMwFieldInfo("SendingPacketTotal", 0x0000001F) },
		                { 0x019, new CMwFieldInfo("UDPSendingPacketTotal", 0x0000001F) },
		                { 0x01A, new CMwFieldInfo("TCPSendingPacketTotal", 0x0000001F) },
		                { 0x01B, new CMwFieldInfo("ReceptionNodTotal", 0x0000001F) },
		                { 0x01C, new CMwFieldInfo("UDPReceptionNodTotal", 0x0000001F) },
		                { 0x01D, new CMwFieldInfo("TCPReceptionNodTotal", 0x0000001F) },
		                { 0x01E, new CMwFieldInfo("SendingNodTotal", 0x0000001F) },
		                { 0x01F, new CMwFieldInfo("UDPSendingNodTotal", 0x0000001F) },
		                { 0x020, new CMwFieldInfo("TCPSendingNodTotal", 0x0000001F) },
		                { 0x021, new CMwFieldInfo("UDPReceptionPacketLoss", 0x0000001F) },
		                { 0x022, new CMwFieldInfo("UDPReceptionPacketTotalWithoutLoss", 0x0000001F) },
		                { 0x023, new CMwFieldInfo("UDPReceptionCounter", 0x0000001F) },
		                { 0x024, new CMwFieldInfo("UDPSendingCounter", 0x0000001F) },
		                { 0x025, new CMwFieldInfo("TCPReceptionCounter", 0x0000001F) },
		                { 0x026, new CMwFieldInfo("TCPSendingCounter", 0x0000001F) },
		                { 0x027, new CMwMethodInfo("Disconnect", 0x00000000, null, null) }
	                  })
	                },
	                { 0x010, new CMwClassInfo("CNetFormConnectionAdmin", new Dictionary<int, CMwMemberInfo>()
	                  {
	                  })
	                },
	                { 0x012, new CMwClassInfo("CNetHttpClient", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("Server", 0x00000029) },
		                { 0x001, new CMwFieldInfo("Port", 0x0000001F) },
		                { 0x002, new CMwFieldInfo("UserName", 0x00000029) },
		                { 0x003, new CMwFieldInfo("Password", 0x00000029) },
		                { 0x004, new CMwFieldInfo("LastError", 0x00000029) },
		                { 0x005, new CMwFieldInfo("Connected", 0x00000001) },
		                { 0x006, new CMwFieldInfo("Requests", 0x00000007) }
	                  })
	                },
	                { 0x013, new CMwClassInfo("CNetHttpResult", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("Server", 0x00000029) },
		                { 0x001, new CMwFieldInfo("Path", 0x00000029) },
		                { 0x002, new CMwEnumInfo("Kind", new string[] { "Download", "Upload" }) },
		                { 0x003, new CMwEnumInfo("Status", new string[] { "Connecting", "Request", "Downloading", "Done", "Error" }) },
		                { 0x004, new CMwFieldInfo("ExpectedSize", 0x0000001F) },
		                { 0x005, new CMwFieldInfo("ContentType", 0x00000029) },
		                { 0x006, new CMwFieldInfo("ContentEncoding", 0x00000029) },
		                { 0x007, new CMwFieldInfo("CurrentSize", 0x0000001F) },
		                { 0x008, new CMwFieldInfo("HttpStatus", 0x0000001F) },
		                { 0x009, new CMwFieldInfo("HttpError", 0x0000001F) }
	                  })
	                },
	                { 0x014, new CMwClassInfo("CNetMasterServer", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("ServerAddr", 0x00000029) },
		                { 0x001, new CMwFieldInfo("ServerSecurePort", 0x0000001F) },
		                { 0x002, new CMwFieldInfo("ServerNonSecurePort", 0x0000001F) },
		                { 0x003, new CMwFieldInfo("ServerPath", 0x00000029) },
		                { 0x004, new CMwFieldInfo("DummyProxyLogin", 0x00000029) },
		                { 0x005, new CMwFieldInfo("DummyProxyPass", 0x00000029) },
		                { 0x006, new CMwFieldInfo("DummyUseProxy", 0x00000001) },
		                { 0x007, new CMwFieldInfo("GameVersion", 0x00000029) },
		                { 0x008, new CMwFieldInfo("Login", 0x00000029) },
		                { 0x009, new CMwFieldInfo("Pass", 0x00000029) }
	                  })
	                },
	                { 0x015, new CMwClassInfo("CNetMasterHost", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("NetHostName", 0x00000029) }
	                  })
	                },
	                { 0x018, new CMwClassInfo("CNetFileTransfer", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("IPSources", 0x00000007) },
		                { 0x001, new CMwFieldInfo("Uploads", 0x00000007) },
		                { 0x002, new CMwFieldInfo("Downloads", 0x00000007) },
		                { 0x003, new CMwFieldInfo("TerminatedDownloads", 0x00000007) },
		                { 0x004, new CMwFieldInfo("AllowUpload", 0x00000001) },
		                { 0x005, new CMwFieldInfo("AllowDownload", 0x00000001) },
		                { 0x006, new CMwFieldInfo("EnableLocators", 0x00000001) },
		                { 0x007, new CMwFieldInfo("WaitForDownload", 0x00000001) },
		                { 0x008, new CMwFieldInfo("IPAddress", 0x00000029) },
		                { 0x009, new CMwFieldInfo("P2PKey", 0x0000001F) },
		                { 0x00A, new CMwFieldInfo("PlayerUId", 0x0000001F) },
		                { 0x00B, new CMwFieldInfo("IsServer", 0x00000001) },
		                { 0x00C, new CMwFieldInfo("UploadRate", 0x0000001F) },
		                { 0x00D, new CMwFieldInfo("DownloadRate", 0x0000001F) },
		                { 0x00E, new CMwFieldInfo("NbOfCurrentUls", 0x0000001F) },
		                { 0x00F, new CMwFieldInfo("SendBandwidthLimit", 0x0000001F) },
		                { 0x010, new CMwFieldInfo("TotalSendingSize", 0x0000001F) },
		                { 0x011, new CMwFieldInfo("NbSendChannelLeft", 0x0000001F) },
		                { 0x012, new CMwFieldInfo("NbSendChannelToRestore", 0x0000001F) },
		                { 0x013, new CMwFieldInfo("NbSendChannelUsed", 0x0000001F) },
		                { 0x014, new CMwFieldInfo("FirstTimeNotEnoughSendBandwidth", 0x0000001F) },
		                { 0x015, new CMwFieldInfo("IsEmissionSaturated", 0x00000001) },
		                { 0x016, new CMwFieldInfo("NbOfCurrentDls", 0x0000001F) },
		                { 0x017, new CMwFieldInfo("ReceiveBandwidthLimit", 0x0000001F) },
		                { 0x018, new CMwFieldInfo("TotalReceivingSize", 0x0000001F) },
		                { 0x019, new CMwFieldInfo("NbReceiveChannelLeft", 0x0000001F) },
		                { 0x01A, new CMwFieldInfo("NbReceiveChannelToRestore", 0x0000001F) },
		                { 0x01B, new CMwFieldInfo("NbReceiveChannelUsed", 0x0000001F) },
		                { 0x01C, new CMwFieldInfo("FirstTimeNotEnoughReceiveBandwidth", 0x0000001F) },
		                { 0x01D, new CMwFieldInfo("MaxDownloadRateLeft", 0x0000001F) },
		                { 0x01E, new CMwFieldInfo("MaxDownloadChannelLeft", 0x0000001F) },
		                { 0x01F, new CMwFieldInfo("MaxUploadRateLeft", 0x0000001F) },
		                { 0x020, new CMwFieldInfo("MaxUploadChannelLeft", 0x0000001F) },
		                { 0x021, new CMwFieldInfo("MinSizeTransfer", 0x0000001F) },
		                { 0x022, new CMwFieldInfo("MaxSizeTransfer", 0x0000001F) },
		                { 0x023, new CMwFieldInfo("MaxDownloads", 0x0000001F) },
		                { 0x024, new CMwFieldInfo("MaxUploads", 0x0000001F) },
		                { 0x025, new CMwFieldInfo("MaxChannelPerTransfer", 0x0000001F) },
		                { 0x026, new CMwFieldInfo("NewConnectionTimeoutForDownload", 0x0000001F) },
		                { 0x027, new CMwFieldInfo("NewConnectionTimeoutForUpload", 0x0000001F) },
		                { 0x028, new CMwFieldInfo("SendMsgThroughServerTimeoutForDownload", 0x0000001F) },
		                { 0x029, new CMwFieldInfo("SendMsgThroughServerTimeoutForUpload", 0x0000001F) },
		                { 0x02A, new CMwFieldInfo("LastUpdateTime", 0x0000001F) },
		                { 0x02B, new CMwFieldInfo("UpdateDelta", 0x0000001F) },
		                { 0x02C, new CMwFieldInfo("WriteOnDiskError", 0x0000001F) },
		                { 0x02D, new CMwFieldInfo("Server", 0x00000005) },
		                { 0x02E, new CMwFieldInfo("Client", 0x00000005) },
		                { 0x02F, new CMwFieldInfo("MasterServer", 0x00000005) },
		                { 0x030, new CMwFieldInfo("PackManager", 0x00000005) },
		                { 0x031, new CMwFieldInfo("DiskCacheDir", 0x00000005) }
	                  })
	                },
	                { 0x019, new CMwClassInfo("CNetMasterServerInfo", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("Addr", 0x00000029) },
		                { 0x001, new CMwFieldInfo("Path", 0x00000029) },
		                { 0x002, new CMwFieldInfo("Name", 0x00000029) },
		                { 0x003, new CMwEnumInfo("Level", new string[] { "Beginner", "Normal", "Expert" }) },
		                { 0x004, new CMwFieldInfo("NbOnline", 0x0000001F) },
		                { 0x005, new CMwFieldInfo("NbServer", 0x0000001F) }
	                  })
	                },
	                { 0x01A, new CMwClassInfo("CNetFileTransferNod", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("Name", 0x0000002D) },
		                { 0x001, new CMwFieldInfo("Checksum", 0x00000029) },
		                { 0x002, new CMwFieldInfo("File", 0x00000005) },
		                { 0x003, new CMwFieldInfo("TotalSize", 0x0000001F) },
		                { 0x004, new CMwFieldInfo("CurrentOffset", 0x0000001F) }
	                  })
	                },
	                { 0x01B, new CMwClassInfo("CNetFileTransferForm", new Dictionary<int, CMwMemberInfo>()
	                  {
	                  })
	                },
	                { 0x01C, new CMwClassInfo("CNetFileTransferDownload", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("IdDownload", 0x0000001F) },
		                { 0x001, new CMwEnumInfo("DownloadState", new string[] { "Download_WaitingSources", "Download_ProcessingSources", "Download_ProcessingSources", "Download_OnTermination", "Download_Done" }) },
		                { 0x002, new CMwFieldInfo("PackDesc", 0x00000005) },
		                { 0x003, new CMwFieldInfo("PriorityLevel", 0x0000001F) },
		                { 0x004, new CMwFieldInfo("PriorityFlags", 0x0000001F) },
		                { 0x005, new CMwFieldInfo("Sources", 0x00000007) },
		                { 0x006, new CMwFieldInfo("NbOfEffectiveSources", 0x0000001F) },
		                { 0x007, new CMwFieldInfo("ActiveSource", 0x00000005) },
		                { 0x008, new CMwFieldInfo("UrlSources", 0x00000007) },
		                { 0x009, new CMwFieldInfo("ActiveUrlSource", 0x00000005) },
		                { 0x00A, new CMwFieldInfo("Url", 0x00000029) },
		                { 0x00B, new CMwFieldInfo("LastUrlUsed", 0x00000029) },
		                { 0x00C, new CMwFieldInfo("AcceptUrl", 0x00000001) },
		                { 0x00D, new CMwFieldInfo("SkipServerSource", 0x00000001) },
		                { 0x00E, new CMwFieldInfo("IsNearFinished", 0x00000001) },
		                { 0x00F, new CMwFieldInfo("TempFileName", 0x0000002D) },
		                { 0x010, new CMwFieldInfo("LastWriteTimeout", 0x0000001F) },
		                { 0x011, new CMwFieldInfo("PackDescUpToDateCheck", 0x00000005) },
		                { 0x012, new CMwFieldInfo("PackDescUpToDateChecked", 0x00000001) },
		                { 0x013, new CMwFieldInfo("LastDataMsgTime", 0x0000001F) },
		                { 0x014, new CMwFieldInfo("LastDataReception", 0x0000001F) },
		                { 0x015, new CMwFieldInfo("LastDataWrite", 0x0000001F) },
		                { 0x016, new CMwFieldInfo("SendEfficiency", 0x0000001F) },
		                { 0x017, new CMwFieldInfo("InstantaneousEfficiency", 0x0000001F) },
		                { 0x018, new CMwFieldInfo("AverageEfficiency", 0x0000001F) }
	                  })
	                },
	                { 0x01D, new CMwClassInfo("CNetFileTransferUpload", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("IdUpload", 0x0000001F) },
		                { 0x001, new CMwFieldInfo("IdDownload", 0x0000001F) },
		                { 0x002, new CMwFieldInfo("IdSource", 0x0000001F) },
		                { 0x003, new CMwEnumInfo("UploadState", new string[] { "Upload_WaitingConnection", "Upload_OnQueue", "Upload_RequestingUpload", "Upload_Transfer", "Upload_Done" }) },
		                { 0x004, new CMwFieldInfo("IPSource", 0x00000005) },
		                { 0x005, new CMwFieldInfo("Connection", 0x00000005) },
		                { 0x006, new CMwFieldInfo("DownloadPriorityLevel", 0x0000001F) },
		                { 0x007, new CMwFieldInfo("PriorityLevel", 0x0000001F) },
		                { 0x008, new CMwFieldInfo("NbChannelsUsed", 0x0000001F) },
		                { 0x009, new CMwFieldInfo("NbChannelsUsedValidated", 0x0000001F) },
		                { 0x00A, new CMwFieldInfo("TimeOut", 0x0000001F) },
		                { 0x00B, new CMwFieldInfo("ValidityTimeOut", 0x0000001F) },
		                { 0x00C, new CMwFieldInfo("IsActive", 0x00000001) },
		                { 0x00D, new CMwFieldInfo("MustBeActive", 0x00000001) },
		                { 0x00E, new CMwFieldInfo("LastActiveTime", 0x0000001F) },
		                { 0x00F, new CMwFieldInfo("IsConnecting", 0x00000001) },
		                { 0x010, new CMwFieldInfo("FirstToConnect", 0x00000001) },
		                { 0x011, new CMwFieldInfo("MustSendReqAck", 0x00000001) },
		                { 0x012, new CMwFieldInfo("ReqAckSent", 0x00000001) },
		                { 0x013, new CMwFieldInfo("UploadAttempt", 0x0000001F) },
		                { 0x014, new CMwFieldInfo("MsgAttempt", 0x0000001F) },
		                { 0x015, new CMwFieldInfo("ForceCancel", 0x00000001) },
		                { 0x016, new CMwFieldInfo("ReadOffset", 0x0000001F) },
		                { 0x017, new CMwFieldInfo("LastMessageTime", 0x0000001F) },
		                { 0x018, new CMwFieldInfo("LastReadTimeOut", 0x0000001F) },
		                { 0x019, new CMwFieldInfo("LastSentTime", 0x0000001F) },
		                { 0x01A, new CMwFieldInfo("LastDataComplete", 0x0000001F) },
		                { 0x01B, new CMwFieldInfo("InstantaneousEfficiency", 0x0000001F) },
		                { 0x01C, new CMwFieldInfo("AverageEfficiency", 0x0000001F) }
	                  })
	                },
	                { 0x01E, new CMwClassInfo("CNetSource", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("IdSource", 0x0000001F) },
		                { 0x001, new CMwFieldInfo("IdUpload", 0x0000001F) },
		                { 0x002, new CMwEnumInfo("SourceState", new string[] { "Source_WaitingConnection", "Source_RequestingDownload", "Source_OnQueue", "Source_RefreshingDownload", "Source_ConnectingForTransfer", "Source_Transfer", "Source_Done" }) },
		                { 0x003, new CMwFieldInfo("Download", 0x00000005) },
		                { 0x004, new CMwFieldInfo("SourceAddress", 0x00000005) },
		                { 0x005, new CMwFieldInfo("Connection", 0x00000005) },
		                { 0x006, new CMwFieldInfo("LastMessageTime", 0x0000001F) },
		                { 0x007, new CMwFieldInfo("TimeOut", 0x0000001F) },
		                { 0x008, new CMwFieldInfo("NbChannelsUsed", 0x0000001F) },
		                { 0x009, new CMwFieldInfo("LastNbChannelsUsedProposition", 0x0000001F) },
		                { 0x00A, new CMwFieldInfo("TestedAtLeastOnce", 0x00000001) },
		                { 0x00B, new CMwFieldInfo("OwnsFile", 0x00000001) },
		                { 0x00C, new CMwFieldInfo("IsConnecting", 0x00000001) },
		                { 0x00D, new CMwFieldInfo("FirstToConnect", 0x00000001) },
		                { 0x00E, new CMwFieldInfo("ReqSent", 0x00000001) },
		                { 0x00F, new CMwFieldInfo("MustSendRequest", 0x00000001) },
		                { 0x010, new CMwFieldInfo("MustSendUploadAck", 0x00000001) },
		                { 0x011, new CMwFieldInfo("ForceSending", 0x00000001) },
		                { 0x012, new CMwFieldInfo("HasReceivedUrl", 0x00000001) },
		                { 0x013, new CMwFieldInfo("InterruptTransfer", 0x00000001) },
		                { 0x014, new CMwFieldInfo("ForceCancel", 0x00000001) },
		                { 0x015, new CMwFieldInfo("SourceAttempt", 0x0000001F) },
		                { 0x016, new CMwFieldInfo("MsgAttempt", 0x0000001F) }
	                  })
	                },
	                { 0x020, new CMwClassInfo("CNetIPC", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("Port", 0x0000001F) },
		                { 0x001, new CMwFieldInfo("VersionString", 0x00000029) },
		                { 0x002, new CMwFieldInfo("MaxPacketSize", 0x0000001F) }
	                  })
	                },
	                { 0x021, new CMwClassInfo("CNetFormRpcCall", new Dictionary<int, CMwMemberInfo>()
	                  {
	                  })
	                },
	                { 0x022, new CMwClassInfo("CNetUPnP", new Dictionary<int, CMwMemberInfo>()
	                  {
	                  })
	                },
	                { 0x028, new CMwClassInfo("CNetIPSource", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("RemoteIP", 0x00000029) },
		                { 0x001, new CMwFieldInfo("DownloadRate", 0x0000001F) },
		                { 0x002, new CMwFieldInfo("UploadRate", 0x0000001F) },
		                { 0x003, new CMwFieldInfo("IsServer", 0x00000001) },
		                { 0x004, new CMwFieldInfo("IsUploadEnabled", 0x00000001) },
		                { 0x005, new CMwFieldInfo("P2PKey", 0x0000001F) },
		                { 0x006, new CMwFieldInfo("PlayerUId", 0x0000001F) },
		                { 0x007, new CMwFieldInfo("GameConnection", 0x00000005) },
		                { 0x008, new CMwFieldInfo("ConnectionFrom", 0x00000005) },
		                { 0x009, new CMwFieldInfo("ConnectionTo", 0x00000005) },
		                { 0x00A, new CMwFieldInfo("ConnectTimeOut", 0x0000001F) },
		                { 0x00B, new CMwFieldInfo("LastConnectionTime", 0x0000001F) },
		                { 0x00C, new CMwFieldInfo("LastContactTime", 0x0000001F) },
		                { 0x00D, new CMwFieldInfo("ConnectionFromTimeOut", 0x0000001F) },
		                { 0x00E, new CMwFieldInfo("ConnectionToTimeOut", 0x0000001F) },
		                { 0x00F, new CMwFieldInfo("IsConnecting", 0x00000001) },
		                { 0x010, new CMwFieldInfo("ConnectionAttempt", 0x0000001F) },
		                { 0x011, new CMwFieldInfo("ConnectionTested", 0x00000001) },
		                { 0x012, new CMwFieldInfo("ConnectionPossible", 0x00000001) },
		                { 0x013, new CMwFieldInfo("ThroughServerAttempt", 0x0000001F) },
		                { 0x014, new CMwFieldInfo("ThroughServerTested", 0x00000001) },
		                { 0x015, new CMwFieldInfo("ThroughServerPossible", 0x00000001) },
		                { 0x016, new CMwFieldInfo("CanBeConnectedBy", 0x00000001) },
		                { 0x017, new CMwFieldInfo("SourcesUsingConnectionFrom", 0x00000007) },
		                { 0x018, new CMwFieldInfo("UploadsUsingConnectionFrom", 0x00000007) },
		                { 0x019, new CMwFieldInfo("SourcesUsingConnectionTo", 0x00000007) },
		                { 0x01A, new CMwFieldInfo("UploadsUsingConnectionTo", 0x00000007) },
		                { 0x01B, new CMwFieldInfo("ForceCancel", 0x00000001) }
	                  })
	                },
	                { 0x029, new CMwClassInfo("CNetMasterServerUptoDateCheck", new Dictionary<int, CMwMemberInfo>()
	                  {
	                  })
	                },
	                { 0x030, new CMwClassInfo("CNetURLSource", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwEnumInfo("URLSourceState", new string[] { "URLSource_WaitingConnection", "URLSource_Transfer", "URLSource_Done" }) },
		                { 0x001, new CMwFieldInfo("Download", 0x00000005) },
		                { 0x002, new CMwFieldInfo("MasterServerDownload", 0x00000005) },
		                { 0x003, new CMwFieldInfo("Url", 0x00000029) }
	                  })
	                }
                  })
                },
                { 0x13, new CMwEngineInfo("Input", new Dictionary<int, CMwClassInfo>()
                  {
	                { 0x001, new CMwClassInfo("CInputPort", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwEnumInfo("InputsMode", new string[] { "Timed", "NotTimed", "Config" }) },
		                { 0x001, new CMwFieldInfo("PollingEnabled", 0x00000001) },
		                { 0x002, new CMwFieldInfo("CurrentActionMap", 0x00000029) },
		                { 0x003, new CMwFieldInfo("ConnectedDevices", 0x00000006) },
		                { 0x004, new CMwFieldInfo("MaxSampleRate", 0x0000001F) },
		                { 0x005, new CMwFieldInfo("MinHistoryLength", 0x0000001F) },
		                { 0x006, new CMwFieldInfo("EventInStoreCount", 0x0000001F) },
		                { 0x007, new CMwFieldInfo("IsDoingIME", 0x00000001) },
		                { 0x008, new CMwFieldInfo("RumbleIntensity", 0x00000028) },
		                { 0x009, new CMwFieldInfo("StatsDInputOverflowedLastFrame", 0x00000001) },
		                { 0x00A, new CMwFieldInfo("StatsDInputOverflowCount", 0x0000001F) },
		                { 0x00B, new CMwFieldInfo("StatsDInputEventsLastFrame", 0x0000001F) },
		                { 0x00C, new CMwFieldInfo("StatsDInputEventsCount", 0x0000001F) },
		                { 0x00D, new CMwFieldInfo("StatsLatestEvent", 0x0000001F) },
		                { 0x00E, new CMwFieldInfo("StatsDInputTimedEventsCount", 0x0000001F) },
		                { 0x00F, new CMwFieldInfo("StatsDInputWrongTimestampAhead", 0x0000001F) },
		                { 0x010, new CMwFieldInfo("StatsDInputWrongTimestampRatioAhead", 0x00000024) },
		                { 0x011, new CMwFieldInfo("StatsDInputWrongTimestampLate", 0x0000001F) },
		                { 0x012, new CMwFieldInfo("StatsDInputWrongTimestampRatioLate", 0x00000024) },
		                { 0x013, new CMwFieldInfo("StatsDInputWrongTimestampAvgDelta", 0x00000024) },
		                { 0x014, new CMwFieldInfo("StatsNbMappedInputsReceived", 0x0000001F) }
	                  })
	                },
	                { 0x002, new CMwClassInfo("CInputPortDx8", new Dictionary<int, CMwMemberInfo>()
	                  {
	                  })
	                },
	                { 0x003, new CMwClassInfo("CInputPortNull", new Dictionary<int, CMwMemberInfo>()
	                  {
	                  })
	                },
	                { 0x006, new CMwClassInfo("CInputBindingsConfig", new Dictionary<int, CMwMemberInfo>()
	                  {
	                  })
	                },
	                { 0x007, new CMwClassInfo("CInputDevice", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("DeviceName", 0x0000002D) },
		                { 0x001, new CMwFieldInfo("InstanceName", 0x0000002D) },
		                { 0x002, new CMwFieldInfo("InputNotAvailable", 0x00000001) },
		                { 0x003, new CMwFieldInfo("MustBePolled", 0x00000001) },
		                { 0x004, new CMwFieldInfo("CanRumble", 0x00000001) },
		                { 0x005, new CMwFieldInfo("InstanceId", 0x0000001B) },
		                { 0x006, new CMwFieldInfo("DeviceId", 0x0000001B) },
		                { 0x007, new CMwFieldInfo("ObjectCount", 0x0000001F) },
		                { 0x008, new CMwMethodInfo("ReadHardwareCurState", 0x00000000, null, null) }
	                  })
	                },
	                { 0x008, new CMwClassInfo("CInputDeviceMouse", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("IsInsideWindow", 0x00000001) },
		                { 0x001, new CMwFieldInfo("PosInViewport", 0x00000031) }
	                  })
	                },
	                { 0x00A, new CMwClassInfo("CInputDeviceDx8Mouse", new Dictionary<int, CMwMemberInfo>()
	                  {
	                  })
	                },
	                { 0x00B, new CMwClassInfo("CInputDeviceDx8Keyboard", new Dictionary<int, CMwMemberInfo>()
	                  {
	                  })
	                },
	                { 0x00C, new CMwClassInfo("CInputDeviceDx8Pad", new Dictionary<int, CMwMemberInfo>()
	                  {
	                  })
	                }
                  })
                },
                { 0x14, new CMwEngineInfo("Xml", new Dictionary<int, CMwClassInfo>()
                  {
	                { 0x001, new CMwClassInfo("CXmlNod", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("Attributes", 0x00000007) },
		                { 0x001, new CMwFieldInfo("UIChilds", 0x00000007) }
	                  })
	                },
	                { 0x002, new CMwClassInfo("CXmlAttribute", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("Name", 0x00000029) },
		                { 0x001, new CMwFieldInfo("Value", 0x00000029) }
	                  })
	                },
	                { 0x003, new CMwClassInfo("CXmlComment", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("Comment", 0x00000029) }
	                  })
	                },
	                { 0x004, new CMwClassInfo("CXmlDeclaration", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("Version", 0x00000029) },
		                { 0x001, new CMwFieldInfo("Encoding", 0x00000029) },
		                { 0x002, new CMwFieldInfo("Standalone", 0x00000029) }
	                  })
	                },
	                { 0x005, new CMwClassInfo("CXmlDocument", new Dictionary<int, CMwMemberInfo>()
	                  {
	                  })
	                },
	                { 0x006, new CMwClassInfo("CXmlElement", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("Name", 0x00000029) }
	                  })
	                },
	                { 0x007, new CMwClassInfo("CXmlText", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("Text", 0x00000029) }
	                  })
	                },
	                { 0x008, new CMwClassInfo("CXmlUnknown", new Dictionary<int, CMwMemberInfo>()
	                  {
	                  })
	                },
	                { 0x009, new CMwClassInfo("CHdrComment", new Dictionary<int, CMwMemberInfo>()
	                  {
	                  })
	                },
	                { 0x00A, new CMwClassInfo("CHdrDeclaration", new Dictionary<int, CMwMemberInfo>()
	                  {
	                  })
	                },
	                { 0x00B, new CMwClassInfo("CHdrDocument", new Dictionary<int, CMwMemberInfo>()
	                  {
	                  })
	                },
	                { 0x00C, new CMwClassInfo("CHdrElement", new Dictionary<int, CMwMemberInfo>()
	                  {
	                  })
	                },
	                { 0x00D, new CMwClassInfo("CHdrText", new Dictionary<int, CMwMemberInfo>()
	                  {
	                  })
	                },
	                { 0x00E, new CMwClassInfo("CHdrUnknown", new Dictionary<int, CMwMemberInfo>()
	                  {
	                  })
	                }
                  })
                },
                { 0x24, new CMwEngineInfo("TrackMania", new Dictionary<int, CMwClassInfo>()
                  {
	                { 0x001, new CMwClassInfo("CTrackMania", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("Switcher", 0x00000005) },
		                { 0x001, new CMwFieldInfo("Editor", 0x00000005) },
		                { 0x002, new CMwFieldInfo("MenuManager", 0x00000005) },
		                { 0x003, new CMwFieldInfo("CampaignBaseEditor", 0x00000005) },
		                { 0x004, new CMwEnumInfo("ChallengeType", new string[] { "None", "1P_Basic", "2P_TurnBased", "2P_Retro", "Net_TimeAttack" }) },
		                { 0x005, new CMwFieldInfo("PillarOffset", 0x00000024) },
		                { 0x006, new CMwMethodInfo("ShowContextHelp", 0x00000000, null, null) },
		                { 0x007, new CMwMethodInfo("ScanDiskForChallenges", 0x00000000, null, null) },
		                { 0x008, new CMwMethodInfo("ScanDiskForCampaigns", 0x00000000, null, null) },
		                { 0x009, new CMwMethodInfo("ScanDiskForReplays", 0x00000000, null, null) },
		                { 0x00A, new CMwFieldInfo("IsVehicleUpdateAsync", 0x00000001) },
		                { 0x00B, new CMwMethodInfo("DebugVerifyAllBlocks", 0x00000000, null, null) },
		                { 0x00C, new CMwMethodInfo("DebugConvertReplays", 0x00000000, null, null) },
		                { 0x00D, new CMwEnumInfo("PlayTest", new string[] { "Mix1", "Classic", "Slide", "Realist" }) },
		                { 0x00E, new CMwFieldInfo("MatchSettings", 0x00000007) }
	                  })
	                },
	                { 0x013, new CMwClassInfo("CTrackManiaEditor", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("MinX", 0x00000024) },
		                { 0x001, new CMwFieldInfo("MaxX", 0x00000024) },
		                { 0x002, new CMwFieldInfo("MinZ", 0x00000024) },
		                { 0x003, new CMwFieldInfo("MaxZ", 0x00000024) },
		                { 0x004, new CMwFieldInfo("ZoneHeight", 0x0000000E) },
		                { 0x005, new CMwFieldInfo("CameraAngle", 0x00000024) },
		                { 0x006, new CMwFieldInfo("NbMaxCoppers", 0x0000001F) },
		                { 0x007, new CMwFieldInfo("NbAvailableCoppers", 0x0000000E) },
		                { 0x008, new CMwFieldInfo("EditorLink", 0x00000005) },
		                { 0x009, new CMwFieldInfo("CurrentBlock", 0x00000005) },
		                { 0x00A, new CMwFieldInfo("Cursor", 0x00000005) },
		                { 0x00B, new CMwFieldInfo("EditorInterface", 0x00000005) },
		                { 0x00C, new CMwFieldInfo("EditedBlockInfo", 0x00000005) },
		                { 0x00D, new CMwFieldInfo("SkinText", 0x0000002D) },
		                { 0x00E, new CMwFieldInfo("DeltaTimeDoubleClick", 0x0000001F) },
		                { 0x00F, new CMwFieldInfo("CoordX", 0x0000001F) },
		                { 0x010, new CMwFieldInfo("CoordY", 0x0000001F) },
		                { 0x011, new CMwFieldInfo("CoordZ", 0x0000001F) },
		                { 0x012, new CMwFieldInfo("CoordBlock", 0x00000005) },
		                { 0x013, new CMwFieldInfo("Grid", 0x00000005) },
		                { 0x014, new CMwFieldInfo("GridColor", 0x00000009) },
		                { 0x015, new CMwFieldInfo("GridColorAlpha", 0x00000028) },
		                { 0x016, new CMwFieldInfo("Game", 0x00000005) },
		                { 0x017, new CMwMethodInfo("Sweep", 0x00000000, null, null) },
		                { 0x018, new CMwMethodInfo("SwitchToCameraIconMode", 0x00000000, null, null) },
		                { 0x019, new CMwMethodInfo("SwitchFromCameraIconMode", 0x00000000, null, null) },
		                { 0x01A, new CMwMethodInfo("BlockViewerOnClick", 0x00000000, null, null) },
		                { 0x01B, new CMwMethodInfo("BlockAddOnClick", 0x00000000, null, null) },
		                { 0x01C, new CMwMethodInfo("BlockSubOnClick", 0x00000000, null, null) },
		                { 0x01D, new CMwMethodInfo("ChooseIcon1OnClick", 0x00000000, null, null) },
		                { 0x01E, new CMwMethodInfo("ChooseIcon2OnClick", 0x00000000, null, null) },
		                { 0x01F, new CMwMethodInfo("ChooseIcon3OnClick", 0x00000000, null, null) },
		                { 0x020, new CMwMethodInfo("ChooseIcon4OnClick", 0x00000000, null, null) },
		                { 0x021, new CMwMethodInfo("ChooseIcon5OnClick", 0x00000000, null, null) },
		                { 0x022, new CMwMethodInfo("ChooseIcon6OnClick", 0x00000000, null, null) },
		                { 0x023, new CMwMethodInfo("ChooseIcon7OnClick", 0x00000000, null, null) },
		                { 0x024, new CMwMethodInfo("ChooseIcon8OnClick", 0x00000000, null, null) },
		                { 0x025, new CMwMethodInfo("ChooseIcon9OnClick", 0x00000000, null, null) },
		                { 0x026, new CMwMethodInfo("ButtonContextualHelpOnClick", 0x00000000, null, null) },
		                { 0x027, new CMwMethodInfo("ButtonPlayOnClick", 0x00000000, null, null) },
		                { 0x028, new CMwMethodInfo("ButtonBackOnClick", 0x00000000, null, null) },
		                { 0x029, new CMwMethodInfo("ButtonSaveOnClick", 0x00000000, null, null) },
		                { 0x02A, new CMwMethodInfo("ButtonLoadOnClick", 0x00000000, null, null) },
		                { 0x02B, new CMwMethodInfo("ButtonCameraUpOnClick", 0x00000000, null, null) },
		                { 0x02C, new CMwMethodInfo("ButtonCameraDownOnClick", 0x00000000, null, null) },
		                { 0x02D, new CMwMethodInfo("ButtonCameraLeftOnClick", 0x00000000, null, null) },
		                { 0x02E, new CMwMethodInfo("ButtonCameraRightOnClick", 0x00000000, null, null) },
		                { 0x02F, new CMwMethodInfo("ButtonSweepOnClick", 0x00000000, null, null) },
		                { 0x030, new CMwMethodInfo("ButtonHelper1OnClick", 0x00000000, null, null) },
		                { 0x031, new CMwMethodInfo("ButtonHelper2OnClick", 0x00000000, null, null) },
		                { 0x032, new CMwMethodInfo("ButtonValidateOnClick", 0x00000000, null, null) },
		                { 0x033, new CMwMethodInfo("ButtonCursorRaiseOnClick", 0x00000000, null, null) },
		                { 0x034, new CMwMethodInfo("ButtonCursorLowerOnClick", 0x00000000, null, null) },
		                { 0x035, new CMwMethodInfo("ButtonCursorTurnClockwiseOnClick", 0x00000000, null, null) },
		                { 0x036, new CMwMethodInfo("ButtonCursorTurnAnticlockwiseOnClick", 0x00000000, null, null) },
		                { 0x037, new CMwMethodInfo("ButtonCursorUpOnClick", 0x00000000, null, null) },
		                { 0x038, new CMwMethodInfo("ButtonCursorDownOnClick", 0x00000000, null, null) },
		                { 0x039, new CMwMethodInfo("ButtonCursorLeftOnClick", 0x00000000, null, null) },
		                { 0x03A, new CMwMethodInfo("ButtonCursorRightOnClick", 0x00000000, null, null) },
		                { 0x03B, new CMwMethodInfo("ButtonPasswordOnClick", 0x00000000, null, null) },
		                { 0x03C, new CMwMethodInfo("ButtonUndoOnClick", 0x00000000, null, null) },
		                { 0x03D, new CMwMethodInfo("ButtonRedoOnClick", 0x00000000, null, null) },
		                { 0x03E, new CMwMethodInfo("ButtonZoomInOnClick", 0x00000000, null, null) },
		                { 0x03F, new CMwMethodInfo("ButtonZoomOutOnClick", 0x00000000, null, null) },
		                { 0x040, new CMwMethodInfo("ButtonEraserModeOnClick", 0x00000000, null, null) },
		                { 0x041, new CMwMethodInfo("ButtonUndergroundModeOnClick", 0x00000000, null, null) },
		                { 0x042, new CMwMethodInfo("ButtonFreelookModeOnClick", 0x00000000, null, null) },
		                { 0x043, new CMwMethodInfo("ButtonHeightModeOnClick", 0x00000000, null, null) },
		                { 0x044, new CMwMethodInfo("ButtonChooseSkinModeOnClick", 0x00000000, null, null) },
		                { 0x045, new CMwMethodInfo("ButtonChooseTextModeOnClick", 0x00000000, null, null) },
		                { 0x046, new CMwMethodInfo("ButtonObjectivesOnClick", 0x00000000, null, null) },
		                { 0x047, new CMwMethodInfo("ButtonHelpOnClick", 0x00000000, null, null) },
		                { 0x048, new CMwMethodInfo("ButtonEditEndRaceReplay", 0x00000000, null, null) },
		                { 0x049, new CMwMethodInfo("ButtonBackStepOnClick", 0x00000000, null, null) },
		                { 0x04A, new CMwMethodInfo("ButtonComputeShadowsOnClick", 0x00000000, null, null) },
		                { 0x04B, new CMwMethodInfo("ShowHelp", 0x00000000, null, null) }
	                  })
	                },
	                { 0x014, new CMwClassInfo("CTrackManiaRace", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("LapIndex", 0x0000001F) },
		                { 0x001, new CMwFieldInfo("NbPlayers", 0x0000001F) },
		                { 0x002, new CMwFieldInfo("LocalCarSpeed", 0x0000001F) },
		                { 0x003, new CMwFieldInfo("LapCount", 0x0000001F) },
		                { 0x004, new CMwFieldInfo("CarSpeedDisplay", 0x0000001F) },
		                { 0x005, new CMwFieldInfo("CarDistanceDisplay", 0x0000001F) },
		                { 0x006, new CMwFieldInfo("BestTimeOrScore", 0x0000001F) },
		                { 0x007, new CMwFieldInfo("StrBestTimeOrScore", 0x00000029) },
		                { 0x008, new CMwFieldInfo("OutroDuration", 0x00000024) },
		                { 0x009, new CMwFieldInfo("Game", 0x00000005) },
		                { 0x00A, new CMwFieldInfo("TMTargetPlayerInfo", 0x00000005) },
		                { 0x00B, new CMwFieldInfo("PlayerInfoForSpecPlayerCard", 0x00000005) },
		                { 0x00C, new CMwFieldInfo("NbRespawns", 0x0000001F) },
		                { 0x00D, new CMwFieldInfo("CurrentTime", 0x0000001F) },
		                { 0x00E, new CMwFieldInfo("Fps", 0x00000024) },
		                { 0x00F, new CMwFieldInfo("Message", 0x0000002D) },
		                { 0x010, new CMwFieldInfo("EventValidated", 0x00000001) },
		                { 0x011, new CMwMethodInfo("ResetForParam", 0x00000000, null, null) },
		                { 0x012, new CMwMethodInfo("ButtonQuitOnClick", 0x00000000, null, null) },
		                { 0x013, new CMwMethodInfo("OnQuitRace2", 0x00000000, null, null) },
		                { 0x014, new CMwMethodInfo("OnCancelOfficial", 0x00000000, null, null) },
		                { 0x015, new CMwMethodInfo("OnPlayerPositionSend", 0x00000000, null, null) },
		                { 0x016, new CMwMethodInfo("OnCallVoteAnnounceChoose", 0x00000000, null, null) },
		                { 0x017, new CMwMethodInfo("LocalPlayerReset", 0x00000000, null, null) },
		                { 0x018, new CMwMethodInfo("SelectOpponents", 0x00000000, null, null) },
		                { 0x019, new CMwMethodInfo("SetOfficial", 0x00000000, null, null) },
		                { 0x01A, new CMwMethodInfo("ShowChallengeCard", 0x00000000, null, null) },
		                { 0x01B, new CMwFieldInfo("RaceGhosts", 0x00000007) },
		                { 0x01C, new CMwFieldInfo("BestRaceGhost", 0x00000005) },
		                { 0x01D, new CMwFieldInfo("IsBestRaceGhostVisible", 0x00000001) },
		                { 0x01E, new CMwFieldInfo("ForceDisplayNames", 0x00000001) },
		                { 0x01F, new CMwFieldInfo("SetGhostsFromReplay", 0x00000005) },
		                { 0x020, new CMwFieldInfo("ValidateAReplay", 0x00000005) },
		                { 0x021, new CMwFieldInfo("LocalPlayerMobil", 0x00000005) }
	                  })
	                },
	                { 0x015, new CMwClassInfo("CTrackManiaSwitcher", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwEnumInfo("Mode", new string[] { "None", "Menu", "Editor", "Race" }) }
	                  })
	                },
	                { 0x016, new CMwClassInfo("CTrackManiaEditorFree", new Dictionary<int, CMwMemberInfo>()
	                  {
	                  })
	                },
	                { 0x017, new CMwClassInfo("CTrackManiaEditorPuzzle", new Dictionary<int, CMwMemberInfo>()
	                  {
	                  })
	                },
	                { 0x018, new CMwClassInfo("CTrackManiaRace1P", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwMethodInfo("OnCheatFinish", 0x00000000, null, null) },
		                { 0x001, new CMwFieldInfo("CurrentRaceGhost", 0x00000005) }
	                  })
	                },
	                { 0x026, new CMwClassInfo("CTrackManiaEditorTerrain", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("TerrainBox", 0x00000005) },
		                { 0x001, new CMwFieldInfo("TerrainMobil", 0x00000005) }
	                  })
	                },
	                { 0x02E, new CMwClassInfo("CTrackManiaMenus", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("Menus", 0x00000005) },
		                { 0x001, new CMwFieldInfo("Dialogs", 0x00000005) },
		                { 0x002, new CMwFieldInfo("SystemDialogs", 0x00000005) },
		                { 0x003, new CMwFieldInfo("ChallengeInfosCampaign", 0x00000007) },
		                { 0x004, new CMwFieldInfo("ChallengeInfosEdited", 0x00000007) },
		                { 0x005, new CMwFieldInfo("EnvironmentChapters", 0x00000007) },
		                { 0x006, new CMwFieldInfo("ChallengeSeries", 0x00000007) },
		                { 0x007, new CMwFieldInfo("CustomMenuCampaigns", 0x00000007) },
		                { 0x008, new CMwFieldInfo("CurrentCampaign", 0x00000005) },
		                { 0x009, new CMwFieldInfo("SelectedChallengeInfo", 0x00000005) },
		                { 0x00A, new CMwFieldInfo("SelectedChallenge", 0x00000005) },
		                { 0x00B, new CMwFieldInfo("ServerInfo", 0x00000005) },
		                { 0x00C, new CMwFieldInfo("RankingScrollDelay", 0x00000024) },
		                { 0x00D, new CMwFieldInfo("RankingScrollSpeed", 0x00000024) },
		                { 0x00E, new CMwFieldInfo("LastPage", 0x0000001F) },
		                { 0x00F, new CMwEnumInfo("Medal", new string[] { "None", "Finished", "Bronze", "Silver", "Gold", "Nadeo" }) },
		                { 0x010, new CMwFieldInfo("CampaignChallengeNumber", 0x0000001F) },
		                { 0x011, new CMwFieldInfo("NbLaps", 0x0000001F) },
		                { 0x012, new CMwFieldInfo("StuntsTimeLimit", 0x0000001F) },
		                { 0x013, new CMwFieldInfo("Score", 0x0000000E) },
		                { 0x014, new CMwFieldInfo("ScoreText", 0x00000029) },
		                { 0x015, new CMwFieldInfo("TestComment", 0x0000002D) },
		                { 0x016, new CMwFieldInfo("TMTP_Score", 0x0000002D) },
		                { 0x017, new CMwFieldInfo("GhostFileName", 0x0000002D) },
		                { 0x018, new CMwFieldInfo("GhostFileNameExt", 0x0000002D) },
		                { 0x019, new CMwFieldInfo("LastScore", 0x0000001F) },
		                { 0x01A, new CMwFieldInfo("LastRecord", 0x0000001F) },
		                { 0x01B, new CMwFieldInfo("GoldTime", 0x0000001F) },
		                { 0x01C, new CMwFieldInfo("SilverTime", 0x0000001F) },
		                { 0x01D, new CMwFieldInfo("BronzeTime", 0x0000001F) },
		                { 0x01E, new CMwEnumInfo("NbPlayers", new string[] { "2", "3", "4", "5", "6", "7", "8" }) },
		                { 0x01F, new CMwEnumInfo("Selection", new string[] { "Load", "Create" }) },
		                { 0x020, new CMwEnumInfo("ChallengeSelection", new string[] { "Random", "Sorted" }) },
		                { 0x021, new CMwEnumInfo("ChallengePlayMode", new string[] { "Race", "Platform", "Puzzle", "Crazy(Obsolete)", "Shortcut(Obsolete)", "Stunts" }) },
		                { 0x022, new CMwEnumInfo("ChallengeDifficulty", new string[] { "White", "Green", "Blue", "Red", "Black", "Custom" }) },
		                { 0x023, new CMwEnumInfo("NetworkGameMode", new string[] { "Rounds", "Time Attack", "Team", "Laps", "Stunts", "Cup" }) },
		                { 0x024, new CMwEnumInfo("MultiGameMode", new string[] { "|HotSeat|Time", "|HotSeat|Tries" }) },
		                { 0x025, new CMwFieldInfo("TimeLimit", 0x0000001F) },
		                { 0x026, new CMwFieldInfo("Rounds", 0x0000001F) },
		                { 0x027, new CMwFieldInfo("MedalsCount", 0x0000001F) },
		                { 0x028, new CMwFieldInfo("TestDifficulty0", 0x00000001) },
		                { 0x029, new CMwFieldInfo("TestDifficulty1", 0x00000001) },
		                { 0x02A, new CMwFieldInfo("TestDifficulty2", 0x00000001) },
		                { 0x02B, new CMwFieldInfo("TestDifficulty3", 0x00000001) },
		                { 0x02C, new CMwFieldInfo("TestDifficulty4", 0x00000001) },
		                { 0x02D, new CMwFieldInfo("TestQuality0", 0x00000001) },
		                { 0x02E, new CMwFieldInfo("TestQuality1", 0x00000001) },
		                { 0x02F, new CMwFieldInfo("TestQuality2", 0x00000001) },
		                { 0x030, new CMwFieldInfo("TestQuality3", 0x00000001) },
		                { 0x031, new CMwFieldInfo("TestQuality4", 0x00000001) },
		                { 0x032, new CMwFieldInfo("Use2HotSeatPlayers", 0x00000001) },
		                { 0x033, new CMwFieldInfo("Use3HotSeatPlayers", 0x00000001) },
		                { 0x034, new CMwFieldInfo("Use4HotSeatPlayers", 0x00000001) },
		                { 0x035, new CMwFieldInfo("Use5HotSeatPlayers", 0x00000001) },
		                { 0x036, new CMwFieldInfo("Use6HotSeatPlayers", 0x00000001) },
		                { 0x037, new CMwFieldInfo("Use7HotSeatPlayers", 0x00000001) },
		                { 0x038, new CMwFieldInfo("Use8HotSeatPlayers", 0x00000001) },
		                { 0x039, new CMwMethodInfo("OnChooseAvatar1", 0x00000000, null, null) },
		                { 0x03A, new CMwMethodInfo("OnChooseAvatar2", 0x00000000, null, null) },
		                { 0x03B, new CMwMethodInfo("OnChooseAvatar3", 0x00000000, null, null) },
		                { 0x03C, new CMwMethodInfo("OnChooseAvatar4", 0x00000000, null, null) },
		                { 0x03D, new CMwMethodInfo("OnChooseAvatar5", 0x00000000, null, null) },
		                { 0x03E, new CMwMethodInfo("OnChooseAvatar6", 0x00000000, null, null) },
		                { 0x03F, new CMwMethodInfo("OnChooseAvatar7", 0x00000000, null, null) },
		                { 0x040, new CMwMethodInfo("OnChooseAvatar8", 0x00000000, null, null) },
		                { 0x041, new CMwMethodInfo("OnChooseProfile1", 0x00000000, null, null) },
		                { 0x042, new CMwMethodInfo("OnChooseProfile2", 0x00000000, null, null) },
		                { 0x043, new CMwMethodInfo("OnChooseProfile3", 0x00000000, null, null) },
		                { 0x044, new CMwMethodInfo("OnChooseProfile4", 0x00000000, null, null) },
		                { 0x045, new CMwMethodInfo("OnChooseProfile5", 0x00000000, null, null) },
		                { 0x046, new CMwMethodInfo("OnChooseProfile6", 0x00000000, null, null) },
		                { 0x047, new CMwMethodInfo("OnChooseProfile7", 0x00000000, null, null) },
		                { 0x048, new CMwMethodInfo("OnChooseProfile8", 0x00000000, null, null) },
		                { 0x049, new CMwMethodInfo("MenuMain", 0x00000000, null, null) },
		                { 0x04A, new CMwMethodInfo("MenuMain_OnBuddyCardSelected", 0x00000000, null, null) },
		                { 0x04B, new CMwMethodInfo("MenuMain_OnRemoveBuddy", 0x00000000, null, null) },
		                { 0x04C, new CMwMethodInfo("MenuMain_OnSendMessageToBuddy", 0x00000000, null, null) },
		                { 0x04D, new CMwMethodInfo("MenuMain_GetBuddyDataTypeInfos", 0x00000000, null, null) },
		                { 0x04E, new CMwMethodInfo("MenuEditors", 0x00000000, null, null) },
		                { 0x04F, new CMwMethodInfo("MenuEditors_OnLoadChallenge", 0x00000000, null, null) },
		                { 0x050, new CMwMethodInfo("MenuEditors_OnLoadChallenge_OnSimple", 0x00000000, null, null) },
		                { 0x051, new CMwMethodInfo("MenuEditors_OnLoadChallenge_OnAdvanced", 0x00000000, null, null) },
		                { 0x052, new CMwMethodInfo("MenuChallengeSlots", 0x00000000, null, null) },
		                { 0x053, new CMwMethodInfo("MenuChallengeSlots_OnSlot1", 0x00000000, null, null) },
		                { 0x054, new CMwMethodInfo("MenuChallengeSlots_OnSlot2", 0x00000000, null, null) },
		                { 0x055, new CMwMethodInfo("MenuChallengeSlots_OnSlot3", 0x00000000, null, null) },
		                { 0x056, new CMwMethodInfo("MenuChallengeSlots_OnSlot4", 0x00000000, null, null) },
		                { 0x057, new CMwMethodInfo("MenuChallengeSlots_OnSlot5", 0x00000000, null, null) },
		                { 0x058, new CMwMethodInfo("MenuChoosePlaylistHotseat", 0x00000000, null, null) },
		                { 0x059, new CMwMethodInfo("MenuChoosePlaylistNetwork", 0x00000000, null, null) },
		                { 0x05A, new CMwMethodInfo("MenuChoosePlaylist_Init", 0x00000000, null, null) },
		                { 0x05B, new CMwMethodInfo("MenuChoosePlaylist_Clean", 0x00000000, null, null) },
		                { 0x05C, new CMwMethodInfo("MenuChoosePlaylist_OnPlaylistSelected", 0x00000000, null, null) },
		                { 0x05D, new CMwMethodInfo("MenuChoosePlaylist_OnBack", 0x00000000, null, null) },
		                { 0x05E, new CMwMethodInfo("MenuChoosePlaylist_OnCustom", 0x00000000, null, null) },
		                { 0x05F, new CMwMethodInfo("MenuChoosePlaylist_OnRefresh", 0x00000000, null, null) },
		                { 0x060, new CMwFieldInfo("MenuChoosePlaylist_PlaylistsCount", 0x0000001F) },
		                { 0x061, new CMwMethodInfo("MenuChoosePlaylist_OnOfficialTracks", 0x00000000, null, null) },
		                { 0x062, new CMwMethodInfo("MenuChoosePlaylist_OnMyTracks", 0x00000000, null, null) },
		                { 0x063, new CMwMethodInfo("MenuChoosePlaylist_OnDownloadedTracks", 0x00000000, null, null) },
		                { 0x064, new CMwMethodInfo("MenuQuitGame", 0x00000000, null, null) },
		                { 0x065, new CMwMethodInfo("MenuCreateChallenge", 0x00000000, null, null) },
		                { 0x066, new CMwMethodInfo("MenuCreateChallenge_OnSimple", 0x00000000, null, null) },
		                { 0x067, new CMwMethodInfo("MenuCreateChallenge_OnAdvanced", 0x00000000, null, null) },
		                { 0x068, new CMwMethodInfo("MenuPlayChallenge_Solo", 0x00000000, null, null) },
		                { 0x069, new CMwMethodInfo("MenuPlayChallenge_Solo_Challenges", 0x00000000, null, null) },
		                { 0x06A, new CMwMethodInfo("MenuPlayChallenge_Solo_MyTracks", 0x00000000, null, null) },
		                { 0x06B, new CMwMethodInfo("MenuPlayChallenge_Solo_DownloadedTracks", 0x00000000, null, null) },
		                { 0x06C, new CMwMethodInfo("MenuPlayChallenge_Edit", 0x00000000, null, null) },
		                { 0x06D, new CMwMethodInfo("MenuAcceptOnlineInvitation", 0x00000000, null, null) },
		                { 0x06E, new CMwMethodInfo("MenuDeclineOnlineInvitation", 0x00000000, null, null) },
		                { 0x06F, new CMwMethodInfo("MenuChooseChallenge_OnDownloadReplay", 0x00000000, null, null) },
		                { 0x070, new CMwMethodInfo("MenuChooseChallenge_OnSaveSettings", 0x00000000, null, null) },
		                { 0x071, new CMwMethodInfo("MenuChooseChallenge_OnSaveSettings_OnYes", 0x00000000, null, null) },
		                { 0x072, new CMwMethodInfo("MenuChooseChallenge_OnSaveSettings_DoSave", 0x00000000, null, null) },
		                { 0x073, new CMwMethodInfo("MenuSolo", 0x00000000, null, null) },
		                { 0x074, new CMwMethodInfo("MenuSolo_OnBack", 0x00000000, null, null) },
		                { 0x075, new CMwMethodInfo("MenuSolo_OnChallengeCardSelected", 0x00000000, null, null) },
		                { 0x076, new CMwFieldInfo("MenuSolo_Location", 0x0000002D) },
		                { 0x077, new CMwFieldInfo("MenuSolo_Level", 0x0000001F) },
		                { 0x078, new CMwFieldInfo("MenuSolo_ZoneLogoBitmap", 0x00000005) },
		                { 0x079, new CMwMethodInfo("MenuSolo_OnChallengeGridPrevPage", 0x00000000, null, null) },
		                { 0x07A, new CMwMethodInfo("MenuSolo_OnChallengeGridNextPage", 0x00000000, null, null) },
		                { 0x07B, new CMwMethodInfo("MenuSolo_OnChallengeCardRemoved", 0x00000000, null, null) },
		                { 0x07C, new CMwMethodInfo("MenuSolo_OnChallengeCardRemovedConfirmed", 0x00000000, null, null) },
		                { 0x07D, new CMwMethodInfo("MenuSolo_OnFriendUnlockYes", 0x00000000, null, null) },
		                { 0x07E, new CMwMethodInfo("MenuSolo_OnFriendUnlockNo", 0x00000000, null, null) },
		                { 0x07F, new CMwMethodInfo("MenuReplayEditor", 0x00000000, null, null) },
		                { 0x080, new CMwMethodInfo("MenuGhostEditor", 0x00000000, null, null) },
		                { 0x081, new CMwMethodInfo("MenuHotSeatCreate", 0x00000000, null, null) },
		                { 0x082, new CMwMethodInfo("MenuHotSeatCreate_Start", 0x00000000, null, null) },
		                { 0x083, new CMwMethodInfo("MenuHotSeatCreate_LoadSettings", 0x00000000, null, null) },
		                { 0x084, new CMwMethodInfo("MenuHotSeatCreate_OnBack", 0x00000000, null, null) },
		                { 0x085, new CMwMethodInfo("MenuHotSeatCreate_OnOk", 0x00000000, null, null) },
		                { 0x086, new CMwMethodInfo("MenuMultiPlayerNetworkCreate", 0x00000000, null, null) },
		                { 0x087, new CMwMethodInfo("MenuMultiPlayerNetworkCreate_OnStart", 0x00000000, null, null) },
		                { 0x088, new CMwMethodInfo("MenuMultiPlayerNetworkCreate_OnBack", 0x00000000, null, null) },
		                { 0x089, new CMwMethodInfo("MenuMultiPlayerNetworkCreate_OnAdvanced", 0x00000000, null, null) },
		                { 0x08A, new CMwMethodInfo("MenuMultiPlayerNetworkCreate_OnLoadSettings", 0x00000000, null, null) },
		                { 0x08B, new CMwMethodInfo("MenuMultiPlayerNetworkCreate_OnLoadSettings_OnYes", 0x00000000, null, null) },
		                { 0x08C, new CMwFieldInfo("MenuMultiPlayerNetworkCreate_AcceptReferee", 0x00000001) },
		                { 0x08D, new CMwMethodInfo("MenuMultiPlayer_OnLan", 0x00000000, null, null) },
		                { 0x08E, new CMwMethodInfo("MenuMultiPlayer_OnInternet", 0x00000000, null, null) },
		                { 0x08F, new CMwMethodInfo("MenuStatistics", 0x00000000, null, null) },
		                { 0x090, new CMwMethodInfo("MenuStatistics_OnBack", 0x00000000, null, null) },
		                { 0x091, new CMwMethodInfo("Dialog_OnValidate", 0x00000000, null, null) },
		                { 0x092, new CMwMethodInfo("Dialog_OnCancel", 0x00000000, null, null) },
		                { 0x093, new CMwMethodInfo("DialogQuitRace", 0x00000000, null, null) },
		                { 0x094, new CMwMethodInfo("DialogQuitRace_OnResume", 0x00000000, null, null) },
		                { 0x095, new CMwMethodInfo("DialogQuitRace_OnRestart", 0x00000000, null, null) },
		                { 0x096, new CMwMethodInfo("DialogQuitRace_OnOfficial", 0x00000000, null, null) },
		                { 0x097, new CMwMethodInfo("DialogQuitRace_OnOfficialCustom", 0x00000000, null, null) },
		                { 0x098, new CMwMethodInfo("DialogQuitRace_OnTrackRankings", 0x00000000, null, null) },
		                { 0x099, new CMwMethodInfo("DialogQuitRace_OnTrackRankingsCustom", 0x00000000, null, null) },
		                { 0x09A, new CMwMethodInfo("DialogQuitRace_OnInputSettings", 0x00000000, null, null) },
		                { 0x09B, new CMwMethodInfo("DialogQuitRace_OnStereoscopySettings", 0x00000000, null, null) },
		                { 0x09C, new CMwMethodInfo("DialogQuitRace_OnSelectOpponents", 0x00000000, null, null) },
		                { 0x09D, new CMwMethodInfo("DialogQuitRace_OnEdit", 0x00000000, null, null) },
		                { 0x09E, new CMwMethodInfo("DialogQuitRace_OnQuit", 0x00000000, null, null) },
		                { 0x09F, new CMwMethodInfo("DialogQuickChooseGhostOpponents", 0x00000000, null, null) },
		                { 0x0A0, new CMwMethodInfo("DialogQuickChooseGhostOpponents_OnNone", 0x00000000, null, null) },
		                { 0x0A1, new CMwMethodInfo("DialogQuickChooseGhostOpponents_OnBronze", 0x00000000, null, null) },
		                { 0x0A2, new CMwMethodInfo("DialogQuickChooseGhostOpponents_OnSilver", 0x00000000, null, null) },
		                { 0x0A3, new CMwMethodInfo("DialogQuickChooseGhostOpponents_OnGold", 0x00000000, null, null) },
		                { 0x0A4, new CMwMethodInfo("DialogQuickChooseGhostOpponents_OnAuthor", 0x00000000, null, null) },
		                { 0x0A5, new CMwMethodInfo("DialogChooseGhostOpponents", 0x00000000, null, null) },
		                { 0x0A6, new CMwMethodInfo("DialogChooseGhostOpponents_Clean", 0x00000000, null, null) },
		                { 0x0A7, new CMwMethodInfo("DialogChooseGhostOpponents_OnRefresh", 0x00000000, null, null) },
		                { 0x0A8, new CMwMethodInfo("DialogChooseGhostOpponents_OnSelectReplay", 0x00000000, null, null) },
		                { 0x0A9, new CMwMethodInfo("DialogChooseGhostOpponents_FilterAndRedraw", 0x00000000, null, null) },
		                { 0x0AA, new CMwMethodInfo("DialogChooseGhostOpponents_OnOk", 0x00000000, null, null) },
		                { 0x0AB, new CMwMethodInfo("DialogLowFpsWarn_OnOk", 0x00000000, null, null) },
		                { 0x0AC, new CMwMethodInfo("DialogLowFpsWarn_OnCancel", 0x00000000, null, null) },
		                { 0x0AD, new CMwFieldInfo("DialogLowFpsWarn_NeverAskAgain", 0x00000001) },
		                { 0x0AE, new CMwMethodInfo("DialogQuitEditor", 0x00000000, null, null) },
		                { 0x0AF, new CMwMethodInfo("DialogQuitEditor_OnDelete", 0x00000000, null, null) },
		                { 0x0B0, new CMwMethodInfo("DialogQuitEditor_OnUpload", 0x00000000, null, null) },
		                { 0x0B1, new CMwMethodInfo("DialogQuitEditor_OnQuit", 0x00000000, null, null) },
		                { 0x0B2, new CMwMethodInfo("DialogChallengeResult_OnRetry", 0x00000000, null, null) },
		                { 0x0B3, new CMwMethodInfo("DialogChallengeResult_OnRetryOfficial", 0x00000000, null, null) },
		                { 0x0B4, new CMwMethodInfo("DialogChallengeResult_OnQuit", 0x00000000, null, null) },
		                { 0x0B5, new CMwMethodInfo("DialogChallengeResult_OnReplay", 0x00000000, null, null) },
		                { 0x0B6, new CMwMethodInfo("DialogChallengeResult_OnEdit", 0x00000000, null, null) },
		                { 0x0B7, new CMwMethodInfo("DialogChallengeResult_OnSendScore", 0x00000000, null, null) },
		                { 0x0B8, new CMwMethodInfo("DialogChallengeResult_OnNext", 0x00000000, null, null) },
		                { 0x0B9, new CMwMethodInfo("DialogChallengeResult_OnCreateServer", 0x00000000, null, null) },
		                { 0x0BA, new CMwMethodInfo("DialogChallengeResult_OnRewind", 0x00000000, null, null) },
		                { 0x0BB, new CMwFieldInfo("DialogChallengeResult_IsPaused", 0x00000001) },
		                { 0x0BC, new CMwFieldInfo("DialogChallengeResult_IsPlaying", 0x00000001) },
		                { 0x0BD, new CMwMethodInfo("DialogEndRaceSummary_OnReplay", 0x00000000, null, null) },
		                { 0x0BE, new CMwMethodInfo("DialogChallengeCard", 0x00000000, null, null) },
		                { 0x0BF, new CMwMethodInfo("DialogChallengeCard_OnGetReplay", 0x00000000, null, null) },
		                { 0x0C0, new CMwMethodInfo("DialogChallengeCard_OnOk", 0x00000000, null, null) },
		                { 0x0C1, new CMwFieldInfo("DialogChallengeCard_ChallengeInfo", 0x00000005) },
		                { 0x0C2, new CMwMethodInfo("DialogGainMedalCoppers_OnOk", 0x00000000, null, null) },
		                { 0x0C3, new CMwMethodInfo("DialogNewUnlock_OnOk", 0x00000000, null, null) },
		                { 0x0C4, new CMwMethodInfo("DialogCreateObjectives", 0x00000000, null, null) },
		                { 0x0C5, new CMwMethodInfo("DialogCreateObjectives_OnValidate", 0x00000000, null, null) },
		                { 0x0C6, new CMwMethodInfo("DialogCreateObjectives_OnChangeType", 0x00000000, null, null) },
		                { 0x0C7, new CMwMethodInfo("DialogCreateObjectives_OnPassword", 0x00000000, null, null) },
		                { 0x0C8, new CMwMethodInfo("DialogViewReplay_OnView", 0x00000000, null, null) },
		                { 0x0C9, new CMwMethodInfo("DialogViewReplay_OnEdit", 0x00000000, null, null) },
		                { 0x0CA, new CMwMethodInfo("DialogViewReplay_OnValidate", 0x00000000, null, null) },
		                { 0x0CB, new CMwMethodInfo("DialogViewReplay_OnPlay", 0x00000000, null, null) },
		                { 0x0CC, new CMwMethodInfo("DialogViewReplay_OnBench", 0x00000000, null, null) },
		                { 0x0CD, new CMwMethodInfo("DialogViewReplay_OnShootVideo", 0x00000000, null, null) },
		                { 0x0CE, new CMwMethodInfo("DialogViewReplay_OnBack", 0x00000000, null, null) },
		                { 0x0CF, new CMwMethodInfo("DialogViewReplay_OnExportToValidate", 0x00000000, null, null) },
		                { 0x0D0, new CMwMethodInfo("DialogViewReplay_OnExportChallengeAndReplay", 0x00000000, null, null) },
		                { 0x0D1, new CMwMethodInfo("DialogAskPassword", 0x00000000, null, null) },
		                { 0x0D2, new CMwMethodInfo("DialogAskPassword_OnOk", 0x00000000, null, null) },
		                { 0x0D3, new CMwMethodInfo("DialogAskPassword_OnCancel", 0x00000000, null, null) },
		                { 0x0D4, new CMwMethodInfo("DialogEditorHelp", 0x00000000, null, null) },
		                { 0x0D5, new CMwMethodInfo("DialogEditorHelp_OnOk", 0x00000000, null, null) },
		                { 0x0D6, new CMwMethodInfo("DialogEditorMenu", 0x00000000, null, null) },
		                { 0x0D7, new CMwMethodInfo("DialogEditorMenu_OnHelpers", 0x00000000, null, null) },
		                { 0x0D8, new CMwMethodInfo("DialogEditorMenu_OnEditComments", 0x00000000, null, null) },
		                { 0x0D9, new CMwMethodInfo("DialogEditorMenu_OnEditSnapCamera", 0x00000000, null, null) },
		                { 0x0DA, new CMwMethodInfo("DialogEditorMenu_OnComputeShadows", 0x00000000, null, null) },
		                { 0x0DB, new CMwMethodInfo("DialogEditorMenu_OnReturn", 0x00000000, null, null) },
		                { 0x0DC, new CMwFieldInfo("DialogViewReplay_ReplayTime", 0x0000001F) },
		                { 0x0DD, new CMwMethodInfo("DialogInGameMenu_OnChangeTeam", 0x00000000, null, null) },
		                { 0x0DE, new CMwMethodInfo("DialogInGameMenu_OnValidateBest", 0x00000000, null, null) },
		                { 0x0DF, new CMwMethodInfo("DialogInGameMenu_OnRetire", 0x00000000, null, null) },
		                { 0x0E0, new CMwMethodInfo("DialogInGameMenu_SwitchFavourite", 0x00000000, null, null) },
		                { 0x0E1, new CMwMethodInfo("DialogInGameMenu_OnInputSettings", 0x00000000, null, null) },
		                { 0x0E2, new CMwMethodInfo("DialogInGameMenu_OnStereoscopySettings", 0x00000000, null, null) },
		                { 0x0E3, new CMwMethodInfo("DialogInGameMenu_OnSoloOfficialMode", 0x00000000, null, null) },
		                { 0x0E4, new CMwMethodInfo("DialogInGameMenu_OnTrackRankings", 0x00000000, null, null) },
		                { 0x0E5, new CMwMethodInfo("DialogChooseEnvironment_OnCancel", 0x00000000, null, null) },
		                { 0x0E6, new CMwMethodInfo("DialogEditCutScenes_OnIntroEdit", 0x00000000, null, null) },
		                { 0x0E7, new CMwMethodInfo("DialogEditCutScenes_OnIntroRemove", 0x00000000, null, null) },
		                { 0x0E8, new CMwMethodInfo("DialogEditCutScenes_OnInGameEdit", 0x00000000, null, null) },
		                { 0x0E9, new CMwMethodInfo("DialogEditCutScenes_OnInGameRemove", 0x00000000, null, null) },
		                { 0x0EA, new CMwMethodInfo("DialogEditCutScenes_OnEndRaceEdit", 0x00000000, null, null) },
		                { 0x0EB, new CMwMethodInfo("DialogEditCutScenes_OnEndRaceRemove", 0x00000000, null, null) },
		                { 0x0EC, new CMwMethodInfo("DialogEditCutScenes_OnGlobalEdit", 0x00000000, null, null) },
		                { 0x0ED, new CMwMethodInfo("DialogEditCutScenes_OnGlobalRemove", 0x00000000, null, null) },
		                { 0x0EE, new CMwMethodInfo("DialogEditCutScenes_OnRecordMediaTrackerGhost", 0x00000000, null, null) },
		                { 0x0EF, new CMwMethodInfo("DialogEditCutScenes_OnRecordMediaTrackerGhostPart", 0x00000000, null, null) },
		                { 0x0F0, new CMwMethodInfo("DialogEditCutScenes_OnChooseCustomMusic", 0x00000000, null, null) },
		                { 0x0F1, new CMwMethodInfo("DialogUseGhost", 0x00000000, null, null) },
		                { 0x0F2, new CMwMethodInfo("DialogUseGhost_OnYes", 0x00000000, null, null) },
		                { 0x0F3, new CMwMethodInfo("DialogUseGhost_OnNo", 0x00000000, null, null) },
		                { 0x0F4, new CMwMethodInfo("DialogCreateGhost", 0x00000000, null, null) },
		                { 0x0F5, new CMwMethodInfo("DialogCreateGhost_OnUse", 0x00000000, null, null) },
		                { 0x0F6, new CMwMethodInfo("DialogCreateGhost_OnRetry", 0x00000000, null, null) },
		                { 0x0F7, new CMwMethodInfo("DialogCreateGhost_OnQuit", 0x00000000, null, null) },
		                { 0x0F8, new CMwMethodInfo("DialogCreateGhost_OnReallyQuit", 0x00000000, null, null) },
		                { 0x0F9, new CMwMethodInfo("DialogChooseNbGhosts", 0x00000000, null, null) },
		                { 0x0FA, new CMwMethodInfo("DialogChooseNbGhosts_On4", 0x00000000, null, null) },
		                { 0x0FB, new CMwMethodInfo("DialogChooseNbGhosts_On7", 0x00000000, null, null) },
		                { 0x0FC, new CMwMethodInfo("DialogCreateStuntsGhost", 0x00000000, null, null) },
		                { 0x0FD, new CMwMethodInfo("DialogCreateStuntsGhost_OnQuit", 0x00000000, null, null) },
		                { 0x0FE, new CMwMethodInfo("DialogChangeTeam", 0x00000000, null, null) },
		                { 0x0FF, new CMwMethodInfo("DialogChangeTeam_OnRed", 0x00000000, null, null) },
		                { 0x100, new CMwMethodInfo("DialogChangeTeam_OnBlue", 0x00000000, null, null) },
		                { 0x101, new CMwMethodInfo("DialogChangeTeam_OnCancel", 0x00000000, null, null) },
		                { 0x102, new CMwFieldInfo("DialogChangeTeam_RedPlayers", 0x00000007) },
		                { 0x103, new CMwFieldInfo("DialogChangeTeam_BluePlayers", 0x00000007) },
		                { 0x104, new CMwMethodInfo("DialogHotSeatResult_OnRevenge", 0x00000000, null, null) },
		                { 0x105, new CMwMethodInfo("DialogHotSeatResult_OnNext", 0x00000000, null, null) },
		                { 0x106, new CMwMethodInfo("DialogHotSeatResult_OnQuit", 0x00000000, null, null) },
		                { 0x107, new CMwMethodInfo("DialogHotSeatInGameMenu_OnResume", 0x00000000, null, null) },
		                { 0x108, new CMwMethodInfo("DialogHotSeatInGameMenu_OnGiveUp", 0x00000000, null, null) },
		                { 0x109, new CMwMethodInfo("DialogHotSeatInGameMenu_OnQuit", 0x00000000, null, null) },
		                { 0x10A, new CMwMethodInfo("DialogTestReport", 0x00000000, null, null) },
		                { 0x10B, new CMwMethodInfo("DialogTestReport_OnOk", 0x00000000, null, null) },
		                { 0x10C, new CMwMethodInfo("DialogTestReport_OnCancel", 0x00000000, null, null) },
		                { 0x10D, new CMwMethodInfo("ShowMenus", 0x00000000, null, null) },
		                { 0x10E, new CMwMethodInfo("ShowDialogs", 0x00000000, null, null) },
		                { 0x10F, new CMwFieldInfo("CatalogChapterTotalCoppers", 0x0000001F) },
		                { 0x110, new CMwFieldInfo("DialogChallengeInfos", 0x00000007) },
		                { 0x111, new CMwMethodInfo("DialogLaunchScores_OnOk", 0x00000000, null, null) },
		                { 0x112, new CMwMethodInfo("DialogLaunchScores_OnClickCard", 0x00000000, null, null) },
		                { 0x113, new CMwMethodInfo("DialogLaunchScores_OnClickUp", 0x00000000, null, null) },
		                { 0x114, new CMwMethodInfo("DialogLaunchScores_UpdateParams", 0x00000000, null, null) },
		                { 0x115, new CMwFieldInfo("DialogLaunchScores_CurrentPath", 0x0000002D) },
		                { 0x116, new CMwMethodInfo("DialogChallengeCardSelect_OnPlay", 0x00000000, null, null) },
		                { 0x117, new CMwMethodInfo("DialogChallengeCardSelect_OnBack", 0x00000000, null, null) },
		                { 0x118, new CMwMethodInfo("DialogChallengeCardSelect_OnRemove", 0x00000000, null, null) },
		                { 0x119, new CMwMethodInfo("DialogGetNewChallenge_OnDownload", 0x00000000, null, null) },
		                { 0x11A, new CMwMethodInfo("DialogGetNewChallenge_OnBrowse", 0x00000000, null, null) },
		                { 0x11B, new CMwMethodInfo("DialogGetNewChallenge_OnCancel", 0x00000000, null, null) },
		                { 0x11C, new CMwMethodInfo("DialogNextGhostOpponent_OnOk", 0x00000000, null, null) },
		                { 0x11D, new CMwMethodInfo("DialogNextGhostOpponent_OnCancel", 0x00000000, null, null) },
		                { 0x11E, new CMwMethodInfo("DialogSendScore_OnCopyToClipboard", 0x00000000, null, null) },
		                { 0x11F, new CMwMethodInfo("DialogSendScore_OnExportGhost", 0x00000000, null, null) },
		                { 0x120, new CMwMethodInfo("DialogSendScore_OnOk", 0x00000000, null, null) },
		                { 0x121, new CMwMethodInfo("DialogReplayLoaded_OnPlayAgainst", 0x00000000, null, null) },
		                { 0x122, new CMwMethodInfo("DialogReplayLoaded_OnWatch", 0x00000000, null, null) },
		                { 0x123, new CMwMethodInfo("DialogReplayLoaded_OnCancel", 0x00000000, null, null) }
	                  })
	                },
	                { 0x02F, new CMwClassInfo("CTrackManiaNetwork", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("PlayerInfo", 0x00000005) },
		                { 0x001, new CMwFieldInfo("CurrentRaceIsValid", 0x00000001) },
		                { 0x002, new CMwFieldInfo("ForceEndRound", 0x00000001) },
		                { 0x003, new CMwFieldInfo("WaitTimeBeforeTwoValidations", 0x0000001F) }
	                  })
	                },
	                { 0x030, new CMwClassInfo("CTrackManiaNetForm", new Dictionary<int, CMwMemberInfo>()
	                  {
	                  })
	                },
	                { 0x031, new CMwClassInfo("CTrackManiaPlayer", new Dictionary<int, CMwMemberInfo>()
	                  {
	                  })
	                },
	                { 0x032, new CMwClassInfo("CTrackManiaControlPlayerInput", new Dictionary<int, CMwMemberInfo>()
	                  {
	                  })
	                },
	                { 0x035, new CMwClassInfo("CTrackManiaNetworkServerInfo", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("GameStateName", 0x00000029) },
		                { 0x001, new CMwEnumInfo("ServerFilterRaceType", new string[] { "", "Time Attack", "Rounds", "Team", "Laps", "Stunts", "Cup" }) },
		                { 0x002, new CMwFieldInfo("NbChallenges", 0x0000001F) },
		                { 0x003, new CMwFieldInfo("RaceStartTime", 0x0000001F) },
		                { 0x004, new CMwFieldInfo("TMRoundNum", 0x0000001F) },
		                { 0x005, new CMwEnumInfo("CurGameMode", new string[] { "Rounds", "Time Attack", "Team", "Laps", "Stunts", "Cup" }) },
		                { 0x006, new CMwFieldInfo("CurRoundPointsLimit", 0x0000001F) },
		                { 0x007, new CMwFieldInfo("CurRoundForcedLaps", 0x0000001F) },
		                { 0x008, new CMwFieldInfo("CurRoundUseNewRules", 0x00000001) },
		                { 0x009, new CMwFieldInfo("CurRoundPointsLimitNewRules", 0x0000001F) },
		                { 0x00A, new CMwFieldInfo("CurTeamPointsLimit", 0x0000001F) },
		                { 0x00B, new CMwFieldInfo("CurTeamMaxPoints", 0x0000001F) },
		                { 0x00C, new CMwFieldInfo("CurTeamUseNewRules", 0x00000001) },
		                { 0x00D, new CMwFieldInfo("CurTeamPointsLimitNewRules", 0x0000001F) },
		                { 0x00E, new CMwFieldInfo("CurTimeAttackLimit", 0x0000001F) },
		                { 0x00F, new CMwFieldInfo("CurTimeAttackSynchStartPeriod", 0x0000001F) },
		                { 0x010, new CMwFieldInfo("CurLapsNbLaps", 0x0000001F) },
		                { 0x011, new CMwFieldInfo("CurLapsTimeLimit", 0x0000001F) },
		                { 0x012, new CMwFieldInfo("CurEswcCupPointsLimit", 0x0000001F) },
		                { 0x013, new CMwFieldInfo("CurEswcCupRoundsPerChallenge", 0x0000001F) },
		                { 0x014, new CMwFieldInfo("CurEswcCupNbWinners", 0x0000001F) },
		                { 0x015, new CMwFieldInfo("CurEswcCupWarmUpDuration", 0x0000001F) },
		                { 0x016, new CMwFieldInfo("CurChatTime", 0x0000001F) },
		                { 0x017, new CMwFieldInfo("CurFinishTimeout", 0x0000001F) },
		                { 0x018, new CMwFieldInfo("CurAllWarmUpDuration", 0x0000001F) },
		                { 0x019, new CMwFieldInfo("CurDisableRespawn", 0x00000001) },
		                { 0x01A, new CMwFieldInfo("CurForceMaxOpponents", 0x0000001F) },
		                { 0x01B, new CMwEnumInfo("NextGameMode", new string[] { "Rounds", "Time Attack", "Team", "Laps", "Stunts", "Cup" }) },
		                { 0x01C, new CMwFieldInfo("NextRoundPointsLimit", 0x0000001F) },
		                { 0x01D, new CMwFieldInfo("NextRoundForcedLaps", 0x0000001F) },
		                { 0x01E, new CMwFieldInfo("NextRoundUseNewRules", 0x00000001) },
		                { 0x01F, new CMwFieldInfo("NextRoundPointsLimitNewRules", 0x0000001F) },
		                { 0x020, new CMwFieldInfo("NextTeamPointsLimit", 0x0000001F) },
		                { 0x021, new CMwFieldInfo("NextTeamMaxPoints", 0x0000001F) },
		                { 0x022, new CMwFieldInfo("NextTeamUseNewRules", 0x00000001) },
		                { 0x023, new CMwFieldInfo("NextTeamPointsLimitNewRules", 0x0000001F) },
		                { 0x024, new CMwFieldInfo("NextTimeAttackLimit", 0x0000001F) },
		                { 0x025, new CMwFieldInfo("NextTimeAttackSynchStartPeriod", 0x0000001F) },
		                { 0x026, new CMwFieldInfo("NextLapsNbLaps", 0x0000001F) },
		                { 0x027, new CMwFieldInfo("NextLapsTimeLimit", 0x0000001F) },
		                { 0x028, new CMwFieldInfo("NextEswcCupPointsLimit", 0x0000001F) },
		                { 0x029, new CMwFieldInfo("NextEswcCupRoundsPerChallenge", 0x0000001F) },
		                { 0x02A, new CMwFieldInfo("NextEswcCupNbWinners", 0x0000001F) },
		                { 0x02B, new CMwFieldInfo("NextEswcCupWarmUpDuration", 0x0000001F) },
		                { 0x02C, new CMwFieldInfo("NextChatTime", 0x0000001F) },
		                { 0x02D, new CMwFieldInfo("NextFinishTimeout", 0x0000001F) },
		                { 0x02E, new CMwFieldInfo("NextAllWarmUpDuration", 0x0000001F) },
		                { 0x02F, new CMwFieldInfo("NextDisableRespawn", 0x00000001) },
		                { 0x030, new CMwFieldInfo("NextForceMaxOpponents", 0x00000001) }
	                  })
	                },
	                { 0x036, new CMwClassInfo("CTrackManiaPlayerInfo", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("GameStateName", 0x00000029) },
		                { 0x001, new CMwEnumInfo("RaceState", new string[] { "BeforeStart", "Running", "Finished", "Eliminated" }) },
		                { 0x002, new CMwFieldInfo("TMRoundNum", 0x0000001F) },
		                { 0x003, new CMwFieldInfo("VehicleSpawnLoc", 0x00000013) },
		                { 0x004, new CMwFieldInfo("RaceStartTime", 0x0000001F) },
		                { 0x005, new CMwFieldInfo("RaceTime", 0x0000001F) },
		                { 0x006, new CMwFieldInfo("RaceBestTime", 0x0000001F) },
		                { 0x007, new CMwFieldInfo("StrRaceBestTime", 0x00000029) },
		                { 0x008, new CMwFieldInfo("StrTurnTime", 0x00000029) },
		                { 0x009, new CMwFieldInfo("LapStartTime", 0x0000001F) },
		                { 0x00A, new CMwFieldInfo("LapTime", 0x0000001F) },
		                { 0x00B, new CMwFieldInfo("LapBestTime", 0x0000001F) },
		                { 0x00C, new CMwFieldInfo("PrevRaceTime", 0x0000001F) },
		                { 0x00D, new CMwFieldInfo("StrPrevRaceTime", 0x00000029) },
		                { 0x00E, new CMwFieldInfo("StrPrevRaceScore", 0x00000029) },
		                { 0x00F, new CMwFieldInfo("MinRespawns", 0x0000001F) },
		                { 0x010, new CMwFieldInfo("CurLap", 0x0000001F) },
		                { 0x011, new CMwFieldInfo("NbCompleted", 0x0000001F) },
		                { 0x012, new CMwFieldInfo("MaxCompleted", 0x0000001F) },
		                { 0x013, new CMwFieldInfo("StuntsScore", 0x0000001F) },
		                { 0x014, new CMwFieldInfo("BestStuntsScore", 0x0000001F) },
		                { 0x015, new CMwFieldInfo("StrBestStuntsScore", 0x00000029) },
		                { 0x016, new CMwFieldInfo("CurCheckpoint", 0x0000001F) },
		                { 0x017, new CMwFieldInfo("CurCheckpointTime", 0x0000001F) },
		                { 0x018, new CMwFieldInfo("OffsetFromBestAtCurrentCP", 0x00000024) },
		                { 0x019, new CMwFieldInfo("StrOffsetFromBestAtCurrentCP_MmSsCc", 0x00000029) },
		                { 0x01A, new CMwFieldInfo("StrScoreOffsetFromBestAtCurrentCP", 0x00000029) },
		                { 0x01B, new CMwFieldInfo("CurrentRaceRank", 0x0000001F) },
		                { 0x01C, new CMwFieldInfo("StrCurrentRaceRank", 0x00000029) },
		                { 0x01D, new CMwFieldInfo("CurrentRoundRank", 0x0000001F) },
		                { 0x01E, new CMwFieldInfo("StrCurrentRoundRank", 0x00000029) },
		                { 0x01F, new CMwFieldInfo("Team", 0x0000001F) },
		                { 0x020, new CMwFieldInfo("AverageRank", 0x00000024) },
		                { 0x021, new CMwFieldInfo("CurrentTime", 0x0000001F) },
		                { 0x022, new CMwFieldInfo("ReadyToGoNext", 0x00000001) },
		                { 0x023, new CMwFieldInfo("ReadyEnum", 0x0000001F) }
	                  })
	                },
	                { 0x037, new CMwClassInfo("CTrackManiaRaceNetRounds", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("NbValidRounds", 0x0000001F) },
		                { 0x001, new CMwFieldInfo("CupWarmUpDuration", 0x0000001F) },
		                { 0x002, new CMwFieldInfo("CupRoundsPerChallenge", 0x0000001F) }
	                  })
	                },
	                { 0x03D, new CMwClassInfo("CTrackManiaEditorCatalog", new Dictionary<int, CMwMemberInfo>()
	                  {
	                  })
	                },
	                { 0x041, new CMwClassInfo("CTrackManiaMatchSettings", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("Comment", 0x0000002D) },
		                { 0x001, new CMwFieldInfo("IsSolo", 0x00000001) },
		                { 0x002, new CMwFieldInfo("IsHotSeat", 0x00000001) },
		                { 0x003, new CMwFieldInfo("IsLan", 0x00000001) },
		                { 0x004, new CMwFieldInfo("IsInternet", 0x00000001) },
		                { 0x005, new CMwFieldInfo("SortIndex", 0x0000001F) },
		                { 0x006, new CMwFieldInfo("RandomMapOrder", 0x00000001) },
		                { 0x007, new CMwFieldInfo("NbChallenges", 0x0000001F) },
		                { 0x008, new CMwEnumInfo("Network_GameMode", new string[] { "Rounds", "Time Attack", "Team", "Laps", "Stunts", "Cup" }) },
		                { 0x009, new CMwEnumInfo("HotSeat_GameMode", new string[] { "|HotSeat|Time", "|HotSeat|Tries" }) },
		                { 0x00A, new CMwFieldInfo("HotSeat_TimeLimit", 0x0000001F) },
		                { 0x00B, new CMwFieldInfo("HotSeat_Rounds", 0x0000001F) },
		                { 0x00C, new CMwFieldInfo("ChallengeInfos", 0x00000007) }
	                  })
	                },
	                { 0x042, new CMwClassInfo("CTrackManiaRace2PTurnBased", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("BestTime", 0x00000020) },
		                { 0x001, new CMwFieldInfo("BestTimeUid", 0x00000020) },
		                { 0x002, new CMwFieldInfo("BestTimeName", 0x0000002E) },
		                { 0x003, new CMwFieldInfo("PlayerTimeLeft1", 0x00000028) },
		                { 0x004, new CMwFieldInfo("PlayerTimeLeft2", 0x00000028) },
		                { 0x005, new CMwFieldInfo("PlayerTimeLeft3", 0x00000028) },
		                { 0x006, new CMwFieldInfo("PlayerTimeLeft4", 0x00000028) },
		                { 0x007, new CMwFieldInfo("PlayerTimeLeft5", 0x00000028) },
		                { 0x008, new CMwFieldInfo("PlayerTimeLeft6", 0x00000028) },
		                { 0x009, new CMwFieldInfo("PlayerTimeLeft7", 0x00000028) },
		                { 0x00A, new CMwFieldInfo("PlayerTimeLeft8", 0x00000028) },
		                { 0x00B, new CMwFieldInfo("PlayerName1", 0x0000002D) },
		                { 0x00C, new CMwFieldInfo("PlayerName2", 0x0000002D) },
		                { 0x00D, new CMwFieldInfo("PlayerName3", 0x0000002D) },
		                { 0x00E, new CMwFieldInfo("PlayerName4", 0x0000002D) },
		                { 0x00F, new CMwFieldInfo("PlayerName5", 0x0000002D) },
		                { 0x010, new CMwFieldInfo("PlayerName6", 0x0000002D) },
		                { 0x011, new CMwFieldInfo("PlayerName7", 0x0000002D) },
		                { 0x012, new CMwFieldInfo("PlayerName8", 0x0000002D) },
		                { 0x013, new CMwFieldInfo("PlayerBestScore1", 0x00000029) },
		                { 0x014, new CMwFieldInfo("PlayerBestScore2", 0x00000029) },
		                { 0x015, new CMwFieldInfo("PlayerBestScore3", 0x00000029) },
		                { 0x016, new CMwFieldInfo("PlayerBestScore4", 0x00000029) },
		                { 0x017, new CMwFieldInfo("PlayerBestScore5", 0x00000029) },
		                { 0x018, new CMwFieldInfo("PlayerBestScore6", 0x00000029) },
		                { 0x019, new CMwFieldInfo("PlayerBestScore7", 0x00000029) },
		                { 0x01A, new CMwFieldInfo("PlayerBestScore8", 0x00000029) },
		                { 0x01B, new CMwFieldInfo("PlayerGeneralPosition", 0x0000001F) }
	                  })
	                },
	                { 0x044, new CMwClassInfo("CTrackManiaRaceNet", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("GeneralScores", 0x00000007) },
		                { 0x001, new CMwFieldInfo("CurrentScores", 0x00000007) },
		                { 0x002, new CMwFieldInfo("TeamScores", 0x00000007) },
		                { 0x003, new CMwFieldInfo("PlayerGeneralPosition", 0x0000001F) },
		                { 0x004, new CMwFieldInfo("TimeAttackTargetScore_CheckpointsTime", 0x00000024) },
		                { 0x005, new CMwFieldInfo("TimeAttackTargetScore_RaceTime", 0x00000024) },
		                { 0x006, new CMwFieldInfo("TimeAttackTargetScore_SpeedMin", 0x00000024) },
		                { 0x007, new CMwFieldInfo("TimeAttackTargetScore_InWater", 0x00000024) },
		                { 0x008, new CMwFieldInfo("TimeAttackTargetScore_RemainingCheckpoints", 0x00000024) },
		                { 0x009, new CMwFieldInfo("TimeAttackTargetScore_RemainingTime", 0x00000024) },
		                { 0x00A, new CMwFieldInfo("TimeAttackTargetScore_Team", 0x00000024) },
		                { 0x00B, new CMwFieldInfo("TimeAttackTargetScore_Delayed", 0x00000024) },
		                { 0x00C, new CMwFieldInfo("TimeAttackTargetScore_MustBeAfter", 0x00000024) },
		                { 0x00D, new CMwFieldInfo("TimeAttackTargetScore_AverageRank", 0x00000024) },
		                { 0x00E, new CMwFieldInfo("TimeAttackTargetScore_FirstTurn", 0x00000024) },
		                { 0x00F, new CMwFieldInfo("RoundsTargetScore_CheckpointsTime", 0x00000024) },
		                { 0x010, new CMwFieldInfo("RoundsTargetScore_RaceTime", 0x00000024) },
		                { 0x011, new CMwFieldInfo("RoundsTargetScore_SpeedMin", 0x00000024) },
		                { 0x012, new CMwFieldInfo("RoundsTargetScore_InWater", 0x00000024) },
		                { 0x013, new CMwFieldInfo("RoundsTargetScore_RemainingCheckpoints", 0x00000024) },
		                { 0x014, new CMwFieldInfo("RoundsTargetScore_RemainingTime", 0x00000024) },
		                { 0x015, new CMwFieldInfo("RoundsTargetScore_Team", 0x00000024) },
		                { 0x016, new CMwFieldInfo("RoundsTargetScore_Delayed", 0x00000024) },
		                { 0x017, new CMwFieldInfo("RoundsTargetScore_MustBeAfter", 0x00000024) },
		                { 0x018, new CMwFieldInfo("RoundsTargetScore_AverageRank", 0x00000024) },
		                { 0x019, new CMwFieldInfo("RoundsTargetScore_FirstTurn", 0x00000024) },
		                { 0x01A, new CMwFieldInfo("LapsTargetScore_CheckpointsTime", 0x00000024) },
		                { 0x01B, new CMwFieldInfo("LapsTargetScore_RaceTime", 0x00000024) },
		                { 0x01C, new CMwFieldInfo("LapsTargetScore_SpeedMin", 0x00000024) },
		                { 0x01D, new CMwFieldInfo("LapsTargetScore_InWater", 0x00000024) },
		                { 0x01E, new CMwFieldInfo("LapsTargetScore_RemainingLaps", 0x00000024) },
		                { 0x01F, new CMwFieldInfo("LapsTargetScore_Team", 0x00000024) },
		                { 0x020, new CMwFieldInfo("LapsTargetScore_Delayed", 0x00000024) },
		                { 0x021, new CMwFieldInfo("LapsTargetScore_MustBeAfter", 0x00000024) },
		                { 0x022, new CMwFieldInfo("LapsTargetScore_AverageRank", 0x00000024) },
		                { 0x023, new CMwFieldInfo("LapsTargetScore_FirstTurn", 0x00000024) },
		                { 0x024, new CMwFieldInfo("StuntsTargetScore_CheckpointsScore", 0x00000024) },
		                { 0x025, new CMwFieldInfo("StuntsTargetScore_RaceScore", 0x00000024) },
		                { 0x026, new CMwFieldInfo("StuntsTargetScore_SpeedMin", 0x00000024) },
		                { 0x027, new CMwFieldInfo("StuntsTargetScore_InWater", 0x00000024) },
		                { 0x028, new CMwFieldInfo("StuntsTargetScore_RemainingCheckpoints", 0x00000024) },
		                { 0x029, new CMwFieldInfo("StuntsTargetScore_Team", 0x00000024) },
		                { 0x02A, new CMwFieldInfo("StuntsTargetScore_RemainingScore", 0x00000024) },
		                { 0x02B, new CMwFieldInfo("StuntsTargetScore_MustBeAfter", 0x00000024) },
		                { 0x02C, new CMwFieldInfo("StuntsTargetScore_AverageRank", 0x00000024) },
		                { 0x02D, new CMwFieldInfo("StuntsTargetScore_FirstTurn", 0x00000024) },
		                { 0x02E, new CMwFieldInfo("MinTargetSpeed", 0x00000024) },
		                { 0x02F, new CMwFieldInfo("ThresholdChallengerTime", 0x00000024) },
		                { 0x030, new CMwFieldInfo("ThresholdChallengerScore", 0x00000024) },
		                { 0x031, new CMwFieldInfo("ThresholdTargetScore", 0x00000024) },
		                { 0x032, new CMwFieldInfo("ThresholdNoMoveTime", 0x0000001F) },
		                { 0x033, new CMwFieldInfo("MinTargetTime", 0x0000001F) },
		                { 0x034, new CMwFieldInfo("AcceptedDelayedDelay", 0x00000024) },
		                { 0x035, new CMwFieldInfo("AcceptedRemainingDelay", 0x00000024) },
		                { 0x036, new CMwFieldInfo("RemainingScoreRatio", 0x00000024) },
		                { 0x037, new CMwFieldInfo("RatioToBeBetter", 0x00000024) },
		                { 0x038, new CMwFieldInfo("ReadyToGoNext", 0x00000001) },
		                { 0x039, new CMwMethodInfo("RaceInputsValidateBest", 0x00000000, null, null) }
	                  })
	                },
	                { 0x045, new CMwClassInfo("CTrackManiaRaceNetTimeAttack", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("Bidon", 0x0000001F) }
	                  })
	                },
	                { 0x051, new CMwClassInfo("CTrackManiaEditorInterface", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("InterfaceScene", 0x00000005) },
		                { 0x001, new CMwFieldInfo("Editor", 0x00000005) },
		                { 0x002, new CMwFieldInfo("SubIconOffset", 0x00000024) },
		                { 0x003, new CMwFieldInfo("SubIconSizeFactor", 0x00000024) },
		                { 0x004, new CMwMethodInfo("ToggleBlockRotation", 0x00000000, null, null) },
		                { 0x005, new CMwMethodInfo("UpdateSubButtonsSize", 0x00000000, null, null) },
		                { 0x006, new CMwFieldInfo("Pages", 0x00000007) },
		                { 0x007, new CMwFieldInfo("CurrentToolTip", 0x0000002D) },
		                { 0x008, new CMwFieldInfo("Allocated", 0x00000029) },
		                { 0x009, new CMwMethodInfo("FrameEditSnapCamera_OnOk", 0x00000000, null, null) },
		                { 0x00A, new CMwMethodInfo("FrameEditSnapCamera_OnCancel", 0x00000000, null, null) },
		                { 0x00B, new CMwFieldInfo("FrameEditSnapCamera_BitmapSnap", 0x00000005) }
	                  })
	                },
	                { 0x056, new CMwClassInfo("CTrackManiaRaceNetLaps", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("NbPlayersRacing", 0x0000001F) },
		                { 0x001, new CMwMethodInfo("ComputeScores", 0x00000000, null, null) }
	                  })
	                },
	                { 0x057, new CMwClassInfo("CTrackManiaEditorIcon", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("Article", 0x00000005) },
		                { 0x001, new CMwFieldInfo("Page", 0x00000005) },
		                { 0x002, new CMwFieldInfo("MotherPage", 0x00000005) }
	                  })
	                },
	                { 0x058, new CMwClassInfo("CTrackManiaEditorIconPage", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("Icons", 0x00000007) },
		                { 0x001, new CMwFieldInfo("PageName", 0x00000029) },
		                { 0x002, new CMwFieldInfo("MotherIcon", 0x00000005) },
		                { 0x003, new CMwFieldInfo("NbSubPages", 0x0000001F) }
	                  })
	                },
	                { 0x06E, new CMwClassInfo("CTrackManiaControlCheckPointList", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("StyleName", 0x00000005) },
		                { 0x001, new CMwFieldInfo("StyleRank", 0x00000005) },
		                { 0x002, new CMwFieldInfo("StyleTime", 0x00000005) },
		                { 0x003, new CMwFieldInfo("CardModel", 0x00000005) }
	                  })
	                },
	                { 0x078, new CMwClassInfo("CTrackManiaRace1PGhosts", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("MedalGhosts", 0x00000006) }
	                  })
	                },
	                { 0x07E, new CMwClassInfo("CTrackManiaReplayRecord", new Dictionary<int, CMwMemberInfo>()
	                  {
	                  })
	                },
	                { 0x07F, new CMwClassInfo("CCtnMediaBlockEventTrackMania", new Dictionary<int, CMwMemberInfo>()
	                  {
	                  })
	                },
	                { 0x080, new CMwClassInfo("CTrackManiaPlayerCameraSet", new Dictionary<int, CMwMemberInfo>()
	                  {
	                  })
	                },
	                { 0x085, new CMwClassInfo("CGameControlCameraTrackManiaRace", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("ConeAperture", 0x00000024) },
		                { 0x001, new CMwFieldInfo("CarCameraHeight", 0x00000024) },
		                { 0x002, new CMwFieldInfo("CarCameraDistance", 0x00000024) },
		                { 0x003, new CMwFieldInfo("CarCameraTargetDistance", 0x00000024) },
		                { 0x004, new CMwFieldInfo("CarCameraAlign", 0x00000024) },
		                { 0x005, new CMwFieldInfo("ConeMinSpeed", 0x00000024) },
		                { 0x006, new CMwFieldInfo("ConeMaxSpeed", 0x00000024) },
		                { 0x007, new CMwFieldInfo("UseSpeedDir", 0x00000001) },
		                { 0x008, new CMwFieldInfo("IsSegmentCast", 0x00000001) },
		                { 0x009, new CMwFieldInfo("SegmentCastMinDist", 0x00000024) },
		                { 0x00A, new CMwFieldInfo("SegmentCastLength", 0x00000024) }
	                  })
	                },
	                { 0x086, new CMwClassInfo("CGameControlCameraTrackManiaRace2", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("DeltaDist", 0x00000024) },
		                { 0x001, new CMwFieldInfo("DeltaDistDamperKi", 0x00000024) },
		                { 0x002, new CMwFieldInfo("DeltaDistDamperKa", 0x00000024) },
		                { 0x003, new CMwFieldInfo("LastDeltaPlaneDist", 0x00000024) },
		                { 0x004, new CMwFieldInfo("MaxDeltaPlaneDistStep", 0x00000024) },
		                { 0x005, new CMwFieldInfo("LastDeltaFov", 0x00000024) },
		                { 0x006, new CMwFieldInfo("MaxDeltaFovStep", 0x00000024) },
		                { 0x007, new CMwFieldInfo("DeltaLookAtFactor", 0x00000024) },
		                { 0x008, new CMwFieldInfo("LookAtFactorFromUpSpeedRatio", 0x00000005) },
		                { 0x009, new CMwFieldInfo("StateFlyingLookAtStep", 0x00000024) },
		                { 0x00A, new CMwFieldInfo("DeltaLookAtFactorDamperKi", 0x00000024) },
		                { 0x00B, new CMwFieldInfo("DeltaLookAtFactorDamperKa", 0x00000024) },
		                { 0x00C, new CMwFieldInfo("LastRollAngle", 0x00000024) },
		                { 0x00D, new CMwFieldInfo("LastYawAngle", 0x00000024) },
		                { 0x00E, new CMwFieldInfo("RollFromSpeed", 0x00000005) },
		                { 0x00F, new CMwFieldInfo("YawFromSpeed", 0x00000005) },
		                { 0x010, new CMwFieldInfo("InputGasDistDelta", 0x00000024) },
		                { 0x011, new CMwFieldInfo("InputGasDistTimeUp", 0x0000001F) },
		                { 0x012, new CMwFieldInfo("InputGasDistTimeDown", 0x0000001F) },
		                { 0x013, new CMwFieldInfo("InputBrakeDistDelta", 0x00000024) },
		                { 0x014, new CMwFieldInfo("InputBrakeDistTimeUp", 0x0000001F) },
		                { 0x015, new CMwFieldInfo("InputBrakeDistTimeDown", 0x0000001F) },
		                { 0x016, new CMwFieldInfo("InputSteerDistDelta", 0x00000024) },
		                { 0x017, new CMwFieldInfo("InputSteerDistTimeUp", 0x0000001F) },
		                { 0x018, new CMwFieldInfo("InputSteerDistTimeDown", 0x0000001F) },
		                { 0x019, new CMwFieldInfo("EventTurboFovDelta", 0x00000024) },
		                { 0x01A, new CMwFieldInfo("EventTurboFovTimeUp", 0x0000001F) },
		                { 0x01B, new CMwFieldInfo("EventTurboFovTimeDown", 0x0000001F) },
		                { 0x01C, new CMwFieldInfo("EventTurboDistDelta", 0x00000024) },
		                { 0x01D, new CMwFieldInfo("EventTurboDistTimeUp", 0x0000001F) },
		                { 0x01E, new CMwFieldInfo("EventTurboDistTimeDown", 0x0000001F) },
		                { 0x01F, new CMwFieldInfo("EventChangeGearDistDelta", 0x00000024) },
		                { 0x020, new CMwFieldInfo("EventChangeGearDistTimeUp", 0x0000001F) },
		                { 0x021, new CMwFieldInfo("EventChangeGearDistTimeDown", 0x0000001F) },
		                { 0x022, new CMwFieldInfo("EventBurningLookAtFactorDelta", 0x00000024) },
		                { 0x023, new CMwFieldInfo("EventBurningLookAtFactorTimeUp", 0x0000001F) },
		                { 0x024, new CMwFieldInfo("EventBurningLookAtFactorTimeDown", 0x0000001F) },
		                { 0x025, new CMwFieldInfo("EventBurningDistDelta", 0x00000024) },
		                { 0x026, new CMwFieldInfo("EventBurningDistTimeUp", 0x0000001F) },
		                { 0x027, new CMwFieldInfo("EventBurningDistTimeDown", 0x0000001F) },
		                { 0x028, new CMwFieldInfo("StateFlyingDistDelta", 0x00000024) },
		                { 0x029, new CMwFieldInfo("StateFlyingDistTimeUp", 0x0000001F) },
		                { 0x02A, new CMwFieldInfo("StateFlyingDistTimeDown", 0x0000001F) },
		                { 0x02B, new CMwFieldInfo("StateFlyingPlaneDistDelta", 0x00000024) },
		                { 0x02C, new CMwFieldInfo("StateFlyingPlaneDistTimeUp", 0x0000001F) },
		                { 0x02D, new CMwFieldInfo("StateFlyingPlaneDistTimeDown", 0x0000001F) },
		                { 0x02E, new CMwFieldInfo("StateFlyingLookAtFactorDelta", 0x00000024) },
		                { 0x02F, new CMwFieldInfo("StateFlyingLookAtFactorTimeUp", 0x0000001F) },
		                { 0x030, new CMwFieldInfo("StateFlyingLookAtFactorTimeDown", 0x0000001F) },
		                { 0x031, new CMwFieldInfo("InputLeftSteerRollDelta", 0x00000024) },
		                { 0x032, new CMwFieldInfo("InputLeftSteerRollTimeUp", 0x0000001F) },
		                { 0x033, new CMwFieldInfo("InputLeftSteerRollTimeDown", 0x0000001F) },
		                { 0x034, new CMwFieldInfo("InputRightSteerRollDelta", 0x00000024) },
		                { 0x035, new CMwFieldInfo("InputRightSteerRollTimeUp", 0x0000001F) },
		                { 0x036, new CMwFieldInfo("InputRightSteerRollTimeDown", 0x0000001F) },
		                { 0x037, new CMwFieldInfo("InputLeftSteerYawDelta", 0x00000024) },
		                { 0x038, new CMwFieldInfo("InputLeftSteerYawTimeUp", 0x0000001F) },
		                { 0x039, new CMwFieldInfo("InputLeftSteerYawTimeDown", 0x0000001F) },
		                { 0x03A, new CMwFieldInfo("InputRightSteerYawDelta", 0x00000024) },
		                { 0x03B, new CMwFieldInfo("InputRightSteerYawTimeUp", 0x0000001F) },
		                { 0x03C, new CMwFieldInfo("InputRightSteerYawTimeDown", 0x0000001F) },
		                { 0x03D, new CMwFieldInfo("MinSpeed", 0x00000024) },
		                { 0x03E, new CMwFieldInfo("InputSteerDurationBeforeRollTriggered", 0x0000001F) },
		                { 0x03F, new CMwFieldInfo("InputSteerDurationBeforeAnticipatingTurnTriggered", 0x0000001F) },
		                { 0x040, new CMwFieldInfo("InputSteerDurationBeforeBurnoutShowView", 0x0000001F) },
		                { 0x041, new CMwFieldInfo("InputNoSteerDurationBeforeReset", 0x0000001F) },
		                { 0x042, new CMwFieldInfo("StateFlyingDurationBeforeCameraMove", 0x0000001F) },
		                { 0x043, new CMwFieldInfo("StateFlyingDurationBeforeFlyingMode", 0x0000001F) },
		                { 0x044, new CMwFieldInfo("IsRollFromInput", 0x00000001) }
	                  })
	                },
	                { 0x087, new CMwClassInfo("CGameControlCameraTrackManiaRace3", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("StickToIdeal", 0x00000001) },
		                { 0x001, new CMwFieldInfo("Up", 0x00000024) },
		                { 0x002, new CMwFieldInfo("Far", 0x00000024) },
		                { 0x003, new CMwFieldInfo("RadiusDamperKi", 0x00000024) },
		                { 0x004, new CMwFieldInfo("RadiusDamperKa", 0x00000024) },
		                { 0x005, new CMwFieldInfo("SlerpSpeedModulationFromSpeed", 0x00000005) },
		                { 0x006, new CMwFieldInfo("SlerpSpeed", 0x00000024) },
		                { 0x007, new CMwFieldInfo("SlerpSpeedFlyingBehavior", 0x00000024) },
		                { 0x008, new CMwFieldInfo("SlerpSpeedCamUp", 0x00000024) },
		                { 0x009, new CMwFieldInfo("SlerpSpeedCamUpFlyingBehavior", 0x00000024) },
		                { 0x00A, new CMwFieldInfo("LookAtFactorFromUpSpeedRatio", 0x00000005) },
		                { 0x00B, new CMwFieldInfo("StateFlyingLookAtStep", 0x00000024) },
		                { 0x00C, new CMwFieldInfo("FlyingLookDownFactorFromSpeedRatio", 0x00000005) },
		                { 0x00D, new CMwFieldInfo("ConstantFlyingLookDownFactor", 0x00000024) },
		                { 0x00E, new CMwFieldInfo("FlyingLookDownFactorKi", 0x00000024) },
		                { 0x00F, new CMwFieldInfo("FlyingLookDownFactorKa", 0x00000024) },
		                { 0x010, new CMwFieldInfo("SlerpTargetCamUpDelta", 0x00000024) },
		                { 0x011, new CMwFieldInfo("SlerpTargetCamUpTimeUp", 0x0000001F) },
		                { 0x012, new CMwFieldInfo("SlerpTargetCamUpTimeDown", 0x0000001F) },
		                { 0x013, new CMwFieldInfo("SlerpTargetPosDelta", 0x00000024) },
		                { 0x014, new CMwFieldInfo("SlerpTargetPosTimeUp", 0x0000001F) },
		                { 0x015, new CMwFieldInfo("SlerpTargetPosTimeDown", 0x0000001F) },
		                { 0x016, new CMwFieldInfo("SlerpTargetPosNormalBehaviorDelta", 0x00000024) },
		                { 0x017, new CMwFieldInfo("SlerpTargetPosNormalBehaviorTimeUp", 0x0000001F) },
		                { 0x018, new CMwFieldInfo("SlerpTargetPosNormalBehaviorTimeDown", 0x0000001F) },
		                { 0x019, new CMwFieldInfo("SlerpTargetPosFlyingDelta", 0x00000024) },
		                { 0x01A, new CMwFieldInfo("SlerpTargetPosFlyingTimeUp", 0x0000001F) },
		                { 0x01B, new CMwFieldInfo("SlerpTargetPosFlyingTimeDown", 0x0000001F) },
		                { 0x01C, new CMwFieldInfo("SlerpSpeedDelta", 0x00000024) },
		                { 0x01D, new CMwFieldInfo("SlerpSpeedTimeUp", 0x0000001F) },
		                { 0x01E, new CMwFieldInfo("SlerpSpeedTimeDown", 0x0000001F) },
		                { 0x01F, new CMwFieldInfo("SlerpSpeedCamUpDelta", 0x00000024) },
		                { 0x020, new CMwFieldInfo("SlerpSpeedCamUpTimeUp", 0x0000001F) },
		                { 0x021, new CMwFieldInfo("SlerpSpeedCamUpTimeDown", 0x0000001F) },
		                { 0x022, new CMwFieldInfo("InputGasFarDelta", 0x00000024) },
		                { 0x023, new CMwFieldInfo("InputGasFarTimeUp", 0x0000001F) },
		                { 0x024, new CMwFieldInfo("InputGasFarTimeDown", 0x0000001F) },
		                { 0x025, new CMwFieldInfo("InputBrakeFarDelta", 0x00000024) },
		                { 0x026, new CMwFieldInfo("InputBrakeFarTimeUp", 0x0000001F) },
		                { 0x027, new CMwFieldInfo("InputBrakeFarTimeDown", 0x0000001F) },
		                { 0x028, new CMwFieldInfo("InputSteerFarDelta", 0x00000024) },
		                { 0x029, new CMwFieldInfo("InputSteerFarTimeUp", 0x0000001F) },
		                { 0x02A, new CMwFieldInfo("InputSteerFarTimeDown", 0x0000001F) },
		                { 0x02B, new CMwFieldInfo("EventTurboFovDelta", 0x00000024) },
		                { 0x02C, new CMwFieldInfo("EventTurboFovTimeUp", 0x0000001F) },
		                { 0x02D, new CMwFieldInfo("EventTurboFovTimeDown", 0x0000001F) },
		                { 0x02E, new CMwFieldInfo("EventTurboFarDelta", 0x00000024) },
		                { 0x02F, new CMwFieldInfo("EventTurboFarTimeUp", 0x0000001F) },
		                { 0x030, new CMwFieldInfo("EventTurboFarTimeDown", 0x0000001F) },
		                { 0x031, new CMwFieldInfo("EventChangeGearFarDelta", 0x00000024) },
		                { 0x032, new CMwFieldInfo("EventChangeGearFarTimeUp", 0x0000001F) },
		                { 0x033, new CMwFieldInfo("EventChangeGearFarTimeDown", 0x0000001F) },
		                { 0x034, new CMwFieldInfo("EventBurningLookAtFactorDelta", 0x00000024) },
		                { 0x035, new CMwFieldInfo("EventBurningLookAtFactorTimeUp", 0x0000001F) },
		                { 0x036, new CMwFieldInfo("EventBurningLookAtFactorTimeDown", 0x0000001F) },
		                { 0x037, new CMwFieldInfo("EventBurningRadiusDelta", 0x00000024) },
		                { 0x038, new CMwFieldInfo("EventBurningRadiusTimeUp", 0x0000001F) },
		                { 0x039, new CMwFieldInfo("EventBurningRadiusTimeDown", 0x0000001F) },
		                { 0x03A, new CMwFieldInfo("StateFlyingRadiusDelta", 0x00000024) },
		                { 0x03B, new CMwFieldInfo("StateFlyingRadiusTimeUp", 0x0000001F) },
		                { 0x03C, new CMwFieldInfo("StateFlyingRadiusTimeDown", 0x0000001F) },
		                { 0x03D, new CMwFieldInfo("StateFlyingLookAtFactorDelta", 0x00000024) },
		                { 0x03E, new CMwFieldInfo("StateFlyingLookAtFactorTimeUp", 0x0000001F) },
		                { 0x03F, new CMwFieldInfo("StateFlyingLookAtFactorTimeDown", 0x0000001F) },
		                { 0x040, new CMwFieldInfo("MinSpeed", 0x00000024) },
		                { 0x041, new CMwFieldInfo("MinSpeed2", 0x00000024) },
		                { 0x042, new CMwFieldInfo("FlyDurationBeforeFlyingBehavior", 0x0000001F) },
		                { 0x043, new CMwFieldInfo("InputNoSteerDurationBeforeReset", 0x0000001F) },
		                { 0x044, new CMwFieldInfo("InputSteerDurationBeforeBurnoutShowView", 0x0000001F) }
	                  })
	                },
	                { 0x08C, new CMwClassInfo("CTrackManiaControlPlayerInfoCard", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("ShowBasedTimeInfosInRounds", 0x00000001) },
		                { 0x001, new CMwFieldInfo("Avatar", 0x00000005) },
		                { 0x002, new CMwFieldInfo("ReadyToGoNext", 0x00000001) },
		                { 0x003, new CMwFieldInfo("StrPlayerName", 0x0000002D) },
		                { 0x004, new CMwFieldInfo("StrLadderTeamName", 0x00000029) },
		                { 0x005, new CMwFieldInfo("StrFamePoints", 0x0000001F) },
		                { 0x006, new CMwFieldInfo("StrFame", 0x0000001F) },
		                { 0x007, new CMwFieldInfo("StrLadderRankingSimple", 0x0000001F) },
		                { 0x008, new CMwFieldInfo("StrLadderScore", 0x00000024) },
		                { 0x009, new CMwFieldInfo("StrLadderRaceScore", 0x00000029) },
		                { 0x00A, new CMwFieldInfo("StrCurrentRaceRank", 0x00000029) },
		                { 0x00B, new CMwFieldInfo("SpectatorMode", 0x0000001F) },
		                { 0x00C, new CMwFieldInfo("NbSpectators", 0x0000001F) },
		                { 0x00D, new CMwFieldInfo("StrTotalRoundScore", 0x00000029) },
		                { 0x00E, new CMwFieldInfo("StrLastTotalRoundScore", 0x00000029) },
		                { 0x00F, new CMwFieldInfo("StrLastRaceRoundScore", 0x00000029) },
		                { 0x010, new CMwFieldInfo("StrBestTimeOrScore", 0x00000029) },
		                { 0x011, new CMwFieldInfo("StrPlayerRaceBestTimeOrScoreDelay", 0x00000029) },
		                { 0x012, new CMwFieldInfo("StrPlayerBestRaceTime", 0x00000029) },
		                { 0x013, new CMwFieldInfo("StrPlayerBestRaceScore", 0x00000029) },
		                { 0x014, new CMwFieldInfo("StrRaceBestTime", 0x00000029) },
		                { 0x015, new CMwFieldInfo("StrPrevRaceTimeOrScore", 0x00000029) },
		                { 0x016, new CMwFieldInfo("StrOffsetFromBestAtCurrentCP_MmSsCc", 0x00000029) },
		                { 0x017, new CMwFieldInfo("StrTurnTime", 0x00000029) },
		                { 0x018, new CMwFieldInfo("StrRaceBestScore", 0x00000029) },
		                { 0x019, new CMwFieldInfo("StrScoreOffsetFromBestAtCurrentCP", 0x00000029) }
	                  })
	                },
	                { 0x08F, new CMwClassInfo("CTrackManiaControlCard", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("Race", 0x00000005) },
		                { 0x001, new CMwFieldInfo("IsStuntsMode", 0x00000001) },
		                { 0x002, new CMwFieldInfo("IsRoundsMode", 0x00000001) }
	                  })
	                },
	                { 0x090, new CMwClassInfo("CControlTrackManiaTeamCard", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("Team", 0x0000001F) },
		                { 0x001, new CMwFieldInfo("StrTeamName", 0x0000002D) },
		                { 0x002, new CMwFieldInfo("StrTeamScore", 0x00000029) }
	                  })
	                },
	                { 0x092, new CMwClassInfo("CCtnMediaBlockUiTMSimpleEvtsDisplay", new Dictionary<int, CMwMemberInfo>()
	                  {
	                  })
	                },
	                { 0x09C, new CMwClassInfo("CTrackManiaMatchSettingsControlGrid", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("PlayList", 0x00000005) },
		                { 0x001, new CMwFieldInfo("PlayListNameLabel", 0x00000005) },
		                { 0x002, new CMwFieldInfo("PlayListCommentLabel", 0x00000005) },
		                { 0x003, new CMwFieldInfo("ChallengeCardTemplate", 0x00000005) },
		                { 0x004, new CMwFieldInfo("MaxChallengeInfosPerColumn", 0x0000001F) },
		                { 0x005, new CMwFieldInfo("MaxChallengeInfosPerRow", 0x0000001F) },
		                { 0x006, new CMwFieldInfo("CurrentPage", 0x0000001F) },
		                { 0x007, new CMwFieldInfo("NbPage", 0x0000001F) },
		                { 0x008, new CMwFieldInfo("ButtonNextPage", 0x00000005) },
		                { 0x009, new CMwFieldInfo("ButtonPreviousPage", 0x00000005) },
		                { 0x00A, new CMwMethodInfo("OnNextPage", 0x00000000, null, null) },
		                { 0x00B, new CMwMethodInfo("OnPreviousPage", 0x00000000, null, null) }
	                  })
	                },
	                { 0x09F, new CMwClassInfo("CTrackManiaRaceScore", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("Score", 0x0000001F) },
		                { 0x001, new CMwFieldInfo("LastRaceScore", 0x0000001F) },
		                { 0x002, new CMwFieldInfo("LastRaceTime", 0x0000001F) },
		                { 0x003, new CMwFieldInfo("BestTime", 0x0000001F) },
		                { 0x004, new CMwFieldInfo("BestStuntsScore", 0x0000001F) },
		                { 0x005, new CMwFieldInfo("LapsNbCheckpoint", 0x0000001F) },
		                { 0x006, new CMwFieldInfo("RaceInputsDuration", 0x0000001F) },
		                { 0x007, new CMwFieldInfo("RaceInputsValidationSeed", 0x0000001F) },
		                { 0x008, new CMwFieldInfo("BestRaceInputsTimeStartInReplay", 0x0000001F) },
		                { 0x009, new CMwFieldInfo("RaceInputsTimeOrScore", 0x0000001F) },
		                { 0x00A, new CMwFieldInfo("LadderScore", 0x00000024) },
		                { 0x00B, new CMwFieldInfo("PlayerInfo", 0x00000005) }
	                  })
	                },
	                { 0x0B5, new CMwClassInfo("CTrackManiaPlayerProfile", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("NbCollections", 0x0000001F) },
		                { 0x001, new CMwFieldInfo("ClickedOnShare", 0x0000001F) },
		                { 0x002, new CMwFieldInfo("CustomCampaigns", 0x00000007) },
		                { 0x003, new CMwFieldInfo("IsDisplayRaceHelp", 0x00000003) }
	                  })
	                },
	                { 0x0BE, new CMwClassInfo("CTrackManiaControlRaceScoreCard", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("StrName", 0x0000002D) },
		                { 0x001, new CMwFieldInfo("StrLogin", 0x00000029) },
		                { 0x002, new CMwFieldInfo("StrRank", 0x00000029) },
		                { 0x003, new CMwFieldInfo("StrRankInRace", 0x00000029) },
		                { 0x004, new CMwFieldInfo("StrScore", 0x00000029) },
		                { 0x005, new CMwFieldInfo("StrCheckpoint", 0x00000029) },
		                { 0x006, new CMwFieldInfo("StrLadderScore", 0x00000029) },
		                { 0x007, new CMwFieldInfo("LadderRank", 0x0000002D) },
		                { 0x008, new CMwFieldInfo("StrLastRaceTime", 0x00000029) },
		                { 0x009, new CMwFieldInfo("StrBestTime", 0x00000029) },
		                { 0x00A, new CMwFieldInfo("StrLastRaceScore", 0x00000029) },
		                { 0x00B, new CMwFieldInfo("StrBestStuntsScore", 0x00000029) }
	                  })
	                },
	                { 0x0BF, new CMwClassInfo("CTrackManiaControlScores", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwEnumInfo("Mode", new string[] { "TimeAttack", "Rounds", "Stunts", "Teams", "Laps" }) },
		                { 0x001, new CMwFieldInfo("WithSpecPlayers", 0x00000001) },
		                { 0x002, new CMwFieldInfo("OnlyArrivedPlayers", 0x00000001) },
		                { 0x003, new CMwFieldInfo("Scores", 0x00000007) },
		                { 0x004, new CMwFieldInfo("RedTeamScore", 0x00000005) },
		                { 0x005, new CMwFieldInfo("BlueTeamScore", 0x00000005) },
		                { 0x006, new CMwFieldInfo("RedTeamPlayersScores", 0x00000007) },
		                { 0x007, new CMwFieldInfo("BlueTeamPlayersScores", 0x00000007) },
		                { 0x008, new CMwFieldInfo("GridTitles", 0x00000005) },
		                { 0x009, new CMwFieldInfo("GridScores", 0x00000005) },
		                { 0x00A, new CMwFieldInfo("GridFocusedScore", 0x00000005) },
		                { 0x00B, new CMwFieldInfo("GridRedTeamScore", 0x00000005) },
		                { 0x00C, new CMwFieldInfo("GridBlueTeamScore", 0x00000005) },
		                { 0x00D, new CMwFieldInfo("GridRedTeamPlayersScores", 0x00000005) },
		                { 0x00E, new CMwFieldInfo("GridBlueTeamPlayersScores", 0x00000005) },
		                { 0x00F, new CMwMethodInfo("InitGridTitles", 0x00000000, null, null) },
		                { 0x010, new CMwMethodInfo("InitGridScores", 0x00000000, null, null) },
		                { 0x011, new CMwMethodInfo("InitGridFocusedScore", 0x00000000, null, null) },
		                { 0x012, new CMwMethodInfo("InitGridRedTeamScore", 0x00000000, null, null) },
		                { 0x013, new CMwMethodInfo("InitGridBlueTeamScore", 0x00000000, null, null) },
		                { 0x014, new CMwMethodInfo("InitGridRedTeamPlayersScores", 0x00000000, null, null) },
		                { 0x015, new CMwMethodInfo("InitGridBlueTeamPlayersScores", 0x00000000, null, null) },
		                { 0x016, new CMwMethodInfo("UpdateGridTitles", 0x00000000, null, null) },
		                { 0x017, new CMwMethodInfo("UpdateGridScores", 0x00000000, null, null) },
		                { 0x018, new CMwMethodInfo("UpdateGridFocusedScore", 0x00000000, null, null) },
		                { 0x019, new CMwMethodInfo("UpdateGridRedTeamScore", 0x00000000, null, null) },
		                { 0x01A, new CMwMethodInfo("UpdateGridBlueTeamScore", 0x00000000, null, null) },
		                { 0x01B, new CMwMethodInfo("UpdateGridRedTeamPlayersScores", 0x00000000, null, null) },
		                { 0x01C, new CMwMethodInfo("UpdateGridBlueTeamPlayersScores", 0x00000000, null, null) },
		                { 0x01D, new CMwMethodInfo("CleanGridTitles", 0x00000000, null, null) },
		                { 0x01E, new CMwMethodInfo("CleanGridScores", 0x00000000, null, null) },
		                { 0x01F, new CMwMethodInfo("CleanGridFocusedScore", 0x00000000, null, null) },
		                { 0x020, new CMwMethodInfo("CleanGridRedTeamScore", 0x00000000, null, null) },
		                { 0x021, new CMwMethodInfo("CleanGridBlueTeamScore", 0x00000000, null, null) },
		                { 0x022, new CMwMethodInfo("CleanGridRedTeamPlayersScores", 0x00000000, null, null) },
		                { 0x023, new CMwMethodInfo("CleanGridBlueTeamPlayersScores", 0x00000000, null, null) },
		                { 0x024, new CMwMethodInfo("ResetScores", 0x00000000, null, null) }
	                  })
	                },
	                { 0x0C4, new CMwClassInfo("CTrackManiaControlMatchSettingsCard", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("StrName", 0x0000002D) },
		                { 0x001, new CMwFieldInfo("StrComment", 0x0000002D) },
		                { 0x002, new CMwFieldInfo("ChallengesCount", 0x0000001F) },
		                { 0x003, new CMwFieldInfo("Medals", 0x0000001F) },
		                { 0x004, new CMwFieldInfo("StrTracks", 0x0000002D) },
		                { 0x005, new CMwFieldInfo("BmpBannerGrey", 0x00000005) },
		                { 0x006, new CMwFieldInfo("BmpBanner", 0x00000005) }
	                  })
	                },
	                { 0x0C6, new CMwClassInfo("CTrackManiaRaceInterface", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("Race", 0x00000005) },
		                { 0x001, new CMwFieldInfo("Challenge", 0x00000005) },
		                { 0x002, new CMwFieldInfo("Network", 0x00000005) },
		                { 0x003, new CMwMethodInfo("ChatNextPage", 0x00000000, null, null) },
		                { 0x004, new CMwMethodInfo("ChatPreviousPage", 0x00000000, null, null) },
		                { 0x005, new CMwMethodInfo("ChatFold", 0x00000000, null, null) },
		                { 0x006, new CMwMethodInfo("OnOpponentClicked", 0x00000000, null, null) },
		                { 0x007, new CMwFieldInfo("DownloadProgressAvatarsTotal", 0x00000024) },
		                { 0x008, new CMwFieldInfo("DownloadProgressAvatarsCur", 0x00000024) },
		                { 0x009, new CMwFieldInfo("DownloadProgressChallengeTotal", 0x00000024) },
		                { 0x00A, new CMwFieldInfo("DownloadProgressChallengeCur", 0x00000024) },
		                { 0x00B, new CMwFieldInfo("DownloadProgressPlayersTotal", 0x00000024) },
		                { 0x00C, new CMwFieldInfo("DownloadProgressPlayersCur", 0x00000024) },
		                { 0x00D, new CMwFieldInfo("DownloadProgressActivity", 0x00000024) },
		                { 0x00E, new CMwFieldInfo("ChatEntry", 0x0000002D) },
		                { 0x00F, new CMwFieldInfo("TimeCountDown", 0x0000001F) },
		                { 0x010, new CMwFieldInfo("AutoPlayerChangeDuration", 0x0000001F) },
		                { 0x011, new CMwFieldInfo("NoMoveStartTime", 0x0000001F) },
		                { 0x012, new CMwFieldInfo("NoMoveDurationBeforeMessage", 0x0000001F) },
		                { 0x013, new CMwFieldInfo("OffsetTimeText", 0x00000029) },
		                { 0x014, new CMwFieldInfo("CurrentRacePositionText", 0x0000002D) },
		                { 0x015, new CMwFieldInfo("WarmUpRoundCur", 0x0000001F) },
		                { 0x016, new CMwFieldInfo("WarmUpRoundCount", 0x0000001F) }
	                  })
	                },
	                { 0x0D0, new CMwClassInfo("CTrackManiaRaceAnalyzer", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("InputValue1", 0x00000005) },
		                { 0x001, new CMwFieldInfo("InputValue2", 0x00000005) },
		                { 0x002, new CMwFieldInfo("InputDeltaT1", 0x0000001F) },
		                { 0x003, new CMwFieldInfo("InputDeltaT2", 0x0000001F) },
		                { 0x004, new CMwFieldInfo("InputDeltaT3", 0x0000001F) },
		                { 0x005, new CMwFieldInfo("ValidationSeed", 0x0000001F) }
	                  })
	                },
	                { 0x0D1, new CMwClassInfo("CTrackManiaEnvironmentManager", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("PlayerInfoBaseTint", 0x00000009) },
		                { 0x001, new CMwFieldInfo("PlayerInfoBaseAlpha", 0x00000024) },
		                { 0x002, new CMwFieldInfo("PlayerInfoAltitude", 0x00000024) },
		                { 0x003, new CMwFieldInfo("PlayerInfoNameClipLengthSizeRatio", 0x00000024) },
		                { 0x004, new CMwFieldInfo("PlayerInfoLineWidthRatio", 0x00000024) },
		                { 0x005, new CMwFieldInfo("PlayerInfoForceName", 0x0000002D) },
		                { 0x006, new CMwMethodInfo("PlayerInfosClean", 0x00000000, null, null) }
	                  })
	                },
	                { 0x0D2, new CMwClassInfo("CGameControlCameraFollowAboveWater", new Dictionary<int, CMwMemberInfo>()
	                  {
	                  })
	                },
	                { 0x0D3, new CMwClassInfo("CTrackManiaEditorSimple", new Dictionary<int, CMwMemberInfo>()
	                  {
	                  })
	                },
	                { 0x0D4, new CMwClassInfo("CTrackManiaControlScores2", new Dictionary<int, CMwMemberInfo>()
	                  {
		                { 0x000, new CMwFieldInfo("ListLineCount", 0x0000001F) },
		                { 0x001, new CMwFieldInfo("ListColumnCount", 0x0000001F) },
		                { 0x002, new CMwFieldInfo("EdListCount", 0x0000001F) },
		                { 0x003, new CMwFieldInfo("CardModelPlayer", 0x00000005) },
		                { 0x004, new CMwFieldInfo("PlayerScale", 0x00000024) },
		                { 0x005, new CMwFieldInfo("HorizontalMargin", 0x00000024) },
		                { 0x006, new CMwFieldInfo("VerticalMargin", 0x00000024) },
		                { 0x007, new CMwFieldInfo("CenterMargin", 0x00000024) },
		                { 0x008, new CMwFieldInfo("IsCentered", 0x00000001) },
		                { 0x009, new CMwFieldInfo("Page", 0x0000001F) },
		                { 0x00A, new CMwFieldInfo("PageCount", 0x0000001F) },
		                { 0x00B, new CMwMethodInfo("PrevPage", 0x00000000, null, null) },
		                { 0x00C, new CMwMethodInfo("NextPage", 0x00000000, null, null) },
		                { 0x00D, new CMwFieldInfo("LabelHelpEnabled", 0x00000001) },
		                { 0x00E, new CMwFieldInfo("LabelMessageEnabled", 0x00000001) },
		                { 0x00F, new CMwFieldInfo("EFeature_Rank", 0x00000001) },
		                { 0x010, new CMwFieldInfo("EFeature_Avatar", 0x00000001) },
		                { 0x011, new CMwFieldInfo("EFeature_IsLocalPlayer", 0x00000001) },
		                { 0x012, new CMwFieldInfo("EFeature_IsSpectator", 0x00000001) },
		                { 0x013, new CMwFieldInfo("EFeature_Score", 0x00000001) },
		                { 0x014, new CMwFieldInfo("EFeature_ScoreInc", 0x00000001) },
		                { 0x015, new CMwFieldInfo("EFeature_LapScore", 0x00000001) },
		                { 0x016, new CMwFieldInfo("EFeature_LadderRank", 0x00000001) },
		                { 0x017, new CMwFieldInfo("EFeature_LadderPointsGain", 0x00000001) },
		                { 0x018, new CMwFieldInfo("EFeature_Result", 0x00000001) }
	                  })
	                }
                  })
                }
            };

            #endregion

            _classInfoByType = new Dictionary<Type, CMwClassInfo>();
            foreach (KeyValuePair<int, CMwEngineInfo> enginePair in _engines)
            {
                enginePair.Value.ID = enginePair.Key;
                foreach (CMwClassInfo classInfo in enginePair.Value.Classes)
                {
                    if (classInfo.ClassType != null)
                        _classInfoByType.Add(classInfo.ClassType, classInfo);
                }
            }

            #region Fill _classIDMapping

            _classIDMapping = new Dictionary<uint, uint>()
            {
                { 0x24003000, 0x03043000 },
                { 0x24004000, 0x03033000 },
                { 0x24005000, 0x0304E000 },
                { 0x24006000, 0x03036000 },
                { 0x24007000, 0x03057000 },
                { 0x24008000, 0x03058000 },
                { 0x24009000, 0x030D4000 },
                { 0x2400A000, 0x0301A000 },
                { 0x2400B000, 0x03044000 },
                { 0x2400C000, 0x0305B000 },
                { 0x2400D000, 0x0301F000 },
                { 0x2400E000, 0x0301D000 },
                { 0x2400F000, 0x0301E000 },
                { 0x24011000, 0x0305A000 },
                { 0x24012000, 0x030D1000 },
                { 0x24019000, 0x030CE000 },
                { 0x2401A000, 0x03039000 },
                { 0x2401B000, 0x03092000 },
                { 0x2401C000, 0x0305C000 },
                { 0x2401D000, 0x0305D000 },
                { 0x2401E000, 0x0305E000 },
                { 0x2401F000, 0x03038000 },
                { 0x24020000, 0x0304F000 },
                { 0x24021000, 0x03050000 },
                { 0x24022000, 0x03051000 },
                { 0x24023000, 0x03052000 },
                { 0x24024000, 0x03053000 },
                { 0x24025000, 0x03054000 },
                { 0x24027000, 0x0302D000 },
                { 0x24028000, 0x030CB000 },
                { 0x24029000, 0x03055000 },
                { 0x2402A000, 0x030BB000 },
                { 0x2402B000, 0x030D2000 },
                { 0x2402C000, 0x0305F000 },
                { 0x2402D000, 0x0307E000 },
                { 0x24033000, 0x030D3000 },
                { 0x24034000, 0x0308D000 },
                { 0x24038000, 0x03090000 },
                { 0x24039000, 0x0308F000 },
                { 0x2403A000, 0x03059000 },
                { 0x2403B000, 0x030CC000 },
                { 0x2403C000, 0x0301B000 },
                { 0x2403E000, 0x0301C000 },
                { 0x2403F000, 0x03093000 },
                { 0x24040000, 0x0303B000 },
                { 0x24046000, 0x03035000 },
                { 0x24047000, 0x03047000 },
                { 0x24048000, 0x030AF000 },
                { 0x24049000, 0x030E0000 },
                { 0x2404A000, 0x0308C000 },
                { 0x2404D000, 0x0308A000 },
                { 0x2404E000, 0x03002000 },
                { 0x2404F000, 0x03073000 },
                { 0x24050000, 0x0303A000 },
                { 0x24052000, 0x030AE000 },
                { 0x24053000, 0x030C9000 },
                { 0x24054000, 0x03045000 },
                { 0x24059000, 0x030B8000 },
                { 0x2405A000, 0x03080000 },
                { 0x2405D000, 0x030B1000 },
                { 0x2405E000, 0x03086000 },
                { 0x2405F000, 0x03081000 },
                { 0x24061000, 0x03078000 },
                { 0x24062000, 0x03078000 },
                { 0x24063000, 0x03087000 },
                { 0x24064000, 0x03056000 },
                { 0x24065000, 0x0307F000 },
                { 0x24066000, 0x03085000 },
                { 0x24067000, 0x030A2000 },
                { 0x24068000, 0x030A8000 },
                { 0x24069000, 0x0307C000 },
                { 0x2406A000, 0x03077000 },
                { 0x2406B000, 0x03082000 },
                { 0x2406C000, 0x030B2000 },
                { 0x2406D000, 0x03084000 },
                { 0x2406F000, 0x030A7000 },
                { 0x24070000, 0x030A0000 },
                { 0x24071000, 0x0308B000 },
                { 0x24072000, 0x03094000 },
                { 0x24073000, 0x030CD000 },
                { 0x24075000, 0x030A9000 },
                { 0x24076000, 0x03079000 },
                { 0x24077000, 0x0307A000 },
                { 0x2407A000, 0x030A1000 },
                { 0x2407B000, 0x030B3000 },
                { 0x2407C000, 0x030B4000 },
                { 0x2407D000, 0x030B5000 },
                { 0x24081000, 0x030A5000 },
                { 0x24082000, 0x030AA000 },
                { 0x24083000, 0x030AB000 },
                { 0x24084000, 0x030A3000 },
                { 0x24088000, 0x030A4000 },
                { 0x24089000, 0x030A6000 },
                { 0x2408A000, 0x030AD000 },
                { 0x2408B000, 0x0309F000 },
                { 0x24091000, 0x0307D000 },
                { 0x24094000, 0x030AC000 },
                { 0x24095000, 0x03095000 },
                { 0x24097000, 0x030DE000 },
                { 0x24098000, 0x030DF000 },
                { 0x24099000, 0x0309A000 },
                { 0x2409A000, 0x030BC000 },
                { 0x2409B000, 0x03048000 },
                { 0x240A0000, 0x0308E000 },
                { 0x240A1000, 0x030BE000 },
                { 0x240A2000, 0x0309B000 },
                { 0x240A3000, 0x0309C000 },
                { 0x240A4000, 0x030B9000 },
                { 0x240A5000, 0x030BA000 },
                { 0x240A6000, 0x030BF000 },
                { 0x240A8000, 0x030BD000 },
                { 0x240A9000, 0x030DB000 },
                { 0x240AB000, 0x0303C000 },
                { 0x240AC000, 0x030C1000 },
                { 0x240AD000, 0x03096000 },
                { 0x240AE000, 0x03097000 },
                { 0x240AF000, 0x030C3000 },
                { 0x240B0000, 0x030C4000 },
                { 0x240B1000, 0x030D0000 },
                { 0x240B2000, 0x030D7000 },
                { 0x240B3000, 0x030C6000 },
                { 0x240B4000, 0x030CF000 },
                { 0x240B6000, 0x030C0000 },
                { 0x240B7000, 0x030DC000 },
                { 0x240B8000, 0x03098000 },
                { 0x240B9000, 0x030B6000 },
                { 0x240BA000, 0x030B7000 },
                { 0x240BB000, 0x030C5000 },
                { 0x240BC000, 0x030D8000 },
                { 0x240BD000, 0x03046000 },
                { 0x240C0000, 0x03089000 },
                { 0x240C1000, 0x030DD000 },
                { 0x240C2000, 0x030D6000 },
                { 0x240C3000, 0x030C8000 },
                { 0x240C5000, 0x030D5000 },
                { 0x240C7000, 0x03088000 },
                { 0x240C8000, 0x030D9000 },
                { 0x240C9000, 0x03099000 },
                { 0x240CA000, 0x030CA000 },
                { 0x240CB000, 0x030C2000 },
                { 0x240CC000, 0x03091000 },
                { 0x240CD000, 0x030DA000 },
                { 0x240CE000, 0x030C7000 },
                { 0x240CF000, 0x03083000 },
                { 0x0900D000, 0x0900F000 },
                { 0x09063000, 0x09026000 }
            };

            #endregion

            _reverseClassIDMapping = new Dictionary<uint, uint>();
            foreach (KeyValuePair<uint, uint> pair in _classIDMapping)
            {
                if (!_reverseClassIDMapping.ContainsKey(pair.Value))
                    _reverseClassIDMapping.Add(pair.Value, pair.Key);
            }
        }
    }
}
