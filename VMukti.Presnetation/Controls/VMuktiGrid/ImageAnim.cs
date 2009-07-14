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

// not being used right now, could be safely removed after safe testing.

using System;
using System.IO;
using System.Net;
using System.Security;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Resources;
using System.Windows.Threading;
using VMuktiAPI;
using System.Text;

namespace ReissSoftware
{
    public class ImageAnimExceptionRoutedEventArgs : RoutedEventArgs
    {      
        public Exception ErrorException;

        public ImageAnimExceptionRoutedEventArgs(RoutedEvent routedEvent, object obj)
            : base(routedEvent, obj)
        {
        }
    }

    class WebReadState
    {
        public WebRequest webRequest;
        public MemoryStream memoryStream;
        public Stream readStream;
        public byte[] buffer;
    }

    public class ImageAnim : System.Windows.Controls.UserControl
    {
        // Only one of the following (_gifAnimation or _image) should be non null at any given time
        private GifAnimation _gifAnimation = null;
        private Image _image = null;

        public ImageAnim()
        {
            try
            {
        }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "ImageAnim()", "Controls\\VMuktiGrid\\ImageAnim.cs");
            }
        }
        public static readonly DependencyProperty ForceGifAnimProperty = DependencyProperty.Register("ForceGifAnim", typeof(bool), typeof(ImageAnim), new FrameworkPropertyMetadata(false));
        public bool ForceGifAnim
        {
            get { return (bool)this.GetValue(ForceGifAnimProperty); }
            set { this.SetValue(ForceGifAnimProperty, value); }
        }

        public static readonly DependencyProperty SourceProperty = DependencyProperty.Register("Source", typeof(string), typeof(ImageAnim), new FrameworkPropertyMetadata("", FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender, new PropertyChangedCallback(OnSourceChanged)));
        private static void OnSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            try
            {
                ImageAnim obj = (ImageAnim)d;
                string s = (string)e.NewValue;
                obj.CreateFromSourceString(s);
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "OnSourceChange()", "Controls\\VMuktiGrid\\ImageAnim.cs");
            }
        }
        public string Source
        {
            get { return (string)this.GetValue(SourceProperty); }
            set { this.SetValue(SourceProperty, value); }
        }


        public static readonly DependencyProperty StretchProperty = DependencyProperty.Register("Stretch", typeof(Stretch), typeof(ImageAnim), new FrameworkPropertyMetadata(Stretch.Fill, FrameworkPropertyMetadataOptions.AffectsMeasure, new PropertyChangedCallback(OnStretchChanged)));
        private static void OnStretchChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            try
            {
                ImageAnim obj = (ImageAnim)d;
                Stretch s = (Stretch)e.NewValue;
                if (obj._gifAnimation != null)
                {
                    obj._gifAnimation.Stretch = s;
                }
                else if (obj._image != null)
                {
                    obj._image.Stretch = s;
                }
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "OnStretchChange()", "Controls\\VMuktiGrid\\ImageAnim.cs");
            }
        }
        public Stretch Stretch
        {
            get { return (Stretch)this.GetValue(StretchProperty); }
            set { this.SetValue(StretchProperty, value); }
        }

        public static readonly DependencyProperty StretchDirectionProperty = DependencyProperty.Register("StretchDirection", typeof(StretchDirection), typeof(ImageAnim), new FrameworkPropertyMetadata(StretchDirection.Both, FrameworkPropertyMetadataOptions.AffectsMeasure, new PropertyChangedCallback(OnStretchDirectionChanged)));
        private static void OnStretchDirectionChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            try
            {
                ImageAnim obj = (ImageAnim)d;
                StretchDirection s = (StretchDirection)e.NewValue;
                if (obj._gifAnimation != null)
                {
                    obj._gifAnimation.StretchDirection = s;
                }
                else if (obj._image != null)
                {
                    obj._image.StretchDirection = s;
                }
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "OnSrectchDirectionChange()", "Controls\\VMuktiGrid\\ImageAnim.cs");
            }
        }
        public StretchDirection StretchDirection
        {
            get { return (StretchDirection)this.GetValue(StretchDirectionProperty); }
            set { this.SetValue(StretchDirectionProperty, value); }
        }

        public delegate void ExceptionRoutedEventHandler(object sender, ImageAnimExceptionRoutedEventArgs args);

        public static readonly RoutedEvent ImageFailedEvent = EventManager.RegisterRoutedEvent("ImageFailed", RoutingStrategy.Bubble, typeof(ExceptionRoutedEventHandler), typeof(ImageAnim));

        public event ExceptionRoutedEventHandler ImageFailed
        {
            add { AddHandler(ImageFailedEvent, value); }
            remove { RemoveHandler(ImageFailedEvent, value); }
        }

        void _image_ImageFailed(object sender, ExceptionRoutedEventArgs e)
        {
            try
            {
                RaiseImageFailedEvent(e.ErrorException);
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "_Image_ImageFailed()", "Controls\\VMuktiGrid\\ImageAnim.cs");
            }
        }

        void RaiseImageFailedEvent(Exception exp)
        {
            try
            {
                ImageAnimExceptionRoutedEventArgs newArgs = new ImageAnimExceptionRoutedEventArgs(ImageFailedEvent, this);
                newArgs.ErrorException = exp;
                RaiseEvent(newArgs);
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "RaiseImageFailed()", "Controls\\VMuktiGrid\\ImageAnim.cs");
            }
        }

        private void DeletePreviousImage()
        {
            try
            {
                if (_image != null)
                {
                    this.RemoveLogicalChild(_image);
                    _image = null;
                }
                if (_gifAnimation != null)
                {
                    this.RemoveLogicalChild(_gifAnimation);
                    _gifAnimation = null;
                }
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "DeletePreviousImage()", "Controls\\VMuktiGrid\\ImageAnim.cs");
            }
        }

        private void CreateNonGifAnimationImage()
        {
            try
            {
                _image = new Image();
                _image.ImageFailed += new EventHandler<ExceptionRoutedEventArgs>(_image_ImageFailed);
                ImageSource src = (ImageSource)(new ImageSourceConverter().ConvertFromString(Source));
                _image.Source = src;
                _image.Stretch = Stretch;
                _image.StretchDirection = StretchDirection;
                this.AddChild(_image);
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "CreateNonGifAnimation()", "Controls\\VMuktiGrid\\ImageAnim.cs");
            }
        }

        private void CreateGifAnimation(MemoryStream memoryStream)
        {
            try
            {
                _gifAnimation = new GifAnimation();
                _gifAnimation.CreateGifAnimation(memoryStream);
                _gifAnimation.Stretch = Stretch;
                _gifAnimation.StretchDirection = StretchDirection;
                this.AddChild(_gifAnimation);
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "CreateGifAnimation()--1", "Controls\\VMuktiGrid\\ImageAnim.cs");
            }
        }

        private void CreateFromSourceString(string source)
        {
            try
            {
                DeletePreviousImage();
                Uri uri;
                try
                {
                    uri = new Uri(source, UriKind.RelativeOrAbsolute);
                }
                catch (Exception exp)
                {
                    RaiseImageFailedEvent(exp);
                    return;
                }
                if (source.Trim().ToUpper().EndsWith(".GIF") || ForceGifAnim)
                {
                    if (!uri.IsAbsoluteUri)
                    {
                        GetGifStreamFromPack(uri);
                    }
                    else
                    {

                        string leftPart = uri.GetLeftPart(UriPartial.Scheme);

                        if (leftPart == "http://" || leftPart == "ftp://" || leftPart == "file://")
                        {
                            GetGifStreamFromHttp(uri);
                        }
                        else if (leftPart == "pack://")
                        {
                            GetGifStreamFromPack(uri);
                        }
                        else
                        {
                            CreateNonGifAnimationImage();
                        }
                    }
                }
                else
                {
                    CreateNonGifAnimationImage();
                }
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "CreateFromSourceString()", "Controls\\VMuktiGrid\\ImageAnim.cs");
            }
        }

        private delegate void WebRequestFinishedDelegate(MemoryStream memoryStream);

        private void WebRequestFinished(MemoryStream memoryStream)
        {
            try
            {
                CreateGifAnimation(memoryStream);
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "WebRequestFinished()", "Controls\\VMuktiGrid\\ImageAnim.cs");
            }
        }

        private delegate void WebRequestErrorDelegate(Exception exp);

        private void WebRequestError(Exception exp)
        {
            try
            {
                RaiseImageFailedEvent(exp);
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "WebReqestError()", "Controls\\VMuktiGrid\\ImageAnim.cs");
            }
        }

        private void WebResponseCallback(IAsyncResult asyncResult)
        {
            try
            {
                WebReadState webReadState = (WebReadState)asyncResult.AsyncState;
                WebResponse webResponse;
                try
                {
                    webResponse = webReadState.webRequest.EndGetResponse(asyncResult);
                    webReadState.readStream = webResponse.GetResponseStream();
                    webReadState.buffer = new byte[100000];
                    webReadState.readStream.BeginRead(webReadState.buffer, 0, webReadState.buffer.Length, new AsyncCallback(WebReadCallback), webReadState);
                }
                catch (WebException exp)
                {
                    this.Dispatcher.Invoke(DispatcherPriority.Render, new WebRequestErrorDelegate(WebRequestError), exp);
                }
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "WebResponseCallBack()", "Controls\\VMuktiGrid\\ImageAnim.cs");
            }
        }

        private void WebReadCallback(IAsyncResult asyncResult)
        {
            try
            {
                WebReadState webReadState = (WebReadState)asyncResult.AsyncState;
                int count = webReadState.readStream.EndRead(asyncResult);
                if (count > 0)
                {
                    webReadState.memoryStream.Write(webReadState.buffer, 0, count);
                    try
                    {
                        webReadState.readStream.BeginRead(webReadState.buffer, 0, webReadState.buffer.Length, new AsyncCallback(WebReadCallback), webReadState);
                    }
                    catch (WebException exp)
                    {
                        this.Dispatcher.Invoke(DispatcherPriority.Render, new WebRequestErrorDelegate(WebRequestError), exp);
                    }
                }
                else
                {
                    this.Dispatcher.Invoke(DispatcherPriority.Render, new WebRequestFinishedDelegate(WebRequestFinished), webReadState.memoryStream);
                }
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "WebReadCallBack()", "Controls\\VMuktiGrid\\ImageAnim.cs");
            }
        }

        private void GetGifStreamFromHttp(Uri uri)
        {
            try
            {
                try
                {
                    WebReadState webReadState = new WebReadState();
                    webReadState.memoryStream = new MemoryStream();
                    webReadState.webRequest = WebRequest.Create(uri);
                    webReadState.webRequest.Timeout = 10000;

                    webReadState.webRequest.BeginGetResponse(new AsyncCallback(WebResponseCallback), webReadState);
                }
                catch (SecurityException)
                {
                    // Try image load, The Image class can display images from other web sites
                    CreateNonGifAnimationImage();
                }
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "GetGifStreamFromHttp()", "Controls\\VMuktiGrid\\ImageAnim.cs");
            }
        }

        private void ReadGifStreamSynch(Stream s)
        {
            try
            {
                byte[] gifData;
                MemoryStream memoryStream;
                using (s)
                {
                    memoryStream = new MemoryStream((int)s.Length);
                    BinaryReader br = new BinaryReader(s);
                    gifData = br.ReadBytes((int)s.Length);
                    memoryStream.Write(gifData, 0, (int)s.Length);
                    memoryStream.Flush();
                }
                CreateGifAnimation(memoryStream);
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "readGifStramSynch()", "Controls\\VMuktiGrid\\ImageAnim.cs");
            }
        }

        private void GetGifStreamFromPack(Uri uri)
        {
            try
            {
                try
                {
                    StreamResourceInfo streamInfo;

                    if (!uri.IsAbsoluteUri)
                    {
                        streamInfo = Application.GetContentStream(uri);
                        if (streamInfo == null)
                        {
                            streamInfo = Application.GetResourceStream(uri);
                        }
                    }
                    else
                    {
                        if (uri.GetLeftPart(UriPartial.Authority).Contains("siteoforigin"))
                        {
                            streamInfo = Application.GetRemoteStream(uri);
                        }
                        else
                        {
                            streamInfo = Application.GetContentStream(uri);
                            if (streamInfo == null)
                            {
                                streamInfo = Application.GetResourceStream(uri);
                            }
                        }
                    }
                    if (streamInfo == null)
                    {
                        throw new FileNotFoundException("Resource not found.", uri.ToString());
                    }
                    ReadGifStreamSynch(streamInfo.Stream);
                }
                catch (Exception exp)
                {
                    RaiseImageFailedEvent(exp);
                }
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "GetGifStreamFromPack()", "Controls\\VMuktiGrid\\ImageAnim.cs");
            }
        }
    }
}
