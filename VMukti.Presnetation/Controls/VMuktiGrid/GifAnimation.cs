/* VMukti 2.0 -- An Open Source Video Communications Suite
*
* Copyright (C) 2008 - 2009, VMukti Solutions Pvt. Ltd.
*
* Hardik Sanghvi <hardik@vmukti.com>
*
* See http://www.vmukti.com for more information about
* the VMukti project. Please do not directly contact
* any of the maintainers of this project for assistance;
* the project provides a web site, forums and mailing lists
* for your use.

* This program is free software: you can redistribute it and/or modify
* it under the terms of the GNU General Public License as published by
* the Free Software Foundation, either version 3 of the License, or
* (at your option) any later version.

* This program is distributed in the hope that it will be useful,
* but WITHOUT ANY WARRANTY; without even the implied warranty of
* MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
* GNU General Public License for more details.

* You should have received a copy of the GNU General Public License
* along with this program. If not, see <http://www.gnu.org/licenses/>

*/

//Copyright (c) 2007 Rob Reiss

//Permission is hereby granted, free of charge, to any person
//obtaining a copy of this software and associated documentation
//files (the "Software"), to deal in the Software without
//restriction, including without limitation the rights to use,
//copy, modify, merge, publish, distribute, sublicense, and/or sell
//copies of the Software, and to permit persons to whom the
//Software is furnished to do so, subject to the following
//conditions:

//The above copyright notice and this permission notice shall be
//included in all copies or substantial portions of the Software.

//THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
//EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES
//OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
//NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT
//HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
//WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
//FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//OTHER DEALINGS IN THE SOFTWARE.

using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using System.Text;

namespace ReissSoftware
{
    class GifAnimation : Viewbox
    {
        private class GifFrame : Image
        {
            public int delayTime;
            public int disposalMethod;
            public int left;
            public int top;
            public int width;
            public int height;
        }
        // Gif Animation Fields
        private Canvas _canvas = null;

        private List<GifFrame> _frameList = null;

        private int _frameCounter = 0;
        private int _numberOfFrames = 0;

        private int _numberOfLoops = -1;
        private int _currentLoop = 0;

        private int _logicalWidth = 0;
        private int _logicalHeight = 0;

        private DispatcherTimer _frameTimer = null;

        private GifFrame _currentParseGifFrame;

        public GifAnimation()
        {
            _canvas = new Canvas();
            this.Child = _canvas;
        }

