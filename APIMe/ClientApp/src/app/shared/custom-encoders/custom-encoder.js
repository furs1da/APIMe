"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
exports.CustomEncoder = void 0;
var CustomEncoder = /** @class */ (function () {
    function CustomEncoder() {
    }
    CustomEncoder.prototype.encodeKey = function (key) {
        return encodeURIComponent(key);
    };
    CustomEncoder.prototype.encodeValue = function (value) {
        return encodeURIComponent(value);
    };
    CustomEncoder.prototype.decodeKey = function (key) {
        return decodeURIComponent(key);
    };
    CustomEncoder.prototype.decodeValue = function (value) {
        return decodeURIComponent(value);
    };
    return CustomEncoder;
}());
exports.CustomEncoder = CustomEncoder;
//# sourceMappingURL=custom-encoder.js.map