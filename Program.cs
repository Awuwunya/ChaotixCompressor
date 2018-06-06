using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChaotixCompressor {
	class Program {
		private static string fin;
		private static string fout;
		private static bool test = false, reset = true, logkill = true;
		private static FileStream sin;
		private static FileStream sout;

		static void Main(string[] args) {
			if (logkill) {
				logkill = false;

				if (File.Exists(".log")) {
					File.Delete(".log");
				}
			}

			if(args.Length > 1) {
				if(args[0].Length == 2 || args[0].Length == 3) {
					if(args.Length > 2) {
						fin = args[1];
						fout = args[2];

					} else {
						fout = fin = args[1];
					}

					if (!File.Exists(fin)) {
						e("Input file '" + fin + "' does not exist.");
					}

					try {
						sin = File.OpenRead(fin);

						if(sin == null || !sin.CanRead) {
							e("Input file '" + fin + "' can not be read.");
						}

					} catch (Exception ex) {
						e("Exception occurred when creating filestream for '"+ fin + "'\n"+ ex.ToString());
					}

					try {
						if (reset) {
							sout = File.Create(fout);
						} else {
							sout = File.Open(fout, FileMode.Open, FileAccess.ReadWrite);
						}

						if (sout == null || !sout.CanWrite) {
							e("Output file '" + fout + "' can not be written.");
						}

					} catch (Exception ex) {
						e("Exception occurred when creating filestream for '" + fout + "'\n" + ex.ToString());
					}

					switch (args[0].ToLower()) {
						case "up":
							try {
								c(false);
							} catch (Exception ex) {
								e("Exception occurred when compressing the file:\n" + ex.ToString());
							}
							return;

						case "rp":
							try {
								c(true);
							} catch (Exception ex) {
								e("Exception occurred when compressing the file:\n" + ex.ToString());
							}
							return;

						case "ur":
							try {
								u2r();
							} catch (Exception ex) {
								e("Exception occurred when compressing the file:\n" + ex.ToString());
							}
							return;

						case "ru":
							try {
								r2u();
							} catch (Exception ex) {
								e("Exception occurred when compressing the file:\n" + ex.ToString());
							}
							return;

						case "pu":
							try {
								u(false);
							} catch (Exception ex) {
								e("Exception occurred when compressing the file:\n" + ex.ToString());
							}
							return;

						case "pr":
							try {
								u(true);
							} catch (Exception ex) {
								e("Exception occurred when compressing the file:\n" + ex.ToString());
							}
							return;

						case "rt":
							try {
								delTst();
								string f = fin;
								test = true;
								l("Raw -> Packed");
								Main(new string[]{ "rp", fin, "Packed.tmp" });
								l("Packed -> Unpacked");
								Main(new string[]{ "pu", "Packed.tmp", "Unpacked.tmp" });
								l("Unpacked -> Raw");
								Main(new string[]{ "ur", "Unpacked.tmp", "Raw.tmp" });
								reset = false;
								Main(new string[]{ "cp", f, "Raw.tmp" });
								test = false;
								e("");

							} catch (Exception ex) {
								e("Exception:\n" + ex.ToString());
							}
							return;

						case "rpt":
							try {
								delTst();
								string f = fin;
								test = true;
								l("Raw -> Packed");
								Main(new string[] { "rp", fin, "Packed.tmp" });
								l("Packed -> Raw");
								Main(new string[] { "pr", "Packed.tmp", "Raw.tmp" });
								reset = false;
								Main(new string[] { "cp", f, "Raw.tmp" });
								test = false;
								e("");

							} catch (Exception ex) {
								e("Exception:\n" + ex.ToString());
							}
							return;

						case "rut":
							try {
								delTst();
								string f = fin;
								test = true;
								l("Raw -> Unpacked");
								Main(new string[] { "ru", fin, "Unpacked.tmp" });
								l("Unpacked -> Raw");
								Main(new string[] { "ur", "Unpacked.tmp", "Raw.tmp" });
								reset = false;
								Main(new string[] { "cp", f, "Raw.tmp" });
								test = false;
								e("");

							} catch (Exception ex) {
								e("Exception:\n" + ex.ToString());
							}
							return;

						case "pt":
							try {
								delTst();
								string f = fin;
								test = true;
								l("Packed -> Unpacked");
								Main(new string[] { "pu", fin, "Unpacked.tmp" });
								l("Unpacked -> Raw");
								Main(new string[] { "ur", "Unpacked.tmp", "Raw.tmp" });
								l("Raw -> Packed");
								Main(new string[] { "rp", "Raw.tmp", "Packed.tmp" });
								reset = false;
								Main(new string[] { "cp", f, "Packed.tmp" });
								test = false;
								e("");

							} catch (Exception ex) {
								e("Exception:\n" + ex.ToString());
							}
							return;

						case "put":
							try {
								delTst();
								string f = fin;
								test = true;
								l("Packed -> Unpacked");
								Main(new string[] { "pu", fin, "Unpacked.tmp" });
								l("Unpacked -> Packed");
								Main(new string[] { "up", "Unpacked.tmp", "Packed.tmp" });
								reset = false;
								Main(new string[] { "cp", f, "Packed.tmp" });
								test = false;
								e("");

							} catch (Exception ex) {
								e("Exception:\n" + ex.ToString());
							}
							return;

						case "prt":
							try {
								delTst();
								string f = fin;
								test = true;
								l("Packed -> Raw");
								Main(new string[] { "pr", fin, "Raw.tmp" });
								l("Raw -> Packed");
								Main(new string[] { "rp", "Raw.tmp", "Packed.tmp" });
								reset = false;
								Main(new string[] { "cp", f, "Packed.tmp" });
								test = false;
								e("");

							} catch (Exception ex) {
								e("Exception:\n" + ex.ToString());
							}
							return;

						case "ut":
							try {
								delTst();
								string f = fin;
								test = true;
								l("Unpacked -> Packed");
								Main(new string[] { "up", fin, "Packed.tmp" });
								l("Packed -> Raw");
								Main(new string[] { "pr", "Packed.tmp", "Raw.tmp" });
								l("Raw -> Unpacked");
								Main(new string[] { "ru", "Raw.tmp", "Unpacked.tmp" });
								reset = false;
								Main(new string[] { "cp", f, "Unpacked.tmp" });
								test = false;
								e("");

							} catch (Exception ex) {
								e("Exception:\n" + ex.ToString());
							}
							return;

						case "urt":
							try {
								delTst();
								string f = fin;
								test = true;
								l("Unpacked -> Raw");
								Main(new string[] { "ur", fin, "Raw.tmp" });
								l("Raw -> Unpacked");
								Main(new string[] { "ru", "Raw.tmp", "Unpacked.tmp" });
								reset = false;
								Main(new string[] { "cp", f, "Unpacked.tmp" });
								test = false;
								e("");

							} catch (Exception ex) {
								e("Exception:\n" + ex.ToString());
							}
							return;

						case "upt":
							try {
								delTst();
								string f = fin;
								test = true;
								l("Unpacked -> Packed");
								Main(new string[] { "up", fin, "Packed.tmp" });
								l("Packed -> Unpacked");
								Main(new string[] { "pu", "Packed.tmp", "Unpacked.tmp" });
								reset = false;
								Main(new string[] { "cp", f, "Unpacked.tmp" });
								test = false;
								e("");

							} catch (Exception ex) {
								e("Exception:\n" + ex.ToString());
							}
							return;

						case "cp":
							try {
								l("Test complete!");
								l(fin + ", " + fout);

								if (sin.Length != sout.Length) {
									l(toHexString(sin.Length, 4) +", "+ toHexString(sout.Length, 4));

								} else {
									l("Size matches");
								}

								long len = Math.Min(sin.Length, sout.Length);
								for (long s = 0;s < len;s++) {
									int b1 = sin.ReadByte(), b2 = sout.ReadByte();

									if(b1 != b2) {
										l(toHexString(s, 4) + ": "+ toHexString(b1, 2) +", "+ toHexString(b2, 2));
									}
								}

							} catch (Exception ex) {
								e("Exception:\n" + ex.ToString());
							}
							return;
					}
				}

			}

			help();
		}

		private static void delTst() {
			File.Delete("Packed.tmp");
			File.Delete("Unpacked.tmp");
			File.Delete("Raw.tmp");
		}

		private static string toHexString(long res, int zeroes) {
			return "0x" + string.Format("{0:x" + zeroes + "}", res).ToUpper();
		}

		// variables for bitstream function
		// free available bits
		private static int frb = 0;
		// bits available to be read for the last byte
		private static byte sb = 0;

		// common function used to rotate the next bits "shift" times, as well as preserve next bits for concurrent operation.
		// Essentially each value read is "shift" bits large, and use the first "shift" bits
		private static byte readbitstream(int shift) {
			byte o = 0; // will be the final destination of bits

			// loops until all bits are populated
			while(--shift >= 0) {
				// fills sb with next byte if frb (available bits) is 0
				if(frb == 0) {
					byte b = rb();

					// reverse bit order of b to sb. This helps to later roll bits to o easier
					sb = (byte)(((b & 0x1) << 7) |
						((b & 0x2) << 5) |
						((b & 0x4) << 3) |
						((b & 0x8) << 1) |
						((b & 0x10) >> 1) |
						((b & 0x20) >> 3) |
						((b & 0x40) >> 5) |
						((b & 0x80) >> 7));

					frb = 8;
				}
				
				// then insert a bit from sb to correct place
				o |= (byte)((sb & 1) << shift);

				// then shift sb to get rid of the last bit we used
				sb >>= 1;

				// finally tell frb we used a bit
				frb--;
			}

			return o;
		}

		// common function used to write bits to a bitstream and then write to file
		// preserves bit order
		private static void writebitstream(int bits, int data) {
			// reverse bit order of data. This helps to later roll bits easier
			data = ((data & 0x1) << 7) |
				((data & 0x2) << 5) |
				((data & 0x4) << 3) |
				((data & 0x8) << 1) |
				((data & 0x10) >> 1) |
				((data & 0x20) >> 3) |
				((data & 0x40) >> 5) |
				((data & 0x80) >> 7);

			data >>= 8 - bits;

			// loops until all bits are populated
			while (--bits >= 0) {
				// writes sb if frb (available bits) is 0
				if (frb == 0) {
					// write it
					wb(sb);
					// reset vars
					frb = 8;
					sb = 0;
				}

				sb <<= 1;
				// then insert a bit to sb
				sb |= (byte) (data & 1);

				// shift out the used bit from input
				data >>= 1;

				// finally tell frb we used a bit
				frb--;
			}
		}

		private static void u(bool raw) {
			frb = 0;

			// $0-1 - Size of Uncompressed/Unpacked Art.
			rw();
			// $2 - Compression Marker [Always $42]
			if(rb() != 0x42){
				l("Warn: Compression marker not set to 0x42! Are you sure this is compressed format data file?");
			}

			// $3 - Initial X/Base X/Left Most possible X from Center Point.
			byte rx = rb();
			// $4 - Bitcount for X.
			byte rbx = rb();
			// $5 - Initial Y/Base Y/Top Most possible Y from Center Point.
			byte ry = rb();
			// $6 - Bitcount for Y.
			byte rby = rb();
			// $7 - Base Palette Color Index [Usually $00 unless you are not setting it in the ArtQueue Load]
			byte rc = rb();
			// $8 - Bitcount for Palette Color Entries.
			byte rbc = rb();

			// Final X Pixel Location[Sets Initial X Prior to calling BitStream]
			byte w = readbitstream(rbx);

			// Final Y Pixel Location [Sets Initial Y Prior to calling BitStream]
			byte h = readbitstream(rby);

			// now we write packed format header
			if (raw) {
				// $0-1 - Initial X
				ww(sexbw(rx));
				// $2-3 - Final X
				ww((short)(rx + w));
				// $4-5 - Initial Y
				ww((short)(ry << 8));
				// $6-7 - Final Y
				ww((short) ((ry + h) << 8));

			} else {
				// $0-1 - Initial X
				ww(sexbw(rx));
				// $2 - Compression Marker
				wb(0);
				// $3 - Final X which gets sign extended into a word later.
				wb((byte) (rx + w));
				// $4-5 - Initial Y[First byte is relevant second byte is usually always $00, example: $EE00]
				ww((short) (ry << 8));
				// $6-7 - Final Y. [First byte relevant, see Initial Y for example.]
				ww((short) ((ry + h) << 8));
			}

			// loop until end token
			while (true) {
				// get row data start and end
				byte xst = readbitstream(rbx), xnd = readbitstream(rbx);

				if (sin.Position > 0x139) {
					int x = 0;
				}

				// Byte - Row Starting X within the Initial/Final X boundary.
				wb((byte) (xst + rx));
				// Byte - Row End X within the Initial/Final X Boundary.
				wb((byte) (xnd + rx));
				
				// if start and end are same, end
				if (xst == xnd) {
					break;
				}

				// get y-offset of the row
				byte y = readbitstream(rby);

				// write row header
				if (raw) {
					// Byte - Current Y Row [Multiplied by 256/ shifted 8 bits to word.]
					wb((byte) (y + ry));

				} else {
					// Word - Current Y Row [Starts from the Initial Y, Same First byte relevant as above.]
					ww((short) ((y + ry) << 8));
				}

				// calculate pixels in the line
				int n = xnd - xst;

				// check if xst is less than xnd
				if (n < 0) {
					e("Unable to convert at " + sin.Position + "; row start '" + xst + "' is more than row end '" + xnd + "'!");
					return;
				}

				// loop for all pixels on that row
				for (;n != 0;n--) {
					// Byte - Pixels to draw, Always has to end on an even number of pixels. etc.
					wb((byte) (readbitstream(rbc) + rc));
				}
			}

			e("Unpacking complete!");
		}

		private static void u2r() {
			byte rx;
			// $0-1 - Initial X
			short t = rw();
			rx = (byte) t;
			ww(t);

			if (rb() != 0) {
				l("Warn: Compression marker not set to 0x00! Are you sure this is unpacked format data file?");
			}

			// $2-3 - Final X
			ww(sexbw(rb()));
			// $4-5 - Initial Y
			ww(rw());
			// $6-7 - Final Y
			ww(rw());

			// loop until end token
			while (true) {
				// write row header
				byte xst = rb(), xnd = rb();
				// Byte - Row Starting X within the Initial/Final X boundary.
				wb(xst);
				// Byte - Row End X within the Initial/Final X Boundary.
				wb(xnd);

				xst -= rx;
				xnd -= rx;
				// if start and end are same, end
				if (xst == xnd) {
					break;
				}

				// Byte - Current Y Row [Multiplied by 256/ shifted 8 bits to word.]
				wb((byte)(rw() >> 8));

				// calculate pixels in the line
				int n = xnd - xst;

				// check if xst is less than xnd
				if (n < 0) {
					e("Unable to convert at " + sin.Position + "; row start '" + xst + "' is more than row end '" + xnd + "'!");
					return;
				}

				// loop for all pixels on that row
				for (;n != 0;n--) {
					// Byte - Pixels to draw, Always has to end on an even number of pixels. etc.
					wb(rb());
				}
			}

			e("Conversion complete!");
		}

		private static void r2u() {
			byte rx;
			// $0-1 - Initial X
			short t = rw();
			rx = (byte) t;
			ww(t);
			// $2 - Compression Marker
			wb(0);
			// $3 - Final X which gets sign extended into a word later.
			wb((byte)rw());
			// $4-5 - Initial Y[First byte is relevant second byte is usually always $00, example: $EE00]
			ww(rw());
			// $6-7 - Final Y. [First byte relevant, see Initial Y for example.]
			ww(rw());

			// loop until end token
			while (true) {
				// write row header
				byte xst = rb(), xnd = rb();
				// Byte - Row Starting X within the Initial/Final X boundary.
				wb(xst);
				// Byte - Row End X within the Initial/Final X Boundary.
				wb(xnd);

				xst -= rx;
				xnd -= rx;

				// if start and end are same, end
				if (xst == xnd) {
					break;
				}

				// Byte - Current Y Row [Multiplied by 256/ shifted 8 bits to word.]
				ww((short) (rb() << 8));

				// calculate pixels in the line
				int n = xnd - xst;

				// check if xst is less than xnd
				if (n < 0) {
					e("Unable to convert at " + sin.Position + "; row start '" + xst + "' is more than row end '" + xnd + "'!");
					return;
				}

				// loop for all pixels on that row
				for (;n != 0;n--) {
					// Byte - Pixels to draw, Always has to end on an even number of pixels. etc.
					wb(rb());
				}
			}

			e("Conversion complete!");
		}

		private static void c(bool raw) {
			List<D> a = new List<D>();
			byte rx, ry, w, h;
			short sz = 6;

			if (raw) {
				// $0-1 - Initial X
				rx = (byte) rw();
				// $2-3 - Final X
				w = (byte) ((byte) rw() - rx);
				a.Add(new Dx(w));
				// $4-5 - Initial Y
				ry = (byte) (rw() >> 8);
				// $6-7 - Final Y
				h = (byte) ((byte) (rw() >> 8) - ry);
				a.Add(new Dy(h));

			} else {
				// $0-1 - Initial X
				rx = (byte) rw();

				if (rb() != 0) {
					l("Warn: Compression marker not set to 0x00! Are you sure this is unpacked format data file?");
				}

				// $3 - Final X which gets sign extended into a word later.
				w = (byte) (rb() - rx);
				a.Add(new Dx(w));
				// $4-5 - Initial Y[First byte is relevant second byte is usually always $00, example: $EE00]
				ry = (byte) (rw() >> 8);
				// $6-7 - Final Y. [First byte relevant, see Initial Y for example.]
				h = (byte) ((byte) (rw() >> 8) - ry);
				a.Add(new Dy(h));
			}

			// loop until end token
			while (true) {
				sz += 4;
				byte xst = (byte) (rb() - rx), xnd = (byte) (rb() - rx);

				// Byte - Row Starting X within the Initial/Final X boundary.
				a.Add(new Dx(xst));
				// Byte - Row End X within the Initial/Final X Boundary.
				a.Add(new Dx(xnd));

				if (xst == xnd) {
					break;
				}

				if (raw) {
					// Byte - Current Y Row [Multiplied by 256/ shifted 8 bits to word.]
					a.Add(new Dy(rb() - ry));

				} else {
					// Word - Current Y Row [Starts from the Initial Y, Same First byte relevant as above.]
					a.Add(new Dy((byte) (rw() >> 8) - ry));
				}

				// calculate pixels in the line
				int n = xnd - xst;

				// check if xst is less than xnd
				if (n < 0) {
					e("Unable to read at "+ sin.Position +"; row start '" + xst + "' is more than row end '" + xnd + "'!");
					return;
				}

				// loop for all pixels on that row
				for (;n != 0;n--) {
					// Byte - Pixels to draw, Always has to end on an even number of pixels. etc.
					a.Add(new Dc(rb()));
					sz++;
				}
			}

			// gather bitcounts
			long mx = 0, my = 0, mc = 0;

			foreach (D d in a) {
				if (d is Dx) {
					if (d.data() > mx) {
						mx = d.data();
					}
				} else if (d is Dy) {
					if (d.data() > my) {
						my = d.data();
					}
				} else if (d is Dc) {
					if (d.data() > mc) {
						mc = d.data();
					}
				}
			}

			l("Max value for x: " + mx);
			byte rbx = getBitCount(mx);
			l("Selected bit count for x: " + rbx);
			l("Max value for y: " + my);
			byte rby = getBitCount(my);
			l("Selected bit count for y: " + rby);
			l("Max value for c: " + mc);
			byte rbc = getBitCount(mc);
			l("Selected bit count for c: " + rbc);
			frb = 8;

			// write header
			// $0-1 - Size of Uncompressed/Unpacked Art.
			ww(sz);
			// $2 - Compression Marker [Always $42]
			wb(0x42);
			// $3 - Initial X/Base X/Left Most possible X from Center Point.
			wb(rx);
			// $4 - Bitcount for X.
			wb(rbx);
			// $5 - Initial Y/Base Y/Top Most possible Y from Center Point.
			wb(ry);
			// $6 - Bitcount for Y.
			wb(rby);
			// $7 - Base Palette Color Index [Usually $00 unless you are not setting it in the ArtQueue Load]
			wb(0);
			// $8 - Bitcount for Palette Color Entries.
			wb(rbc);

			// Final X Pixel Location[Sets Initial X Prior to calling BitStream]
			writebitstream(rbx, w);
			// Final Y Pixel Location [Sets Initial Y Prior to calling BitStream]
			writebitstream(rby, h);

			// remove first 2 things
			a.RemoveRange(0, 2);

			// loop until end token
			while (true) {
				// get row data start and end
				byte xst = (byte)a[0].data(), xnd = (byte) a[1].data();
				if(xst == xnd) {
					int x = 0;
				}

				writebitstream(rbx, xst);
				writebitstream(rbx, xnd);

				// remove first 2 entries we used
				a.RemoveRange(0, 2);

				// if start and end are same, end
				if (xst == xnd) {
					break;
				}

				// get y-offset of the row
				byte y = (byte) a[0].data();
				writebitstream(rby, y);

				// remove the entry we used
				a.RemoveAt(0);

				// calculate pixels in the line
				int n = xnd - xst;

				// check if xst is less than xnd
				if (n < 0) {
					e("Unable to pack; row start '" + xst + "' is more than row end '" + xnd + "'!");
					return;
				}

				// loop for all pixels on that row
				for (;n != 0;n--) {
					writebitstream(rbc, (byte) a[0].data());

					// remove the entry we used
					a.RemoveAt(0);
				}
			}

			// get extra bit; this ensures data is flushed properly.
			writebitstream(9 - frb, 0);
			e("Packing complete!");
		}

		private static byte getBitCount(long x) {
			for(byte i = 1, z = 1;i <= 8;i++, z = (byte)((z << 1) | 1)) {
				if(x == (x & z)) {
					return i;
				}
			}

			e("Unable to find bitcount! Max value of "+ x +" can not be represented in 8 bits!");
			return 8;
		}

		private static short sexbw(byte b) {
			if((b & 0x80) == 0) {
				return b;

			} else {
				return (short)(0xFF00 | b);
			}
		}

		private static void ww(short n) {
			wb((byte)(n >> 8));
			wb((byte)(n & 0xFF));
		}

		private static void wb(byte n) {
			sout.WriteByte(n);
		}

		private static byte rb() {
			return (byte) (sin.ReadByte());
		}

		private static short rw() {
			return (short)((sin.ReadByte() << 8) + sin.ReadByte());
		}

		private static void help() {
			l("ChaotixCompressor by Natsumi, originally created in 2016. This compressor was\n"+
				"created to convert Chaotix sprite data between different formats. While other\n" +
				"tools exist to do this, they do not do very good job at the conversion. This\n"+
				"tool fixed many of the issues with the previous tools, but I was never able to" + 
				"\nconfirm this tool works for all cases. I provide this tool and source as is,\n"+
				"and no support will be offered.\n\n"+ 
				"usage: ChaotixCompressor ft in <out>\n"+
				"  in = Absolute path for input file\n"+
				"  <out> = Absolute path for output file.\n"+
				"    If none is provided, input file path is used instead.\n" +
				"  ft = Select type conversion. f is the letter determining original type,\n"+
				"    and t is the letter determining the new format. Valid values are:\n" +
				"      p -> packed\n" +
				"      u -> unpacked\n" +
				"      r -> raw");
			Console.ReadKey();
			Environment.Exit(0);
		}

		private static void e(string v) {
			l(v);

			// close all streams
			try {
				if (sout != null) {
					sout.Flush();
					sout.Close();
				}
			} catch (Exception) { }

			try {
				if (sin != null) {
					sin.Close();
				}
			} catch (Exception) { }

			// wait for key read
			if (!test) {
				Console.ReadKey();
				Environment.Exit(0);
			}
		}

		private static void l(string v) {
			Console.WriteLine(v);
			File.AppendAllText(".log", v +'\n');
		}
	}

	internal interface D {
		long data();
	}

	internal class Dx : D {
		private long dat;
		public Dx(long d) {
			dat = d;
		}

		public long data() {
			return dat;
		}
	}

	internal class Dy : D {
		private long dat;
		public Dy(long d) {
			dat = d;
		}

		public long data() {
			return dat;
		}
	}

	internal class Dc : D {
		private long dat;
		public Dc(long d) {
			dat = d;
		}

		public long data() {
			return dat;
		}
	}
}
