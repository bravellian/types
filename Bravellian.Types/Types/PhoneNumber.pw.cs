// Copyright (c) Samuel McAravey
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

namespace Bravellian;

public readonly partial record struct PhoneNumber
{
    private static partial void ProcessValue(string value, out string number)
    {
        if (value == null)
        {
            throw new ArgumentNullException(nameof(value));
        }

        number = value;
    }

    //private static partial void ProcessValue(string value, out PhoneNumbers.PhoneNumber number)
    //{
    //    if (value == null)
    //    {
    //        throw new ArgumentNullException(nameof(value));
    //    }

    //    number = PhoneNumberUtil.GetInstance().Parse(value, null);
    //}
}
