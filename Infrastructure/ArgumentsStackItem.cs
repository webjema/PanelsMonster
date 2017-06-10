﻿// Copyright © 2017 Nick Kavunenko. All rights reserved.
// Contact me: nick@kavunenko.com
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//    http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using com.webjema.Functional;

namespace com.webjema.PanelsMonster
{
    class ArgumentsStackItem<T>
    {
        public string ScreenName { get; set; }
        public Option<T> Arguments { get; set; }

        public ArgumentsStackItem(string screenName, Option<T> arg)
        {
            this.ScreenName = screenName;
            this.Arguments = arg;
        }
    } // ArgumentsStackItem
}