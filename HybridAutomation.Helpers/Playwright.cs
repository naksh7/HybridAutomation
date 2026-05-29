using Microsoft.Playwright;

namespace HybridAutomation.Helpers
{
    /// <summary>
    /// Web browser automation using Microsoft Playwright with multi-browser support and centralized session management.
    /// Provides similar interface to Selenium with synchronous method calls and enhanced capabilities.
    /// 
    /// Enhanced with DriverManager integration for unified session management.
    /// </summary>
    public class Playwright
    {
        private IPlaywright? _playwright;
        private IBrowser? _browser;
        private IBrowserContext? _context;
        private IPage? _page;
        private PlaywrightDetails? PlaywrightDetails;
        private DriverManager? _driverManagerInstance;

        /// <summary>
        /// Initializes Playwright, browser, and creates a new page with specified browser type.
        /// Now enhanced with DriverManager integration option while maintaining full backward compatibility.
        /// </summary>
        /// <param name="browserType">Browser type: "chromium", "firefox", "webkit" (default: "chromium")</param>
        /// <param name="headless">Run browser in headless mode (default: false)</param>
        /// <param name="downloadPath">Download directory path (optional)</param>
        public void InitializeDriver(string browserType = "chromium", bool headless = false, string? downloadPath = null)
        {
            try
            {
                PlaywrightDetails = Utilities.DriverManager.CreatePlaywrightDriver(browserType, headless, downloadPath);

                _context = PlaywrightDetails.BrowserContextInstance;
                _page = PlaywrightDetails.PageInstance;
            }
            catch (Exception ex)
            {
                throw new Exception($"{ex.Message}\nInitializeDriver failed for browser: {browserType}\n{ex.StackTrace}", ex);
            }
        }
            
        /// <summary>
        /// Navigates page to the specified URL.
        /// </summary>
        /// <param name="url">Target URL to navigate to</param>
        public void NavigateToURL(string url)
        {
            try
            {
                if (_page == null)
                {
                    throw new InvalidOperationException("Page is not initialized. Call InitializeDriver first.");
                }
                _page.GotoAsync(url).GetAwaiter().GetResult();
            }
            catch (Exception ex)
            {
                throw new Exception($"{ex.Message}\nNavigateToURL failed for url: {url}\n{ex.StackTrace}", ex);
            }
        }

        /// <summary>
        /// Closes browser and terminates Playwright session with cleanup.
        /// </summary>
        public void CloseApp()
        {
            try
            {
                if (_page != null) _page.CloseAsync().GetAwaiter().GetResult();
                if (_context != null) _context.CloseAsync().GetAwaiter().GetResult();
                if (_browser != null) _browser.CloseAsync().GetAwaiter().GetResult();
                _playwright?.Dispose();
            }
            catch (Exception ex)
            {
                throw new Exception($"{ex.Message}\nCloseApp failed\n{ex.StackTrace}", ex);
            }
        }

        /// <summary>
        /// Finds and returns web element using the specified selector.
        /// </summary>
        /// <param name="selector">CSS selector, XPath, or other supported selector</param>
        /// <param name="timeout">Timeout in milliseconds (default: 30000)</param>
        /// <returns>Located web element</returns>
        public IElementHandle GetElement(string selector, int timeout = 30000)
        {
            try
            {
                if (_page == null)
                {
                    throw new InvalidOperationException("Page is not initialized. Call InitializeDriver first.");
                }
                
                var element = _page.WaitForSelectorAsync(selector, new PageWaitForSelectorOptions { Timeout = timeout }).GetAwaiter().GetResult();
                if (element == null)
                {
                    throw new Exception($"Element not found for selector: {selector}");
                }
                return element;
            }
            catch (Exception ex)
            {
                throw new Exception($"{ex.Message}\nGetElement failed for selector: {selector}\n{ex.StackTrace}", ex);
            }
        }

        /// <summary>
        /// Finds and returns all web elements matching the specified selector.
        /// </summary>
        /// <param name="selector">CSS selector, XPath, or other supported selector</param>
        /// <returns>List of all matching web elements</returns>
        public List<IElementHandle> GetElements(string selector)
        {
            try
            {
                if (_page == null)
                {
                    throw new InvalidOperationException("Page is not initialized. Call InitializeDriver first.");
                }
                
                var elements = _page.QuerySelectorAllAsync(selector).GetAwaiter().GetResult();
                return elements.ToList();
            }
            catch (Exception ex)
            {
                throw new Exception($"{ex.Message}\nGetElements failed for selector: {selector}\n{ex.StackTrace}", ex);
            }
        }

