//FileResult? photo = await MediaPicker.Default.CapturePhotoAsync();

            /*
            await MediaPicker.Default.CapturePhotoAsync().ContinueWith(
                async (task) =>
                {
                    if (task.Result != null)
                    {
                        FileResult photo = task.Result;
                        if (photo != null)
                        {
                            using var s = await photo.OpenReadAsync();
                            byte[] imageBytes = new byte[s.Length];
                            await s.ReadAsync(imageBytes);

                            MainThread.BeginInvokeOnMainThread(() =>
                            {
                                myImage.Source = ImageSource.FromStream(() => new MemoryStream(imageBytes));
                            });
                        }

                        string result = "iVBORw0KGgoAAAANSUhEUgAAAFgAAABuCAIAAAAs1EPTAAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAJcEhZcwAADsMAAA7DAcdvqGQAAALsSURBVHhe7dBRkuIwDEXR3v+mGZV06QoMoe1Ysg31zs+MhZOO7s9NnEJAIaAQUAgoBPYK8fMKvxXbJQRLn+NemS1CsOsd0/N5hfUh2PLtntxwjLItDsFyDetx7ytDsFnzbtyuabE+BIc28YjhnGdZiMv7XH7wvTUhYhnDuQdPZrdYGYJDv8HHX1IIfGQIM/6GJwoBhcCCECk7pLzkSCGgEFAILAhhxtcYf8MThYBC4CNDDD7+kkJgZQjDuQdPfkcIc3mZyw++tziE4dyGZ74phGGnnq1677fbIoRh9BZXvy+EYTPH6ASXaiqYxSECK94xdYwcoxpbhDDseo57ZXYJEVj6Eb8V2ytEmLn/L4WAQkAhoBBQCCgE9g1hOE+xVwgCPOK3YruEYOm7p0ncKbVFCNY94IfDT5zLLA7Blu44if+HmBjONVaGYD/H6FUIE0PDucCaEKx1x9T9PwkxN5yzLQjBQo7RwdncxE+Gc6rZIVjFMXr05icTvxrOeeaFYAPH6JXGC4Zzkkkh+HbH6ET7HcM5w4wQfHXbdzfejGuG87DaEHysY/SX9stx03AeUxiCz3SMGnTdj8uG84CSEHydY9Ss96m4HxhdkhyCL3KMOl17Np4ynPtlhuBbHKN+1x6PpwKjTiUhOF8y8oahZ/l3GyPLGIUYpRBQCCgEFAIKAYWAQkAhoBBQCCgEFAKbhjCcZ9krBA3umE6xUQi2P+CHKbYIwd4Hv8O4MMH6ELFwYOQYzWqxOAS7OkYHZ/MKy0LEkoHRK9yoz7EgBJs5Rm+13xwxO0RsFRj9hdvFLaaGYKH+lXisssWkEOzhGHUaebbFjBCxQ2DUj+fLWtSG4NvvmF7FW2paFIbgqx2jYblvO6oKEV8cGCWpeKcpeOMBo1S8Or0v/ybhGx2jAhXvz3xdfJ/hXIm/lPe3kkPwv3pRwXAeNu/T01EiqcUHhzCUyGjx2SGMQiRTCCgEFAIKAYWAQkAhoBBQCCiEu93+AULsKLiMx2Y9AAAAAElFTkSuQmCC";

                        MainThread.BeginInvokeOnMainThread(async () =>
                        {
                            //await Navegador.EvaluateJavaScriptAsync($"setresult_takephoto('{result}')");
                            await Navegador.EvaluateJavaScriptAsync("document.getElementById('" + param2 + "').value='" + result + "';");
                            await Navegador.EvaluateJavaScriptAsync("document.getElementById('" + param2 + "Img').src='data:image/png;base64," + result + "';");
                        });
                    }
                });
                */


            //}