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

using System.Collections;

namespace com.webjema.PanelsMonster
{
    public class HashtableScreenArguments : IScreenArguments
    {
        private Hashtable _arguments;

        public HashtableScreenArguments(Hashtable arguments)
        {
            this._arguments = arguments;
        }

        public object GetScreenArguments()
        {
            return this._arguments;
        }

        public Hashtable GetTypedScreenArguments()
        {
            return this._arguments;
        }
    } // HashtableScreenArguments
}