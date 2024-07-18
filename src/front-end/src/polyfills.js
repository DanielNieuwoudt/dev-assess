import { Buffer } from 'buffer';
import process from 'process';
import https from 'https-browserify';

window.Buffer = Buffer;
window.process = process;
window.https = https;