        /// <summary>
        /// Clears existing text and enters new text into web element.
        /// </summary>
        /// <param name="selector">Element selector</param>
        /// <param name="text">Text to enter into the element</param>
        /// <param name="timeout">Timeout in milliseconds (default: 30000)</param>
        public void SetText(string selector, string text, int timeout = 30000)
        {
            try
            {
                if (_page == null)
                {
                    throw new InvalidOperationException("Page is not initialized. Call InitializeDriver first.");
                }
                
                _page.FillAsync(selector, text, new PageFillOptions { Timeout = timeout }).GetAwaiter().GetResult();
            }
            catch (Exception ex)
            {
                throw new Exception($"{ex.Message}\nSetText failed for selector: {selector} and text: {text}\n{ex.StackTrace}", ex);
            }
        }

        /// <summary>
        /// Gets visible text content from web element.
        /// </summary>
        /// <param name="selector">Element selector</param>
        /// <param name="timeout">Timeout in milliseconds (default: 30000)</param>
        /// <returns>Element's visible text content</returns>
        public string GetText(string selector, int timeout = 30000)
        {
            try
            {
                if (_page == null)
                {
                    throw new InvalidOperationException("Page is not initialized. Call InitializeDriver first.");
                }
                
                var text = _page.TextContentAsync(selector, new PageTextContentOptions { Timeout = timeout }).GetAwaiter().GetResult();
                return text ?? string.Empty;
            }
            catch (Exception ex)
            {
                throw new Exception($"{ex.Message}\nGetText failed for selector: {selector}\n{ex.StackTrace}", ex);
            }
        }

        /// <summary>
        /// Clears text content from input element.
        /// </summary>
        /// <param name="selector">Element selector</param>
        /// <param name="timeout">Timeout in milliseconds (default: 30000)</param>
        public void ClearText(string selector, int timeout = 30000)
        {
            try
            {
                if (_page == null)
                {
                    throw new InvalidOperationException("Page is not initialized. Call InitializeDriver first.");
                }
                
                _page.FillAsync(selector, "", new PageFillOptions { Timeout = timeout }).GetAwaiter().GetResult();
            }
            catch (Exception ex)
            {
                throw new Exception($"{ex.Message}\nClearText failed for selector: {selector}\n{ex.StackTrace}", ex);
            }
        }

        /// <summary>
        /// Sends keyboard input to element without clearing existing content.
        /// </summary>
        /// <param name="selector">Element selector</param>
        /// <param name="keys">Keys or text to send to the element</param>
        /// <param name="timeout">Timeout in milliseconds (default: 30000)</param>
        public void SendKeys(string selector, string keys, int timeout = 30000)
        {
            try
            {
                if (_page == null)
                {
                    throw new InvalidOperationException("Page is not initialized. Call InitializeDriver first.");
                }
                
                _page.Locator(selector).PressSequentiallyAsync(keys, new LocatorPressSequentiallyOptions { Timeout = timeout }).GetAwaiter().GetResult();
            }
            catch (Exception ex)
            {
                throw new Exception($"{ex.Message}\nSendKeys failed for selector: {selector} and keys: {keys}\n{ex.StackTrace}", ex);
            }
        }

        /// <summary>
        /// Gets specified attribute value from web element.
        /// </summary>
        /// <param name="selector">Element selector</param>
        /// <param name="attributeName">Name of the attribute to retrieve</param>
        /// <param name="timeout">Timeout in milliseconds (default: 30000)</param>
        /// <returns>Attribute value or null if attribute doesn't exist</returns>
        public string? GetAttribute(string selector, string attributeName, int timeout = 30000)
        {
            try
            {
                if (_page == null)
                {
                    throw new InvalidOperationException("Page is not initialized. Call InitializeDriver first.");
                }
                
                return _page.GetAttributeAsync(selector, attributeName, new PageGetAttributeOptions { Timeout = timeout }).GetAwaiter().GetResult();
            }
            catch (Exception ex)
            {
                throw new Exception($"{ex.Message}\nGetAttribute failed for selector: {selector} and attribute: {attributeName}\n{ex.StackTrace}", ex);
            }
        }

