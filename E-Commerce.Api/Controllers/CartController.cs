using E_Commerce.Application.DTOs;
using E_Commerce.Application.Interfaces;
using E_Commerce.Domain.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace E_Commerce.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class CartController : ControllerBase
    {
        private readonly ICartService _cartService;

        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }

        /// <summary>
        /// Get current user's cart
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<ApiResponse<CartDto>>> GetCart()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return StatusCode(401, ApiResponse<CartDto>.ErrorResult("Invalid token", null, 401));
            }

            var result = await _cartService.GetCartAsync(userId);
            return StatusCode(result.StatusCode, result);
        }

        /// <summary>
        /// Add item to cart
        /// </summary>
        [HttpPost("items")]
        public async Task<ActionResult<ApiResponse<CartDto>>> AddItem([FromBody] AddCartItemDto dto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return StatusCode(401, ApiResponse<CartDto>.ErrorResult("Invalid token", null, 401));
            }

            var result = await _cartService.AddItemAsync(userId, dto);
            return StatusCode(result.StatusCode, result);
        }

        /// <summary>
        /// Update item quantity in cart
        /// </summary>
        [HttpPut("items/{productId}")]
        public async Task<ActionResult<ApiResponse<CartDto>>> UpdateItemQuantity(int productId, [FromBody] UpdateCartItemDto dto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return StatusCode(401, ApiResponse<CartDto>.ErrorResult("Invalid token", null, 401));
            }

            var result = await _cartService.UpdateItemQuantityAsync(userId, productId, dto);
            return StatusCode(result.StatusCode, result);
        }

        /// <summary>
        /// Increment item quantity by 1
        /// </summary>
        [HttpPost("items/{productId}/increment")]
        public async Task<ActionResult<ApiResponse<CartDto>>> IncrementItemQuantity(int productId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return StatusCode(401, ApiResponse<CartDto>.ErrorResult("Invalid token", null, 401));
            }

            var result = await _cartService.IncrementItemQuantityAsync(userId, productId);
            return StatusCode(result.StatusCode, result);
        }

        /// <summary>
        /// Decrement item quantity by 1 (removes item if quantity reaches 0)
        /// </summary>
        [HttpPost("items/{productId}/decrement")]
        public async Task<ActionResult<ApiResponse<CartDto>>> DecrementItemQuantity(int productId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return StatusCode(401, ApiResponse<CartDto>.ErrorResult("Invalid token", null, 401));
            }

            var result = await _cartService.DecrementItemQuantityAsync(userId, productId);
            return StatusCode(result.StatusCode, result);
        }

        /// <summary>
        /// Remove item from cart
        /// </summary>
        [HttpDelete("items/{productId}")]
        public async Task<ActionResult<ApiResponse<bool>>> RemoveItem(int productId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return StatusCode(401, ApiResponse<bool>.ErrorResult("Invalid token", null, 401));
            }

            var result = await _cartService.RemoveItemAsync(userId, productId);
            return StatusCode(result.StatusCode, result);
        }

        /// <summary>
        /// Clear all items from cart
        /// </summary>
        [HttpDelete]
        public async Task<ActionResult<ApiResponse<bool>>> ClearCart()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return StatusCode(401, ApiResponse<bool>.ErrorResult("Invalid token", null, 401));
            }

            var result = await _cartService.ClearCartAsync(userId);
            return StatusCode(result.StatusCode, result);
        }
    }
}
