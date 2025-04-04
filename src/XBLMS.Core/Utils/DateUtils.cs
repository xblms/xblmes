﻿using System;
using System.Globalization;
using XBLMS.Configuration;
using XBLMS.Enums;
using XBLMS.Utils;

namespace XBLMS.Core.Utils
{
    public static class DateUtils
    {
        public const string FormatStringDateTimeJoinCN = "yyyy年MM月dd日HH时mm分ss秒";
        public const string FormatStringDateTimeCN = "MM月dd日 HH:mm";
        public const string FormatStringDateOnlyCN = "yyyy年MM月dd日";

        public const string FormatStringDateTimeWithOutSpace = "yyyyMMddHHmmss";
        public const string FormatStringDateTime = "yyyy-MM-dd HH:mm:ss";
        public const string FormatStringDateOnly = "yyyy-MM-dd";
        private static readonly DateTime JanFirst1970 = new DateTime(1970, 1, 1);

        public static string ToString(string dateTimeStr)
        {
            var dateTime = TranslateUtils.ToDateTime(dateTimeStr);
            return ToString(dateTime);
        }

        public static string ToString(DateTime? dateTime)
        {
            return dateTime.HasValue ? dateTime.Value.ToString(FormatStringDateTime) : string.Empty;
        }

        public static int GetUnixTimestamp(DateTime dateTime)
        {
            return (int)dateTime.Subtract(JanFirst1970).TotalSeconds;
        }
        public static long DateTimeToUnixTimestamp(DateTime dateTime)
        {
            DateTime epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            TimeSpan timeSpan = dateTime.ToUniversalTime() - epoch;
            return (long)timeSpan.TotalMilliseconds;
        }
        public static int GetTimestamp(DateTime dateTime)
        {
            return (int)dateTime.Subtract(JanFirst1970).TotalMilliseconds;
        }

        public static DateTime GetExpiresAt(TimeSpan expiresAt)
        {
            return DateTime.UtcNow.Add(expiresAt);
        }

        public static string GetRelatedDateTimeString(DateTime dateTime)
        {
            string retrieval;
            var interval = DateTime.Now - dateTime;
            if (interval.Days > 0)
            {
                if (interval.Days >= 7 && interval.Days < 35)
                {
                    retrieval = $"{interval.Days / 7}周";
                }
                else
                {
                    retrieval = $"{interval.Days}天";
                }
            }
            else if (interval.Hours > 0)
            {
                retrieval = $"{interval.Hours}小时";
            }
            else if (interval.Minutes > 0)
            {
                retrieval = $"{interval.Minutes}分钟";
            }
            else if (interval.Seconds > 0)
            {
                retrieval = $"{interval.Seconds}秒";
            }
            else if (interval.Milliseconds > 0)
            {
                retrieval = $"{interval.Milliseconds}毫秒";
            }
            else
            {
                retrieval = "1毫秒";
            }
            return retrieval;
        }

        private static string GetDateAndTimeString(DateTime dateTime, DateFormatType dateFormat, TimeFormatType timeFormat)
        {
            return $"{GetDateString(dateTime, dateFormat)} {GetTimeString(dateTime, timeFormat)}";
        }

        public static string GetDateAndTimeString(DateTime dateTime)
        {
            if (dateTime == Constants.SqlMinValue || dateTime == DateTime.MinValue) return string.Empty;
            return GetDateAndTimeString(dateTime, DateFormatType.Day, TimeFormatType.ShortTime);
        }

        public static string GetDateString(DateTime dateTime)
        {
            if (dateTime == Constants.SqlMinValue || dateTime == DateTime.MinValue) return string.Empty;
            return GetDateString(dateTime, DateFormatType.Day);
        }

        public static string GetDateAndTimeString(DateTimeOffset? offset)
        {
            return offset.HasValue ? GetDateAndTimeString(offset.Value.DateTime) : string.Empty;
        }

        public static string GetDateString(DateTime dateTime, DateFormatType dateFormat)
        {
            var format = string.Empty;
            if (dateFormat == DateFormatType.Year)
            {
                format = "yyyy年MM月";
            }
            else if (dateFormat == DateFormatType.Month)
            {
                format = "MM月dd日";
            }
            else if (dateFormat == DateFormatType.Day)
            {
                format = "yyyy-MM-dd";
            }
            else if (dateFormat == DateFormatType.Chinese)
            {
                format = "yyyy年M月d日";
            }
            return dateTime.ToString(format);
        }