        /// <summary>
        /// Counts the number of elements matching the specified selector.
        /// </summary>
        /// <param name="selector">Element selector</param>
        /// <returns>Total number of matching elements found</returns>
        public int GetElementCount(string selector)
        {
            try
            {
                var elements = GetElements(selector);
                return elements.Count;
            }
            catch (Exception ex)
            {
                throw new Exception($"{ex.Message}\nGetElementCount failed for selector: {selector}\n{ex.StackTrace}", ex);
            }
        }

        /// <summary>
        /// Switches focus to a different browser page/tab.
        /// </summary>
        /// <param name="pageIndex">Zero-based index of the page to switch to</param>
        public void SwitchToPage(int pageIndex)
        {
            try
            {
                if (_context == null)
                {
                    throw new InvalidOperationException("Browser context is not initialized. Call InitializeDriver first.");
                }
                
                var pages = _context.Pages;
                if (pageIndex >= 0 && pageIndex < pages.Count)
                {
                    _page = pages[pageIndex];
                    _page.BringToFrontAsync().GetAwaiter().GetResult();
                }
                else
                {
                    throw new ArgumentOutOfRangeException(nameof(pageIndex), $"Page index {pageIndex} is out of range. Available pages: {pages.Count}");
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"{ex.Message}\nSwitchToPage failed for pageIndex: {pageIndex}\n{ex.StackTrace}", ex);
            }
        }

        /// <summary>
        /// Switches focus to iframe or frame element within the page.
        /// </summary>
        /// <param name="selector">Frame element selector</param>
        /// <param name="timeout">Timeout in milliseconds (default: 30000)</param>
        /// <returns>Frame object for further operations</returns>
        public IFrame SwitchToFrame(string selector, int timeout = 30000)
        {
            try
            {
                if (_page == null)
                {
                    throw new InvalidOperationException("Page is not initialized. Call InitializeDriver first.");
                }
                
                var frameElement = _page.WaitForSelectorAsync(selector, new PageWaitForSelectorOptions { Timeout = timeout }).GetAwaiter().GetResult();
                if (frameElement == null)
                {
                    throw new Exception($"Frame element not found for selector: {selector}");
                }
                
                var frame = frameElement.ContentFrameAsync().GetAwaiter().GetResult();
                if (frame == null)
                {
                    throw new Exception($"Could not get frame content for selector: {selector}");
                }
                
                return frame;
            }
            catch (Exception ex)
            {
                throw new Exception($"{ex.Message}\nSwitchToFrame failed for selector: {selector}\n{ex.StackTrace}", ex);
            }
        }

        /// <summary>
        /// Performs drag and drop operation between source and target elements.
        /// </summary>
        /// <param name="sourceSelector">Source element selector</param>
        /// <param name="targetSelector">Target element selector</param>
        /// <param name="timeout">Timeout in milliseconds (default: 30000)</param>
        public void DragAndDrop(string sourceSelector, string targetSelector, int timeout = 30000)
        {
            try
            {
                if (_page == null)
                {
                    throw new InvalidOperationException("Page is not initialized. Call InitializeDriver first.");
                }
                
                _page.DragAndDropAsync(sourceSelector, targetSelector, new PageDragAndDropOptions { Timeout = timeout }).GetAwaiter().GetResult();
            }
            catch (Exception ex)
            {
                throw new Exception($"{ex.Message}\nDragAndDrop failed from {sourceSelector} to {targetSelector}\n{ex.StackTrace}", ex);
            }
        }

        /// <summary>
        /// Moves mouse cursor to the specified element.
        /// </summary>
        /// <param name="selector">Element selector</param>
        /// <param name="timeout">Timeout in milliseconds (default: 30000)</param>
        public void MoveToElement(string selector, int timeout = 30000)
        {
            try
            {
                if (_page == null)
                {
                    throw new InvalidOperationException("Page is not initialized. Call InitializeDriver first.");
                }
                
                _page.HoverAsync(selector, new PageHoverOptions { Timeout = timeout }).GetAwaiter().GetResult();
            }
            catch (Exception ex)
            {
                throw new Exception($"{ex.Message}\nMoveToElement failed for selector: {selector}\n{ex.StackTrace}", ex);
            }
        }

