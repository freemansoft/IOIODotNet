using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IOIOLib.Component.Types
{
		public enum DigitalInputSpecMode {
			/**
			 * Pin is floating. When the pin is left disconnected the value
			 * sensed is undefined. Use this mode when an external pull-up or
			 * pull-down resistor is used or when interfacing push-pull type
			 * logic circuits.
			 */
			FLOATING,
			/**
			 * Internal pull-up resistor is used. When the pin is left
			 * disconnected, a logical "HIGH" (true) will be sensed. This is
			 * useful for interfacing with open drain circuits or for
			 * interacting with a switch connected between the pin and ground.
			 */
			PULL_UP,
			/**
			 * Internal pull-down resistor is used. When the pin is left
			 * disconnected, a logical "LOW" (false) will be sensed. This is
			 * useful for interacting with a switch connected between the pin
			 * and Vdd.
			 */
			PULL_DOWN
		}
}
