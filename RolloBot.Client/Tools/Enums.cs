using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RolloBot.Client.Tools
{
    public enum CaptureExecutionSpeed
    {
        /// <summary>
        /// Only use, if the tool doesn´t depend on Video
        /// </summary>
        None,
        /// <summary>
        /// Runs once every second
        /// </summary>
        ReallySlow,
        /// <summary>
        /// Runs twice every second
        /// </summary>
        Slow,
        /// <summary>
        /// Runs every 20 Frames
        /// </summary>
        Medium,
        /// <summary>
        /// Runs every 15 Frames
        /// </summary>
        Fast,
        /// <summary>
        /// Runs every second Frame
        /// </summary>
        ReallyFast,
        /// <summary>
        /// Runs every Frame
        /// </summary>
        RealTime,
        /// <summary>
        /// Executes only once, then cancels the tool
        /// </summary>
        Once
    }

    public enum ExecutionTrigger
    {
        /// <summary>
        /// Executes regardless of conditions
        /// </summary>
        None,
        /// <summary>
        /// Executes when a taken screenshot is detected
        /// </summary>
        ScreenshotTaken,



    }
}