        /// <summary>
        /// Scrolls page to bring element into view.
        /// </summary>
        /// <param name="selector">Element selector</param>
        /// <param name="timeout">Timeout in milliseconds (default: 30000)</param>
        public void ScrollToElement(string selector, int timeout = 30000)
        {
            try
            {
                if (_page == null)
                {
                    throw new InvalidOperationException("Page is not initialized. Call InitializeDriver first.");
                }
                
                // Use JavaScript to scroll element into view
                _page.EvaluateAsync(@"(selector) => {
                    const element = document.querySelector(selector);
                    if (element) {
                        element.scrollIntoView({ behavior: 'smooth', block: 'center' });
                    }
                }", selector).GetAwaiter().GetResult();
            }
            catch (Exception ex)
            {
                throw new Exception($"{ex.Message}\nScrollToElement failed for selector: {selector}\n{ex.StackTrace}", ex);
            }
        }

        /// <summary>
        /// Maximizes the browser window to full screen.
        /// </summary>
        public void MaximizeWindow()
        {
            try
            {
                if (_page == null)
                {
                    throw new InvalidOperationException("Page is not initialized. Call InitializeDriver first.");
                }
                
                _page.SetViewportSizeAsync(1920, 1080).GetAwaiter().GetResult(); // Set to a common full-screen size
            }
            catch (Exception ex)
            {
                throw new Exception($"{ex.Message}\nMaximizeWindow failed\n{ex.StackTrace}", ex);
            }
        }

        /// <summary>
        /// Sets the browser window size.
        /// </summary>
        /// <param name="width">Window width in pixels</param>
        /// <param name="height">Window height in pixels</param>
        public void SetWindowSize(int width, int height)
        {
            try
            {
                if (_page == null)
                {
                    throw new InvalidOperationException("Page is not initialized. Call InitializeDriver first.");
                }
                
                _page.SetViewportSizeAsync(width, height).GetAwaiter().GetResult();
            }
            catch (Exception ex)
            {
                throw new Exception($"{ex.Message}\nSetWindowSize failed with width: {width}, height: {height}\n{ex.StackTrace}", ex);
            }
        }

        /// <summary>
        /// Gets collection of all available pages in the current browser context.
        /// </summary>
        /// <returns>List of page objects</returns>
        public List<IPage> GetPages()
        {
            try
            {
                if (_context == null)
                {
                    throw new InvalidOperationException("Browser context is not initialized. Call InitializeDriver first.");
                }
                
                return _context.Pages.ToList();
            }
            catch (Exception ex)
            {
                throw new Exception($"{ex.Message}\nGetPages failed\n{ex.StackTrace}", ex);
            }
        }

        /// <summary>
        /// Closes current page (not the entire session).
        /// </summary>
        public void ClosePage()
        {
            try
            {
                if (_page != null)
                {
                    _page.CloseAsync().GetAwaiter().GetResult();
                    _page = null;
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"{ex.Message}\nClosePage failed\n{ex.StackTrace}", ex);
            }
        }

        /// <summary>
        /// Waits for page to contain specified text.
        /// </summary>
        /// <param name="text">Expected text content to wait for</param>
        /// <param name="timeoutInSeconds">Maximum wait time in seconds (default: 30)</param>
        public void WaitForText(string text, int timeoutInSeconds = 30)
        {
            try
            {
                if (_page == null)
                {
                    throw new InvalidOperationException("Page is not initialized. Call InitializeDriver first.");
                }
                
                _page.WaitForSelectorAsync($"text={text}", new PageWaitForSelectorOptions { Timeout = timeoutInSeconds * 1000 }).GetAwaiter().GetResult();
            }
            catch (Exception ex)
            {
                throw new Exception($"{ex.Message}\nWaitForText failed for text: {text} with timeout: {timeoutInSeconds} seconds\n{ex.StackTrace}", ex);
            }
        }

        /// <summary>
        /// Highlights element with red border for debugging purposes.
        /// </summary>
        /// <param name="selector">Element selector</param>
        /// <param name="timeout">Timeout in milliseconds (default: 30000)</param>
        public void HighlightElement(string selector, int timeout = 30000)
        {
            try
            {
                if (_page == null)
                {
                    throw new InvalidOperationException("Page is not initialized. Call InitializeDriver first.");
                }
                
                _page.EvaluateAsync(@"(selector) => {
                    const element = document.querySelector(selector);
                    if (element) {
                        element.style.border = '3px solid red';
                    }
                }", selector).GetAwaiter().GetResult();
            }
            catch (Exception ex)
            {
                throw new Exception($"{ex.Message}\nHighlightElement failed for selector: {selector}\n{ex.StackTrace}", ex);
            }
        }