        public static string GetTimeString(DateTime dateTime)
        {
            return GetTimeString(dateTime, TimeFormatType.ShortTime);
        }

        private static string GetTimeString(DateTime dateTime, TimeFormatType timeFormat)
        {
            var retVal = string.Empty;
            if (timeFormat == TimeFormatType.LongTime)
            {
                retVal = dateTime.ToLongTimeString();
            }
            else if (timeFormat == TimeFormatType.ShortTime)
            {
                retVal = dateTime.ToShortTimeString();
            }
            return retVal;
        }

        public static bool IsSince(string val)
        {
            if (!string.IsNullOrEmpty(val))
            {
                val = StringUtils.TrimAndToLower(val);
                if (val.EndsWith("y") || val.EndsWith("m") || val.EndsWith("d") || val.EndsWith("h"))
                {
                    return true;
                }
            }
            return false;
        }

        public static int GetSinceHours(string intWithUnitString)
        {
            var hours = 0;
            if (!string.IsNullOrEmpty(intWithUnitString))
            {
                intWithUnitString = StringUtils.TrimAndToLower(intWithUnitString);
                if (intWithUnitString.EndsWith("y"))
                {
                    hours = 8760 * TranslateUtils.ToInt(intWithUnitString.TrimEnd('y'));
                }
                else if (intWithUnitString.EndsWith("m"))
                {
                    hours = 720 * TranslateUtils.ToInt(intWithUnitString.TrimEnd('m'));
                }
                else if (intWithUnitString.EndsWith("d"))
                {
                    hours = 24 * TranslateUtils.ToInt(intWithUnitString.TrimEnd('d'));
                }
                else if (intWithUnitString.EndsWith("h"))
                {
                    hours = TranslateUtils.ToInt(intWithUnitString.TrimEnd('h'));
                }
                else
                {
                    hours = TranslateUtils.ToInt(intWithUnitString);
                }
            }
            return hours;
        }

        public static bool IsSinceMinutes(string since)
        {
            if (string.IsNullOrWhiteSpace(since)) return false;
            if (StringUtils.IsNumber(since)
            || since.EndsWith("y")
            || since.EndsWith("m")
            || since.EndsWith("d")
            || since.EndsWith("h"))
            {
                var minutes = GetSinceMinutes(since);
                return minutes > 0;
            }
            return false;
        }

        public static int GetSinceMinutes(string intWithUnitString)
        {
            var minutes = 0;
            if (!string.IsNullOrEmpty(intWithUnitString))
            {
                intWithUnitString = StringUtils.TrimAndToLower(intWithUnitString);
                if (intWithUnitString.EndsWith("y"))
                {
                    minutes = 8760 * 60 * TranslateUtils.ToInt(intWithUnitString.TrimEnd('y'));
                }
                else if (intWithUnitString.EndsWith("m"))
                {
                    minutes = 720 * 60 * TranslateUtils.ToInt(intWithUnitString.TrimEnd('m'));
                }
                else if (intWithUnitString.EndsWith("d"))
                {
                    minutes = 24 * 60 * TranslateUtils.ToInt(intWithUnitString.TrimEnd('d'));
                }
                else if (intWithUnitString.EndsWith("h"))
                {
                    minutes = 60 * TranslateUtils.ToInt(intWithUnitString.TrimEnd('h'));
                }
                else
                {
                    minutes = TranslateUtils.ToInt(intWithUnitString);
                }
            }
            return minutes;
        }

        public static string Format(DateTimeOffset? offset, string formatString)
        {
            return offset.HasValue ? Format(offset.Value.DateTime, formatString) : string.Empty;
        }

        public static string Format(DateTime dateTime, string formatString)
        {
            string retrieval;
            if (!string.IsNullOrEmpty(formatString))
            {
                retrieval = formatString.IndexOf("{0:", StringComparison.Ordinal) != -1 ? string.Format(DateTimeFormatInfo.InvariantInfo, formatString, dateTime) : dateTime.ToString(formatString, DateTimeFormatInfo.InvariantInfo);
            }
            else
            {
                retrieval = GetDateString(dateTime);
            }
            return retrieval;
        }

