using Majestic.WarehouseService.Models.Misc;
using Majestic.WarehouseService.Models.v1.CreateCars.Request;
using Majestic.WarehouseService.Models.v1.ProcessCarSell.Event;
using Majestic.WarehouseService.Models.v1.UpdateCars.Request;

namespace Majestic.WarehouseService.Services.Validators.Cars.CreateCarValidator
{
    public class CarValidator : ICarValidator
    {
        public ServiceResult Validate(IEnumerable<CreateCarRequest> requests)
        {
            var result = new ServiceResult();
            foreach (var request in requests)
            {
                var validateResult = Validate(request);
                if (!validateResult.IsSuccess)
                {
                    var key = $"{validateResult.Message}-{Guid.NewGuid()}";
                    var value = validateResult.Data.Select(x => $"{x.Key}-{string.Join("; ", x.Value)}").ToList();

                    result.Data.Add(key, value);
                }
            }

            if (result.Data?.Any() == true)
            {
                return result;
            }

            return new ServiceResult(true);
        }

        public ServiceResult Validate(CreateCarRequest request)
        {
            var errors = new Dictionary<string, IList<string>>();

            #region ValidateCarName
            var carNameErrors = ValidateCarName(request.CarName);
            if (carNameErrors != null)
            {
                errors.Add("CarName", carNameErrors);
            }
            #endregion

            #region ValidateModelName
            var modelNameErrors = ValidateModelName(request.ModelName);
            if (modelNameErrors != null)
            {
                errors.Add("ModelName", modelNameErrors);
            }
            #endregion

            #region ValidateOwnerName
            var ownerNameErrors = ValidateOwnerName(request.OwnerName);
            if (ownerNameErrors != null)
            {
                errors.Add("OwnerName", ownerNameErrors);
            }
            #endregion

            #region ValidateDealersPrice
            var dealersPriceErrors = ValidateDealersPrice(request.DealersPrice, request.OwnersPrice);
            if (dealersPriceErrors != null)
            {
                errors.Add("DealersPrice", dealersPriceErrors);
            }
            #endregion

            #region ValidateOwnersPrice
            var ownersPriceErrors = ValidateOwnersPrice(request.OwnersPrice);
            if (ownersPriceErrors != null)
            {
                errors.Add("OwnersPrice", ownersPriceErrors);
            }
            #endregion

            #region ValidateDealerNote
            var dealerNotesErrors = ValidateDealerNote(request.DealerNotes);
            if (dealerNotesErrors != null)
            {
                errors.Add("DealerNotes", dealerNotesErrors);
            }
            #endregion

            if (errors.Any())
            {
                return new ServiceResult
                {
                    Message = $"Some fields was invalid for {request.CarName}-{request.OwnerName}-{request.DealersPrice}",
                    Data = errors
                };
            }

            return new ServiceResult(true);
        }

        public ServiceResult Validate(UpdateCarRequest request)
        {
            var errors = new Dictionary<string, IList<string>>();

            #region ValidateCarName
            var carNameErrors = ValidateCarName(request.CarName);
            if (carNameErrors != null)
            {
                errors.Add("CarName", carNameErrors);
            }
            #endregion

            #region ValidateModelName
            var modelNameErrors = ValidateModelName(request.ModelName);
            if (modelNameErrors != null)
            {
                errors.Add("ModelName", modelNameErrors);
            }
            #endregion

            #region ValidateOwnerName
            var ownerNameErrors = ValidateOwnerName(request.OwnerName);
            if (ownerNameErrors != null)
            {
                errors.Add("OwnerName", ownerNameErrors);
            }
            #endregion

            #region ValidateDealersPrice
            var dealersPriceErrors = ValidateDealersPrice(request.DealersPrice, request.OwnersPrice);
            if (dealersPriceErrors != null)
            {
                errors.Add("DealersPrice", dealersPriceErrors);
            }
            #endregion

            #region ValidateOwnersPrice
            var ownersPriceErrors = ValidateOwnersPrice(request.OwnersPrice);
            if (ownersPriceErrors != null)
            {
                errors.Add("OwnersPrice", ownersPriceErrors);
            }
            #endregion

            #region ValidateSellFinalPrice
            var sellFinalPriceErrors = ValidateSellFinalPrice(request.SellFinalPrice);
            if (sellFinalPriceErrors != null)
            {
                errors.Add("SellFinalPrice", sellFinalPriceErrors);
            }
            #endregion

            #region ValidateDealerNote
            var dealerNotesErrors = ValidateDealerNote(request.DealerNotes);
            if (dealerNotesErrors != null)
            {
                errors.Add("DealerNotes", dealerNotesErrors);
            }
            #endregion

            if (errors.Any())
            {
                return new ServiceResult
                {
                    Message = $"Some fields was invalid for {request.CarName}-{request.OwnerName}-{request.DealersPrice}",
                    Data = errors
                };
            }

            return new ServiceResult(true);
        }