        /// <summary>
        /// Hovers mouse cursor over element (same as MoveToElement).
        /// </summary>
        /// <param name="selector">Element selector</param>
        /// <param name="timeout">Timeout in milliseconds (default: 30000)</param>
        public void HoverOverElement(string selector, int timeout = 30000)
        {
            MoveToElement(selector, timeout);
        }

        /// <summary>
        /// Scrolls within page by pixel offset.
        /// </summary>
        /// <param name="x">Horizontal scroll offset in pixels</param>
        /// <param name="y">Vertical scroll offset in pixels</param>
        public void Scroll(int x, int y)
        {
            try
            {
                if (_page == null)
                {
                    throw new InvalidOperationException("Page is not initialized. Call InitializeDriver first.");
                }
                
                _page.EvaluateAsync($"window.scrollBy({x}, {y})").GetAwaiter().GetResult();
            }
            catch (Exception ex)
            {
                throw new Exception($"{ex.Message}\nScroll failed with x: {x}, y: {y}\n{ex.StackTrace}", ex);
            }
        }

        /// <summary>
        /// Pauses execution for specified duration.
        /// </summary>
        /// <param name="timeoutInSeconds">Wait duration in seconds (default: 10)</param>
        public void WaitForAppIdle(int timeoutInSeconds = 10)
        {
            try
            {
                System.Threading.Thread.Sleep(timeoutInSeconds * 1000);
            }
            catch (Exception ex)
            {
                throw new Exception($"{ex.Message}\nWaitForAppIdle failed with timeout: {timeoutInSeconds} seconds\n{ex.StackTrace}", ex);
            }
        }        

        /// <summary>
        /// Executes JavaScript code in the page context.
        /// </summary>
        /// <param name="script">JavaScript code to execute</param>
        /// <param name="args">Arguments to pass to the script</param>
        /// <returns>Script execution result</returns>
        public object? ExecuteJavaScript(string script, params object[] args)
        {
            try
            {
                if (_page == null)
                {
                    throw new InvalidOperationException("Page is not initialized. Call InitializeDriver first.");
                }
                
                return _page.EvaluateAsync(script, args).GetAwaiter().GetResult();
            }
            catch (Exception ex)
            {
                throw new Exception($"{ex.Message}\nExecuteJavaScript failed for script: {script}\n{ex.StackTrace}", ex);
            }
        }

        /// <summary>
        /// Gets the current page URL.
        /// </summary>
        /// <returns>Current page URL</returns>
        public string GetCurrentUrl()
        {
            try
            {
                if (_page == null)
                {
                    throw new InvalidOperationException("Page is not initialized. Call InitializeDriver first.");
                }
                
                return _page.Url;
            }
            catch (Exception ex)
            {
                throw new Exception($"{ex.Message}\nGetCurrentUrl failed\n{ex.StackTrace}", ex);
            }
        }

        /// <summary>
        /// Gets the current page title.
        /// </summary>
        /// <returns>Current page title</returns>
        public string GetTitle()
        {
            try
            {
                if (_page == null)
                {
                    throw new InvalidOperationException("Page is not initialized. Call InitializeDriver first.");
                }
                
                return _page.TitleAsync().GetAwaiter().GetResult();
            }
            catch (Exception ex)
            {
                throw new Exception($"{ex.Message}\nGetTitle failed\n{ex.StackTrace}", ex);
            }
        }

        /// <summary>
        /// Performs left-click action on web element.
        /// </summary>
        /// <param name="selector">Element selector</param>
        /// <param name="timeout">Timeout in milliseconds (default: 30000)</param>
        public void Click(string selector, int timeout = 30000)
        {
            try
            {
                if (_page == null)
                {
                    throw new InvalidOperationException("Page is not initialized. Call InitializeDriver first.");
                }
                
                _page.ClickAsync(selector, new PageClickOptions { Timeout = timeout }).GetAwaiter().GetResult();
            }
            catch (Exception ex)
            {
                throw new Exception($"{ex.Message}\nClick failed for selector: {selector}\n{ex.StackTrace}", ex);
            }
        }