        /// <summary>
        /// 把两个时间差，三天内的时间用今天，昨天，前天表示，后跟时间，无日期
        /// </summary>
        public static string ParseThisMoment(DateTime dateTime, DateTime currentDateTime)
        {
            string result;
            if (currentDateTime.Year == dateTime.Year && currentDateTime.Month == dateTime.Month)//如果date和当前时间年份或者月份不一致，则直接返回"yyyy-MM-dd HH:mm"格式日期
            {
                if (DateDiff("hour", dateTime, currentDateTime) <= 10)//如果date和当前时间间隔在10小时内(曾经是3小时)
                {
                    if (DateDiff("hour", dateTime, currentDateTime) > 0)
                        return DateDiff("hour", dateTime, currentDateTime) + "小时前";

                    if (DateDiff("minute", dateTime, currentDateTime) > 0)
                        return DateDiff("minute", dateTime, currentDateTime) + "分钟前";

                    if (DateDiff("second", dateTime, currentDateTime) >= 0)
                        return DateDiff("second", dateTime, currentDateTime) + "秒前";
                    else
                        return "刚才";//为了解决时间精度不够导致发帖时间问题的兼容
                }
                else
                {
                    switch (currentDateTime.Day - dateTime.Day)
                    {
                        case 0:
                            result = "今天 " + dateTime.ToString("HH") + ":" + dateTime.ToString("mm");
                            break;
                        case 1:
                            result = "昨天 " + dateTime.ToString("HH") + ":" + dateTime.ToString("mm");
                            break;
                        case 2:
                            result = "前天 " + dateTime.ToString("HH") + ":" + dateTime.ToString("mm");
                            break;
                        default:
                            result = dateTime.ToString("yyyy-MM-dd HH:mm");
                            break;
                    }
                }
            }
            else
                result = dateTime.ToString("yyyy-MM-dd HH:mm");
            return result;
        }

        /// <summary>
        /// 两个时间的差值，可以为秒，小时，天，分钟
        /// </summary>
        /// <returns></returns>
        private static long DateDiff(string interval, DateTime startDate, DateTime endDate)
        {
            long retVal = 0;
            var ts = new TimeSpan(endDate.Ticks - startDate.Ticks);
            if (interval == "second")
            {
                retVal = (long)ts.TotalSeconds;
            }
            else if (interval == "minute")
            {
                retVal = (long)ts.TotalMinutes;
            }
            else if (interval == "hour")
            {
                retVal = (long)ts.TotalHours;
            }
            else if (interval == "day")
            {
                retVal = ts.Days;
            }
            else if (interval == "week")
            {
                retVal = ts.Days / 7;
            }
            else if (interval == "month")
            {
                retVal = ts.Days / 30;
            }
            else if (interval == "quarter")
            {
                retVal = ts.Days / 30 / 3;
            }
            else if (interval == "year")
            {
                retVal = ts.Days / 365;
            }
            return retVal;
        }

        public static string SecondToHms(long duration)
        {
            TimeSpan ts = new TimeSpan(0, 0, Convert.ToInt32(duration));
            string str = "";
            if (ts.Hours > 0)
            {
                str = String.Format("{0:00}", ts.Hours) + ":" + String.Format("{0:00}", ts.Minutes) + ":" + String.Format("{0:00}", ts.Seconds);
            }
            if (ts.Hours == 0 && ts.Minutes > 0)
            {
                str = "00:" + String.Format("{0:00}", ts.Minutes) + ":" + String.Format("{0:00}", ts.Seconds);
            }
            if (ts.Hours == 0 && ts.Minutes == 0)
            {
                str = "00:00:" + String.Format("{0:00}", ts.Seconds);
            }
            return str;
        }
        public static string SecondToHmsCN(long duration)
        {
            TimeSpan ts = new TimeSpan(0, 0, Convert.ToInt32(duration));
            string str = "";
            if (ts.Hours > 0)
            {
                str = String.Format("{0:00}", ts.Hours) + "时" + String.Format("{0:00}", ts.Minutes) + "分" + String.Format("{0:00}", ts.Seconds) + "秒";
            }
            if (ts.Hours == 0 && ts.Minutes > 0)
            {
                str = "00时" + String.Format("{0:00}", ts.Minutes) + "分" + String.Format("{0:00}", ts.Seconds) + "秒";
            }
            if (ts.Hours == 0 && ts.Minutes == 0)
            {
                str = "00时00分" + String.Format("{0:00}", ts.Seconds) + "秒";
            }
            return str;
        }
    }
}