        public ServiceResult Validate(ProcessSellCarEvent request)
        {
            var errors = new Dictionary<string, IList<string>>();

            #region ValidateNewOwnerName
            var ownerNameErrors = ValidateOwnerName(request.NewOwnerName);
            if (ownerNameErrors != null)
            {
                errors.Add("NewOwnerName", ownerNameErrors);
            }
            #endregion

            #region ValidateSellPrice
            var sellFinalPriceErrors = ValidateSellPrice(request.Amount);
            if (sellFinalPriceErrors != null)
            {
                errors.Add("Amount", sellFinalPriceErrors);
            }
            #endregion

            if (errors.Any())
            {
                return new ServiceResult
                {
                    Message = $"Some fields was invalid",
                    Data = errors
                };
            }

            return new ServiceResult(true);
        }

        #region ValidateCarName
        private List<string> ValidateCarName(string carName)
        {
            var errors = new List<string>();

            if (string.IsNullOrWhiteSpace(carName))
            {
                errors.Add("The required field");
                return errors;
            }

            const int MaxCarNameLength = 100;
            if (carName.Length > MaxCarNameLength)
            {
                errors.Add($"Car name max length is {MaxCarNameLength}");
                return errors;
            }

            return null;
        }
        #endregion

        #region ValidateModelName
        private List<string> ValidateModelName(string modelName)
        {
            var errors = new List<string>();

            if (string.IsNullOrWhiteSpace(modelName))
            {
                errors.Add("The required field");
                return errors;
            }

            const int MaxModelNameLength = 100;
            if (modelName.Length > MaxModelNameLength)
            {
                errors.Add($"Model name max length is {MaxModelNameLength}");
                return errors;
            }

            return null;
        }
        #endregion

        #region ValidateOwnerName
        private List<string> ValidateOwnerName(string ownerName)
        {
            var errors = new List<string>();

            if (string.IsNullOrWhiteSpace(ownerName))
            {
                errors.Add("The required field");
                return errors;
            }

            const int MaxOwnerNameLength = 100;
            if (ownerName.Length > MaxOwnerNameLength)
            {
                errors.Add($"Owner name max length is {MaxOwnerNameLength}");
                return errors;
            }

            return null;
        }
        #endregion

        #region ValidateDealersPrice
        private List<string> ValidateDealersPrice(decimal dealerPrice, decimal ownerPrice)
        {
            var errors = new List<string>();

            if (dealerPrice <= 0)
            {
                errors.Add("Dealer's price cannot be negative");
                return errors;
            }

            const int MaxPrice = 1_000_000;
            if (dealerPrice > MaxPrice)
            {
                errors.Add($"Dealer's rice cannot be more than {MaxPrice}");
                return errors;
            }

            if (dealerPrice > ownerPrice)
            {
                errors.Add($"Dealer's price cannot be more that owner's price");
                return errors;
            }

            return null;
        }
        #endregion

        #region ValidateOwnersPrice
        private List<string> ValidateOwnersPrice(decimal ownerPrice)
        {
            var errors = new List<string>();

            if (ownerPrice <= 0)
            {
                errors.Add("Owner's price cannot be negative");
                return errors;
            }

            const int MaxPrice = 1_000_000;
            if (ownerPrice > MaxPrice)
            {
                errors.Add($"Owner's price cannot be more than {MaxPrice}");
                return errors;
            }

            return null;
        }
        #endregion

        #region ValidateSellFinalPrice
        private List<string> ValidateSellFinalPrice(decimal? sellFinalPrice)
        {
            var errors = new List<string>();

            if (sellFinalPrice == null)
            {
                return null;
            }

            if (sellFinalPrice <= 0)
            {
                errors.Add("Sell final price cannot be negative");
                return errors;
            }

            const int MaxPrice = 1_000_000;
            if (sellFinalPrice > MaxPrice)
            {
                errors.Add($"Sell final price cannot be more than {MaxPrice}");
                return errors;
            }

            return null;
        }
        #endregion

        #region ValidateDealerNote
        private List<string> ValidateDealerNote(string dealerNotes)
        {
            var errors = new List<string>();

            if (string.IsNullOrWhiteSpace(dealerNotes))
            {
                errors.Add("Dealer's notes cannot be empty");
                return errors;
            }

            const int MaxDealersNotesLength = 1000;
            if (dealerNotes.Length > MaxDealersNotesLength)
            {
                errors.Add($"Dealer's notes cannot be more than 1000 {MaxDealersNotesLength}");
                return errors;
            }

            return null;
        }
        #endregion

        #region ValidateSellPrice
        private List<string> ValidateSellPrice(decimal sellPrice)
        {
            var errors = new List<string>();

            if (sellPrice <= 0)
            {
                errors.Add("Sell price cannot be negative");
                return errors;
            }

            const int MaxPrice = 1_000_000;
            if (sellPrice > MaxPrice)
            {
                errors.Add($"Sell's rice cannot be more than {MaxPrice}");
                return errors;
            }

            return null;
        }
        #endregion
    }
}