        /// <summary>
        /// Performs double-click action on web element.
        /// </summary>
        /// <param name="selector">Element selector</param>
        /// <param name="timeout">Timeout in milliseconds (default: 30000)</param>
        public void DoubleClick(string selector, int timeout = 30000)
        {
            try
            {
                if (_page == null)
                {
                    throw new InvalidOperationException("Page is not initialized. Call InitializeDriver first.");
                }
                
                _page.DblClickAsync(selector, new PageDblClickOptions { Timeout = timeout }).GetAwaiter().GetResult();
            }
            catch (Exception ex)
            {
                throw new Exception($"{ex.Message}\nDoubleClick failed for selector: {selector}\n{ex.StackTrace}", ex);
            }
        }

        /// <summary>
        /// Performs right-click (context menu) action on web element.
        /// </summary>
        /// <param name="selector">Element selector</param>
        /// <param name="timeout">Timeout in milliseconds (default: 30000)</param>
        public void RightClick(string selector, int timeout = 30000)
        {
            try
            {
                if (_page == null)
                {
                    throw new InvalidOperationException("Page is not initialized. Call InitializeDriver first.");
                }
                
                _page.ClickAsync(selector, new PageClickOptions { Button = MouseButton.Right, Timeout = timeout }).GetAwaiter().GetResult();
            }
            catch (Exception ex)
            {
                throw new Exception($"{ex.Message}\nRightClick failed for selector: {selector}\n{ex.StackTrace}", ex);
            }
        }

        /// <summary>
        /// Selects dropdown option by visible text.
        /// </summary>
        /// <param name="selector">Dropdown element selector</param>
        /// <param name="text">Visible text of the option to select</param>
        /// <param name="timeout">Timeout in milliseconds (default: 30000)</param>
        public void SelectDropdownByText(string selector, string text, int timeout = 30000)
        {
            try
            {
                if (_page == null)
                {
                    throw new InvalidOperationException("Page is not initialized. Call InitializeDriver first.");
                }
                
                _page.SelectOptionAsync(selector, new SelectOptionValue { Label = text }, 
                    new PageSelectOptionOptions { Timeout = timeout }).GetAwaiter().GetResult();
            }
            catch (Exception ex)
            {
                throw new Exception($"{ex.Message}\nSelectDropdownByText failed for selector: {selector} and text: {text}\n{ex.StackTrace}", ex);
            }
        }

        /// <summary>
        /// Selects dropdown option by index position.
        /// </summary>
        /// <param name="selector">Dropdown element selector</param>
        /// <param name="index">Zero-based index of the option to select</param>
        /// <param name="timeout">Timeout in milliseconds (default: 30000)</param>
        public void SelectDropdownByIndex(string selector, int index, int timeout = 30000)
        {
            try
            {
                if (_page == null)
                {
                    throw new InvalidOperationException("Page is not initialized. Call InitializeDriver first.");
                }
                
                _page.SelectOptionAsync(selector, new SelectOptionValue { Index = index }, 
                    new PageSelectOptionOptions { Timeout = timeout }).GetAwaiter().GetResult();
            }
            catch (Exception ex)
            {
                throw new Exception($"{ex.Message}\nSelectDropdownByIndex failed for selector: {selector} and index: {index}\n{ex.StackTrace}", ex);
            }
        }

        /// <summary>
        /// Selects dropdown option by value attribute.
        /// </summary>
        /// <param name="selector">Dropdown element selector</param>
        /// <param name="value">Value attribute of the option to select</param>
        /// <param name="timeout">Timeout in milliseconds (default: 30000)</param>
        public void SelectDropdownByValue(string selector, string value, int timeout = 30000)
        {
            try
            {
                if (_page == null)
                {
                    throw new InvalidOperationException("Page is not initialized. Call InitializeDriver first.");
                }
                
                _page.SelectOptionAsync(selector, new SelectOptionValue { Value = value }, 
                    new PageSelectOptionOptions { Timeout = timeout }).GetAwaiter().GetResult();
            }
            catch (Exception ex)
            {
                throw new Exception($"{ex.Message}\nSelectDropdownByValue failed for selector: {selector} and value: {value}\n{ex.StackTrace}", ex);
            }
        }