        private void Reset()
        {
            try
            {
            if (_frameList != null)
            {
                _frameList.Clear();
            }
            _frameList = null;
            _frameCounter = 0;
            _numberOfFrames = 0;
            _numberOfLoops = -1;
            _currentLoop = 0;
            _logicalWidth = 0;
            _logicalHeight = 0;
            if (_frameTimer != null)
            {
                _frameTimer.Stop();
                _frameTimer = null;
            }
        }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "Reset()", "Controls\\VMuktiGrid\\GifAnimation.cs");
            }
        }

        private void ParseGif(byte[] gifData)
        {
            try
            {
            _frameList = new List<GifFrame>();
            _currentParseGifFrame = new GifFrame();
            ParseGifDataStream(gifData, 0);
        }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "ParseGid()", "Controls\\VMuktiGrid\\GifAnimation.cs");
            }
        }
        private int ParseBlock(byte[] gifData, int offset)
        {
            try
            {
            switch (gifData[offset])
            {
                case 0x21:
                    if (gifData[offset + 1] == 0xF9)
                    {
                        return ParseGraphicControlExtension(gifData, offset);
                    }
                    else
                    {
                        return ParseExtensionBlock(gifData, offset);
                    }
                case 0x2C:
                    offset = ParseGraphicBlock(gifData, offset);
                    _frameList.Add(_currentParseGifFrame);
                    _currentParseGifFrame = new GifFrame();
                    return offset;
                case 0x3B:
                    return -1;
                default:
                    throw new Exception("GIF format incorrect: missing graphic block or special-purpose block. ");
            }
        }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "ParseBlock()", "Controls\\VMuktiGrid\\GifAnimation.cs");                
                return 0;
            }
        }
        private int ParseGraphicControlExtension(byte[] gifData, int offset)
        {
            try
            {
            int returnOffset = offset;
            // Extension Block
            int length = gifData[offset + 2];
            returnOffset = offset + length + 2 + 1;

            byte packedField = gifData[offset + 3];
            _currentParseGifFrame.disposalMethod = (packedField & 0x1C) >> 2;

            // Get DelayTime
            int delay = BitConverter.ToUInt16(gifData, offset + 4);
            _currentParseGifFrame.delayTime = delay;
            while (gifData[returnOffset] != 0x00)
            {
                returnOffset = returnOffset + gifData[returnOffset] + 1;
            }

            returnOffset++;

            return returnOffset;
        }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "ParseGraphicControlExtension()", "Controls\\VMuktiGrid\\GifAnimation.cs");                
                return 0;
            }
        }
        private int ParseLogicalScreen(byte[] gifData, int offset)
        {
            try
            {
            _logicalWidth = BitConverter.ToUInt16(gifData, offset);
            _logicalHeight = BitConverter.ToUInt16(gifData, offset + 2);

            byte packedField = gifData[offset + 4];
            bool hasGlobalColorTable = (int)(packedField & 0x80) > 0 ? true : false;

            int currentIndex = offset + 7;
            if (hasGlobalColorTable)
            {
                int colorTableLength = packedField & 0x07;
                colorTableLength = (int)Math.Pow(2, colorTableLength + 1) * 3;
                currentIndex = currentIndex + colorTableLength;
            }
            return currentIndex;
        }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "ParseLogicalScreen()", "Controls\\VMuktiGrid\\GifAnimation.cs");                
                return 0;

            }
        }
        private int ParseGraphicBlock(byte[] gifData, int offset)
        {
            try
            {
            _currentParseGifFrame.left = BitConverter.ToUInt16(gifData, offset + 1);
            _currentParseGifFrame.top = BitConverter.ToUInt16(gifData, offset + 3);
            _currentParseGifFrame.width = BitConverter.ToUInt16(gifData, offset + 5);
            _currentParseGifFrame.height = BitConverter.ToUInt16(gifData, offset + 7);
            if (_currentParseGifFrame.width > _logicalWidth)
            {
                _logicalWidth = _currentParseGifFrame.width;
            }
            if (_currentParseGifFrame.height > _logicalHeight)
            {
                _logicalHeight = _currentParseGifFrame.height;
            }
            byte packedField = gifData[offset + 9];
            bool hasLocalColorTable = (int)(packedField & 0x80) > 0 ? true : false;

            int currentIndex = offset + 9;
            if (hasLocalColorTable)
            {
                int colorTableLength = packedField & 0x07;
                colorTableLength = (int)Math.Pow(2, colorTableLength + 1) * 3;
                currentIndex = currentIndex + colorTableLength;
            }
            currentIndex++; // Skip 0x00

            currentIndex++; // Skip LZW Minimum Code Size;

            while (gifData[currentIndex] != 0x00)
            {
                int length = gifData[currentIndex];
                currentIndex = currentIndex + gifData[currentIndex];
                currentIndex++; // Skip initial size byte
            }
            currentIndex = currentIndex + 1;
            return currentIndex;
        }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "ParseGraphicBlock()", "Controls\\VMuktiGrid\\GifAnimation.cs");                
                return 0;
            }
        }
        private int ParseExtensionBlock(byte[] gifData, int offset)
        {
            try
            {
            int returnOffset = offset;
            // Extension Block
            int length = gifData[offset + 2];
            returnOffset = offset + length + 2 + 1;
            // check if netscape continousLoop extension
            if (gifData[offset + 1] == 0xFF && length > 10)
            {
                string netscape = System.Text.ASCIIEncoding.ASCII.GetString(gifData, offset + 3, 8);
                if (netscape == "NETSCAPE")
                {
                    _numberOfLoops = BitConverter.ToUInt16(gifData, offset + 16);
                    if (_numberOfLoops > 0)
                    {
                        _numberOfLoops++;
                    }
                }
            }
            while (gifData[returnOffset] != 0x00)
            {
                returnOffset = returnOffset + gifData[returnOffset] + 1;
            }

            returnOffset++;

            return returnOffset;
        }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "ParseExtensionBlock()", "Controls\\VMuktiGrid\\GifAnimation.cs");                
                return 0;
            }
        }
        private int ParseHeader(byte[] gifData, int offset)
        {
            try
            {
            string str = System.Text.ASCIIEncoding.ASCII.GetString(gifData, offset, 3);
            if (str != "GIF")
            {
                throw new Exception("Not a proper GIF file: missing GIF header");
            }
            return 6;
        }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "ParseHander()", "Controls\\VMuktiGrid\\GifAnimation.cs");                
                return 0;
            }
        }

        private void ParseGifDataStream(byte[] gifData, int offset)
        {
            try
            {
            offset = ParseHeader(gifData, offset);
            offset = ParseLogicalScreen(gifData, offset);
            while (offset != -1)
            {
                offset = ParseBlock(gifData, offset);
            }
        }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "ParseGifDataSream()", "Controls\\VMuktiGrid\\GifAnimation.cs");
            }
        }
        public void CreateGifAnimation(MemoryStream memoryStream)
        {
            try
            {
            Reset();

            byte[] gifData = memoryStream.GetBuffer();  // Use GetBuffer so that there is no memory copy

            GifBitmapDecoder decoder = new GifBitmapDecoder(memoryStream, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.Default);

            _numberOfFrames = decoder.Frames.Count;

            try
            {
                ParseGif(gifData);
            }
            catch
            {
                throw new FileFormatException("Unable to parse Gif file format.");
            }

            for (int f = 0; f < decoder.Frames.Count; f++)
            {
                _frameList[f].Source = decoder.Frames[f];
                _frameList[f].Visibility = Visibility.Hidden;
                _canvas.Children.Add(_frameList[f]);
                Canvas.SetLeft(_frameList[f], _frameList[f].left);
                Canvas.SetTop(_frameList[f], _frameList[f].top);
                Canvas.SetZIndex(_frameList[f], f);
            }
            _canvas.Height = _logicalHeight;
            _canvas.Width = _logicalWidth;

            _frameList[0].Visibility = Visibility.Visible;

            for (int i = 0; i < _frameList.Count; i++)
            {
                Console.WriteLine(_frameList[i].disposalMethod.ToString() + " " + _frameList[i].width.ToString() + " " + _frameList[i].delayTime.ToString());
            }

            if (_frameList.Count > 1)
            {
                if (_numberOfLoops == -1)
                {
                    _numberOfLoops = 1;
                }
                _frameTimer = new System.Windows.Threading.DispatcherTimer();
                _frameTimer.Tick += NextFrame;
                _frameTimer.Interval = new TimeSpan(0, 0, 0, 0, _frameList[0].delayTime * 10);
                _frameTimer.Start();
            }
        }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "CreateGifAnimation()", "Controls\\VMuktiGrid\\GifAnimation.cs");
            }
        }
        public void NextFrame()
        {
            try
            {
            NextFrame(null, null);
        }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "NextFrame()", "Controls\\VMuktiGrid\\GifAnimation.cs");
            }
        }
        public void NextFrame(object sender, EventArgs e)
        {
            try
            {
            _frameTimer.Stop();
            if (_numberOfFrames == 0) return;
            if (_frameList[_frameCounter].disposalMethod == 2)
            {
                _frameList[_frameCounter].Visibility = Visibility.Hidden;
            }
            if (_frameList[_frameCounter].disposalMethod >= 3)
            {
                _frameList[_frameCounter].Visibility = Visibility.Hidden;
            }
            _frameCounter++;

            if (_frameCounter < _numberOfFrames)
            {
                _frameList[_frameCounter].Visibility = Visibility.Visible;
                _frameTimer.Interval = new TimeSpan(0, 0, 0, 0, _frameList[_frameCounter].delayTime * 10);
                _frameTimer.Start();
            }
            else
            {
                if (_numberOfLoops != 0)
                {
                    _currentLoop++;
                }
                if (_currentLoop < _numberOfLoops || _numberOfLoops == 0)
                {
                    for (int f = 0; f < _frameList.Count; f++)
                    {
                        _frameList[f].Visibility = Visibility.Hidden;
                    }
                    _frameCounter = 0;
                    _frameList[_frameCounter].Visibility = Visibility.Visible;
                    _frameTimer.Interval = new TimeSpan(0, 0, 0, 0, _frameList[_frameCounter].delayTime * 10);
                    _frameTimer.Start();
                }
            }
        }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "NextFrame--1", "Controls\\VMuktiGrid\\GifAnimation.cs");
            }
        }
    }
}