        /// <summary>
        /// Waits for element to become visible.
        /// </summary>
        /// <param name="selector">Element selector</param>
        /// <param name="timeoutInSeconds">Maximum wait time in seconds (default: 30)</param>
        /// <returns>Located web element when visible</returns>
        public IElementHandle WaitForElementVisible(string selector, int timeoutInSeconds = 30)
        {
            try
            {
                if (_page == null)
                {
                    throw new InvalidOperationException("Page is not initialized. Call InitializeDriver first.");
                }
                
                var element = _page.WaitForSelectorAsync(selector, new PageWaitForSelectorOptions 
                { 
                    State = WaitForSelectorState.Visible, 
                    Timeout = timeoutInSeconds * 1000 
                }).GetAwaiter().GetResult();
                
                if (element == null)
                {
                    throw new Exception($"Element not visible for selector: {selector}");
                }
                return element;
            }
            catch (Exception ex)
            {
                throw new Exception($"{ex.Message}\nWaitForElementVisible failed for selector: {selector} with timeout: {timeoutInSeconds} seconds\n{ex.StackTrace}", ex);
            }
        }

        /// <summary>
        /// Waits for element to become clickable (attached and visible).
        /// </summary>
        /// <param name="selector">Element selector</param>
        /// <param name="timeoutInSeconds">Maximum wait time in seconds (default: 30)</param>
        /// <returns>Clickable web element when condition is met</returns>
        public IElementHandle WaitForElementClickable(string selector, int timeoutInSeconds = 30)
        {
            try
            {
                if (_page == null)
                {
                    throw new InvalidOperationException("Page is not initialized. Call InitializeDriver first.");
                }
                
                var element = _page.WaitForSelectorAsync(selector, new PageWaitForSelectorOptions 
                { 
                    State = WaitForSelectorState.Visible, 
                    Timeout = timeoutInSeconds * 1000 
                }).GetAwaiter().GetResult();
                
                if (element == null)
                {
                    throw new Exception($"Element not clickable for selector: {selector}");
                }
                return element;
            }
            catch (Exception ex)
            {
                throw new Exception($"{ex.Message}\nWaitForElementClickable failed for selector: {selector} with timeout: {timeoutInSeconds} seconds\n{ex.StackTrace}", ex);
            }
        }

        /// <summary>
        /// Waits for element to exist in DOM.
        /// </summary>
        /// <param name="selector">Element selector</param>
        /// <param name="timeoutInSeconds">Maximum wait time in seconds (default: 30)</param>
        /// <returns>Existing web element when found in DOM</returns>
        public IElementHandle WaitForElementExists(string selector, int timeoutInSeconds = 30)
        {
            try
            {
                if (_page == null)
                {
                    throw new InvalidOperationException("Page is not initialized. Call InitializeDriver first.");
                }
                
                var element = _page.WaitForSelectorAsync(selector, new PageWaitForSelectorOptions 
                { 
                    State = WaitForSelectorState.Attached, 
                    Timeout = timeoutInSeconds * 1000 
                }).GetAwaiter().GetResult();
                
                if (element == null)
                {
                    throw new Exception($"Element not found for selector: {selector}");
                }
                return element;
            }
            catch (Exception ex)
            {
                throw new Exception($"{ex.Message}\nWaitForElementExists failed for selector: {selector} with timeout: {timeoutInSeconds} seconds\n{ex.StackTrace}", ex);
            }
        }

        /// <summary>
        /// Checks if element exists in DOM without throwing exceptions.
        /// </summary>
        /// <param name="selector">Element selector</param>
        /// <returns>True if element is present, false otherwise</returns>
        public bool IsElementPresent(string selector)
        {
            try 
            { 
                if (_page == null) return false;
                var element = _page.QuerySelectorAsync(selector).GetAwaiter().GetResult();
                return element != null; 
            } 
            catch 
            { 
                return false; 
            }
        }

        /// <summary>
        /// Checks if element is enabled for user interaction without throwing exceptions.
        /// </summary>
        /// <param name="selector">Element selector</param>
        /// <returns>True if element is enabled, false otherwise</returns>
        public bool IsElementEnabled(string selector)
        {
            try 
            { 
                if (_page == null) return false;
                return _page.IsEnabledAsync(selector).GetAwaiter().GetResult(); 
            } 
            catch 
            { 
                return false; 
            }
        }

        /// <summary>
        /// Checks if element is visible on page without throwing exceptions.
        /// </summary>
        /// <param name="selector">Element selector</param>
        /// <returns>True if element is displayed, false otherwise</returns>
        public bool IsElementDisplayed(string selector)
        {
            try 
            { 
                if (_page == null) return false;
                return _page.IsVisibleAsync(selector).GetAwaiter().GetResult(); 
            } 
            catch 
            { 
                return false; 
            }
        }

    }
}
